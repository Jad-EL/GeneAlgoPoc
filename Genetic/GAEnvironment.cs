using System;

namespace Genetic;
public class GAEnvironment
{
    public List<Chromosome> Population { get; private set; } = new();
    public int Generation { get; private set; } = 1;
    public Chromosome BestChromosome { get; private set; }

    private readonly double _mutationRate;
    private readonly List<Chromosome> _previousPopulation;
    private readonly char[] _validGenes;
    private readonly Func<Chromosome, double> _fitnessFunction;
    private readonly int _populationSize;
    private double _fitnessSum;
    private Random _random = new();

    public GAEnvironment(int populationSize, char[] validGenes, int dnaSize, Func<Chromosome, double> fitnessFunction, double mutationRate = 0.01)
    {
        _populationSize = populationSize;
        _fitnessFunction = fitnessFunction;
        _validGenes= validGenes;
        _mutationRate=mutationRate;
        _previousPopulation = new(populationSize);
        Population = new(populationSize);
        BestChromosome = new(dnaSize, GetRandomCharacter);

        for (int i = 0; i < populationSize; i++)
        {
            Population.Add(new Chromosome(dnaSize, GetRandomCharacter));
        }
        SortPopulation();
    }

    private char GetRandomCharacter()
    {
        int i = _random.Next(_validGenes.Length);
        return _validGenes[i];
    }

    public void NextGeneration(int newNumDna = 0)
    {
        if (Population.Count + newNumDna <= 0)
            return;

        if (Population.Count > 0)
        {
            SortPopulation();
        }

        HistorizePopulation();
        Chromosome[] matingPool = CreateMatingPool();

        for (int i = 1; i < _previousPopulation.Count; i++)
        {
            Chromosome parent1 = matingPool[_random.Next(matingPool.Length)];
            Chromosome parent2 = matingPool[_random.Next(matingPool.Length)];
            Chromosome child = parent1.CrossOver(parent2);

            child.Mutate(_mutationRate);
            Population.Add(child);
        }
        SortPopulation();
        Generation++;

    }

    private Chromosome[] CreateMatingPool()
    {
        List<Chromosome> pool = new ();
        double maxFit = BestChromosome.Fitness <= 0 ? 1 : BestChromosome.Fitness;
        foreach (var chromosome in _previousPopulation)
        {
            double fitness = chromosome.Fitness/ maxFit;
            var n = Math.Floor(fitness * 100);

            for (int i = 1; i < n; i++)
            {
                pool.Add(chromosome);
            }
        }

        return pool.ToArray();
    }

    private void HistorizePopulation()
    {
        _previousPopulation.Clear();
        _previousPopulation.AddRange(Population);
        Population = new()
        {
            _previousPopulation.First()
        };
    }

    private void SortPopulation()
    {
        _fitnessSum = 0;
        var best = Population.First();

        for (int i = 0; i < Population.Count; i++)
        {
            _fitnessSum +=  _fitnessFunction(Population[i]);

            if (Population[i].Fitness > BestChromosome.Fitness)
            {
                BestChromosome = Population[i];
            }
        }

        Population = Population.OrderByDescending(c => c.Fitness).ToList();
    }

    public void TestDisplay()
    {
        Console.WriteLine($"\n==========GENERATION {Generation}========== SCORE : {_fitnessSum} =====");
        Population.Take(50).ToList().ForEach(i =>
        {
            Console.WriteLine($"Chromosome(Individu) : {i.GetHashCode()} ---- Genes : {string.Concat(i.Genes)} ----- Score : {i.Fitness}");
        });
    }
}