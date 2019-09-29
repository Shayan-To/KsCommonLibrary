using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ks.Common
{
    public class LongTermMemory : IEnumerable<Model>
    {
        public LongTermMemory(SimulatedAnnealing algorithm, int memorySizeLimit)
        {
            this.MemorySizeLimit = memorySizeLimit;
            this.Algorithm = algorithm;
            algorithm.Stepped += this.AlgorithmStepped;
        }

        private void AlgorithmStepped(object sender, EventArgs e)
        {
            if (this.AddModel(this.Algorithm.Model, this.Algorithm.ModelFitness) & this.MemorySize > this.MemorySizeLimit)
            {
                this.RemoveLastModel();
            }
        }

        private bool AddModel(Model model, double fitness)
        {
            if (this.MemoryDictionary.TryGetValue(fitness, out var models))
            {
                foreach (var m in models)
                {
                    if (model.Equals(m))
                    {
                        return false;
                    }
                }
                this.MemoryDictionary[fitness] = models.Add(model.Clone());
                this.MemorySize += 1;
                return true;
            }

            this.MemoryDictionary[fitness] = new LazyList<Model>(model.Clone());
            this.MemorySize += 1;
            return true;
        }

        private void RemoveLastModel()
        {
            var min = this.MemoryDictionary.First();
            var models = min.Value;

            if ((models.Rest?.Count ?? 0) != 0)
            {
                models.Rest.RemoveAt(this.Random.Next(models.Rest.Count));
                this.MemorySize -= 1;
                return;
            }

            this.MemoryDictionary.Remove(min.Key);
            this.MemorySize -= 1;
            return;
        }

        public IEnumerator<Model> GetEnumerator()
        {
            foreach (var kv in this.MemoryDictionary.Reverse())
            {
                yield return kv.Value.First;
                if (kv.Value.Rest != null)
                {
                    for (var i = 0; i < kv.Value.Rest.Count; i++)
                    {
                        yield return kv.Value.Rest[i];
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public SimulatedAnnealing Algorithm { get; }
        public int MemorySizeLimit { get; }

        private Random _Random;
        public Random Random
        {
            get
            {
                if (this._Random == null)
                {
                    this._Random = this.Algorithm.Random;
                }
                return this._Random;
            }
            set => this._Random = value;
        }

        private readonly SortedDictionary<double, LazyList<Model>> MemoryDictionary = new SortedDictionary<double, LazyList<Model>>();
        private int MemorySize = 0;

        private struct LazyList<T> : IEnumerable<T>
        {

            public LazyList(T item)
            {
                this.First = item;
                this.Rest = null;
            }

            public T First { get; set; }
            public List<T> Rest { get; set; }

            public LazyList<T> Add(T item)
            {
                if (this.Rest == null)
                {
                    this.Rest = new List<T>();
                }
                this.Rest.Add(item);
                return this;
            }

            public IEnumerator<T> GetEnumerator()
            {
                yield return this.First;
                if (this.Rest != null)
                {
                    for (var i = 0; i < this.Rest.Count; i++)
                    {
                        yield return this.Rest[i];
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
