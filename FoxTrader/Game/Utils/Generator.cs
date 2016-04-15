using System;
using System.Collections.Generic;
using System.Linq;

namespace FoxTrader.Game.Utils
{
    public static class Generator
    {
        private static readonly Dictionary<IEnumerable<int>, DiscreteDistribution> m_discreteDistributionsCache = new Dictionary<IEnumerable<int>, DiscreteDistribution>();

        public static Random Engine
        {
            get;
        } = new Random((int)DateTime.Now.Ticks & (0x0000FFFF + (int)DateTime.Now.Ticks));

        public static int RandomRange(int c_min, int c_max)
        {
            return Engine.Next(c_min, c_max);
        }

        public static int RandomDistribution(IEnumerable<int> c_enumerable)
        {
            DiscreteDistribution a_discreteDistribution;

            var a_enumerable = c_enumerable as int[] ?? c_enumerable.ToArray();

            if (!m_discreteDistributionsCache.ContainsKey(a_enumerable))
            {
                a_discreteDistribution = new DiscreteDistribution(a_enumerable);
                m_discreteDistributionsCache.Add(a_enumerable, a_discreteDistribution);
            }
            else
            {
                a_discreteDistribution = m_discreteDistributionsCache[a_enumerable];
            }

            return a_discreteDistribution.Next();
        }

        public static string Name()
        {
            var a_name = Constants.kNameGenPrefix[RandomRange(0, Constants.kNameGenPrefix.Length)];
            a_name += Constants.kNameGenSuffix[RandomRange(0, Constants.kNameGenSuffix.Length)];
            a_name += Constants.kNameGenStems[RandomRange(0, Constants.kNameGenStems.Length)];

            return UppercaseFirst(a_name);
        }

        public static string FullName()
        {
            return Name() + " " + Name();
        }

        public static string CatalogueName()
        {
            return Name() + "-" + Engine.Next(1, 257);
        }

        static string UppercaseFirst(string c_s)
        {
            if (string.IsNullOrEmpty(c_s))
            {
                return string.Empty;
            }

            var a_a = c_s.ToCharArray();
            a_a[0] = char.ToUpper(a_a[0]);

            return new string(a_a);
        }

        class DiscreteDistribution
        {
            private readonly List<int> m_accumulatedWeights;
            private readonly int m_totalWeight;

            public DiscreteDistribution(IEnumerable<int> c_weights)
            {
                var a_accumulator = 0;
                m_accumulatedWeights = c_weights.Select(
                    c_prob =>
                    {
                        var a_output = a_accumulator;
                        a_accumulator += c_prob;
                        return a_output;
                    }
                ).ToList();
                m_totalWeight = a_accumulator;
            }

            public int Next()
            {
                var a_index = m_accumulatedWeights.BinarySearch(Generator.Engine.Next(m_totalWeight));
                return (a_index >= 0) ? a_index : ~a_index - 1;
            }
        }
    }
}
