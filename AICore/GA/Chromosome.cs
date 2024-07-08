using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AICore.GA
{
    public class Chromosome
    {
        private Random random = new Random(unchecked((int)DateTime.Now.Ticks));
        public double SelectedRatio { get; set; }
        public double Fitness { get; set; }
        public double MutationRatio { get; private set; } = 0.1f;
        public double Max { get; private set; }
        public double Min { get; private set; }
        public List<double> Values { get; private set; }
        public Chromosome(List<double> values,
            double mutationRatio, double max, double min)
        {
            Values = new List<double>();
            Values.AddRange(values);

            MutationRatio = mutationRatio;
            Max = max;
            Min = min;
        }

        public void ChromosomeMutate()
        {
            for (int i = 0; i < Values.Count; i++)
            {
                double f = Values[i];
                int r = random.Next(0, 1000);
                switch (r % 2)
                {
                    case 0:
                        f = f + MutationRatio
                            * (Max - f) * r / 1000f;

                        f = f > Max ? Max : f;
                        break;
                    case 1:
                        f = f - MutationRatio
                            * (f - Min) * r / 1000f;
                        f = f < Min ? Min : f;
                        break;
                }
                Values[i] = f;
            }
        }

        public Chromosome GetCopy()
        {
            return new Chromosome(Values,
                MutationRatio, Max, Min)
            {
                Fitness = this.Fitness,
                //FitnessParam = this.FitnessParam?.Clone(),
                SelectedRatio = this.SelectedRatio
            };
        }
    }
}
