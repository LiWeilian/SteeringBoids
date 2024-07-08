using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AICore.GA
{
    public class Population
    {
        private Random random = new Random(unchecked((int)DateTime.Now.Ticks));
        public string PopName { get; private set; }
        public int PopSize { get; private set; }
        /// <summary>
        /// 当前代数
        /// </summary>
        public int Generation { get; set; } = 0;
        public double CrossoverRatio { get; private set; }
        public double ChromosomeMutationRatio { get; private set; }
        public List<Chromosome> Chromosomes { get; private set; }
        public Population(string popName, double xoverRatio, int popSize,
            List<Chromosome> chromosomes, double mutationRatio)
        {
            PopName = popName;
            CrossoverRatio = xoverRatio;
            PopSize = popSize;

            Chromosomes = new List<Chromosome>();
            foreach (Chromosome chrom in chromosomes)
            {
                Chromosomes.Add(chrom.GetCopy());
            }

            ChromosomeMutationRatio = mutationRatio;

            //排序、筛选
            ChromosomeSort();
            //更新染色体被选择概率
            UpdateChromosomeSelectedRatio();
        }

        public void ChromosomeMutate()
        {
            foreach (Chromosome chrom in Chromosomes)
            {
                chrom.ChromosomeMutate();
            }
        }
        public void PopulationCrossover()
        {
            int current_size = Chromosomes.Count();
            int one = -1;
            int first = 0;

            List<Chromosome> chromosomes_new = new List<Chromosome>();

            for (int mem = 0; mem < current_size; ++mem)
            {
                double select_ratio = random.Next(0, 10000);
                bool selected = select_ratio <= Chromosomes[mem].SelectedRatio * 10000;

                if (selected)
                {
                    double xover_ratio = random.Next(0, 1000) / 1000.0;
                    if (xover_ratio < CrossoverRatio)
                    {
                        ++first;
                        if (first % 2 == 0)
                        {
                            List<double> doubles1 = new List<double>();
                            List<double> doubles2 = new List<double>();
                            for (int i = 0; i < Chromosomes[one].Values.Count; i++)
                            {
                                double xc = random.Next(0, 100) / 100.0;

                                double f1 = Chromosomes[one].Values[i];
                                double f2 = Chromosomes[mem].Values[i];
                                f1 = f1 * xc + f2 * (1 - xc);
                                f2 = f2 * xc + f1 * (1 - xc);

                                doubles1.Add(f1);
                                doubles2.Add(f2);
                            }

                            chromosomes_new.Add(new Chromosome(doubles1,
                                Chromosomes[one].MutationRatio,
                                Chromosomes[one].Max,
                                Chromosomes[one].Min));
                            chromosomes_new.Add(new Chromosome(doubles2,
                                Chromosomes[mem].MutationRatio,
                                Chromosomes[mem].Max,
                                Chromosomes[mem].Min));
                        }
                        else
                        {
                            one = mem;
                        }
                    }
                }
            }

            Chromosomes.AddRange(chromosomes_new);
        }
        public void ChromosomeSort()
        {
            //按适应度倒序排序
            Chromosomes.Sort((x, y) => -x.Fitness.CompareTo(y.Fitness));
            //保留适应度较高的染色体
            int size = PopSize > 0 ? PopSize : 100;
            Chromosomes = Chromosomes.Take(size).ToList();
        }
        public void UpdateChromosomeSelectedRatio()
        {
            //按适应度排名设置选择率，1~10：100%，11~20：80%，21~40：60%，41~70：40%，71~90：20%，91~100：10%
            for (int i = 0; i < Chromosomes.Count; i++)
            {
                if (i >= 0 && i < 10)
                {
                    Chromosomes[i].SelectedRatio = 1.0;
                }
                else
                if (i >= 10 && i < 20)
                {
                    Chromosomes[i].SelectedRatio = 0.8;
                }
                else
                if (i >= 20 && i < 40)
                {
                    Chromosomes[i].SelectedRatio = 0.6;
                }
                else
                if (i >= 40 && i < 70)
                {
                    Chromosomes[i].SelectedRatio = 0.4;
                }
                else
                if (i >= 70 && i < 90)
                {
                    Chromosomes[i].SelectedRatio = 0.2;
                }
                else
                if (i >= 90 && i < 100)
                {
                    Chromosomes[i].SelectedRatio = 0.1;
                }
            }
        }
    }
}
