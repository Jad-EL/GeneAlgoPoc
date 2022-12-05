using Genetic;

internal class Program
{
    private static int populationSize = 100;
    private static string validGenes = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890, .-;:_!\"#%&/()=?@${[]}";
    private static string target = "This is a test";

    private static void Main(string[] args)
    {
        var env = new GAEnvironment(100, validGenes.ToArray(), target.Length, CalculateFitness, 0.01);

        while (env.BestChromosome.Fitness <= 0.95)
        {
            env.NextGeneration();
        }

        env.TestDisplay();
    }

    private static double CalculateFitness(Chromosome chromosome)
    {
        double score = 0;

        for (int i = 0; i < chromosome.Genes.Length; i++)
        {
            if (chromosome.Genes[i] == target[i])
            {
                score += 1;
            }
        }

        score /= target.Length;

        score = MathF.Pow(2, (float)score) - 1;
        chromosome.Fitness = score;
        return score;
    }
}