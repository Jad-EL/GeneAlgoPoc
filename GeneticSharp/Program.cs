// See https://aka.ms/new-console-template for more information
using GeneticSharp;
using GeneticSharpPoc;

Console.WriteLine("Hello, World!");

string validGenes = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890, .-;:_!\"#%&/()=?@${[]}";
string target = "This is a test";

var chromosome = new TextChromosome(target.Length, validGenes);

var population = new Population(50, 100, chromosome);

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
    new RouletteWheelSelection(),
    new UniformCrossover(),
    new InsertionMutation());

ga.Termination = new FitnessThresholdTermination(0.9);
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




