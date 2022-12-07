// See https://aka.ms/new-console-template for more information
using GeneticSharp;
using GeneticSharpPoc;


string validGenes = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890, .-;:_!\"#%&/()=?@${[]}";
string target = "Hey this is a fcking test (hello world)";

var chromosome = new TextChromosome(target.Length, validGenes);

var population = new Population(100, 100, chromosome);

var fitness = new FuncFitness((c) =>
{
    var fc = c as TextChromosome;

    double score = 0;


    for (int i = 0; i < fc!.GetGenes().Length; i++)
    {
        if ((char)fc.GetGenes()[i].Value == target[i])
        {
            score += 1;
        }
    }

    score /= target.Length;

    score = MathF.Pow(2, (float)score) - 1;
    chromosome.Fitness = score;
    return score;
});


var ga = new GeneticAlgorithm(
    population,
    fitness,
    new EliteSelection(),
    new UniformCrossover(),
    new UniformMutation(true))
{
    Termination = new FitnessThresholdTermination(1)
};

double latestFitness = 0.0;

ga.GenerationRan += (object? sender, EventArgs e) =>
{
    var bestChromosome = ga.BestChromosome as TextChromosome;
    var bestFitness = bestChromosome.Fitness.Value;

    if (bestFitness != latestFitness)
    {
        latestFitness = bestFitness;
        Console.WriteLine($"Generation {ga.GenerationsNumber} ==== {bestChromosome} ==== FIT : {bestFitness}");
    }
};




ga.Start();




