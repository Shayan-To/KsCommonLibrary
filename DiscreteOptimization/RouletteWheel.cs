using System;
using System.Collections.Generic;
using System.Linq;

namespace Ks.Common
{
    public class RouletteWheel
    {
        public int GenerateIndex()
        {
            var max = this._Weights[^1];
            var rnd = this.Random.NextDouble() * max;
            var (index, _) = this._Weights.BinarySearch(rnd);
            return index;
        }

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

        private double[] _Weights;
        public IEnumerable<double> Weights
        {
            set => this._Weights = value.Cumulate(0.0, (s, v) => s + v).ToArray();
        }
    }
}
