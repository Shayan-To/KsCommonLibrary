using System;

namespace Ks.Common
{
    public class SimulatedAnnealing : Algorithm
    {
        public SimulatedAnnealing()
        {
            this.OnReset();
        }

        protected override bool Step()
        {
            var newModel = this.ChangeModel.Invoke(this.Model.Clone());
            var newFitness = this.FitnessFunction.Invoke(newModel);

            var takeNewModel = false;
            if (newFitness >= this.ModelFitness)
            {
                takeNewModel = true;
            }
            else
            {
                var probability = Math.Exp(-(this.ModelFitness - newFitness) / this.Temperature);
                if (this.Random.NextDouble() < probability)
                {
                    // Console.WriteLine($"% {probability * 100:G3} (- {this.ModelFitness - newFitness:G3}) (T = {this.Temperature:G3})");
                    takeNewModel = true;
                }
            }

            if (takeNewModel)
            {
                this.Model = newModel;
                this.ModelFitness = newFitness;
                this.MovedStepCount += 1;
            }
            else
            {
                this.NotMovedStepCount += 1;
            }

            if (this.TemperatureUpdatePredicate.Invoke())
            {
                this.Temperature = this.TemperatureStep.Invoke(this.Temperature);
            }

            return true;
        }

        protected override void OnReset()
        {
            this.MovedStepCount = 0;
            this.NotMovedStepCount = 0;
            this.ModelFitness = double.NegativeInfinity;
        }

        public int MovedStepCount { get; private set; }
        public int NotMovedStepCount { get; private set; }

        public Func<Model, Model> ChangeModel { get; set; }

        public double ModelFitness { get; private set; }
        public Model Model { get; set; }
        public Func<Model, double> FitnessFunction { get; set; }

        public double Temperature { get; set; }
        public Func<double, double> TemperatureStep { get; set; } = d => d;
        public Func<bool> TemperatureUpdatePredicate { get; set; } = () => true;

        private Random _Random;
        public Random Random
        {
            get
            {
                if (this._Random == null)
                {
                    this._Random = DefaultCacher<Random>.Value;
                }
                return this._Random;
            }
            set => this._Random = value;
        }
    }
}
