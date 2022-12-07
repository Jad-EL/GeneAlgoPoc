using GeneticSharp;

int numberToFind = 8998;
int min = 0;
int max = 15000;

IntegerChromosome chromosome = new(min, max);

Population population = new(50, 100, chromosome);

var fitnessFunction = new FuncFitness((c) =>
{
    var chromosome = c as IntegerChromosome;
    int targetDiff = 0;
    double a = MathF.Abs((float)(numberToFind - chromosome.ToInteger()) / (numberToFind - min));
    double score = 1 - Math.Pow(a, 2);

    return score;
});


var ga = new GeneticAlgorithm(
    population,
    fitnessFunction,
    new EliteSelection(),
    new UniformCrossover(),
    new UniformMutation(true))
{
    Termination = new FitnessStagnationTermination()
};

double latestFitness = 0.0;

ga.GenerationRan += (object? sender, EventArgs e) =>
{
    var bestChromosome = ga.BestChromosome as IntegerChromosome;
    var bestFitness = bestChromosome.Fitness.Value;

    if (bestFitness != latestFitness)
    {
        latestFitness = bestFitness;
        Console.WriteLine($"Generation {ga.GenerationsNumber} ==== {bestChromosome.ToInteger()} ==== FIT : {bestFitness}");
    }
};




ga.Start();