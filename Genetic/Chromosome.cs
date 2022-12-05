namespace Genetic;
public class Chromosome
{
    public char[] Genes { get; set; }
    public double Fitness { get; set; }
    public readonly Func<char> _getRandomGene;
    private readonly Random _random = new();

    public Chromosome(int size, Func<char> getRandomGene)
    {
        _getRandomGene = getRandomGene;
        Genes = new char[size];
        for (int i = 0; i < Genes.Length; i++)
        {
            Genes[i] = _getRandomGene();
        }
    }

    public Chromosome CrossOver(Chromosome otherParent)
    {
        Chromosome child = new(Genes.Length, _getRandomGene);

        for (int i = 0; i < Genes.Length; i++)
        {
            child.Genes[i] = _random.NextDouble() < 0.5 ? Genes[i] : otherParent.Genes[i];
        }

        return child;
    }

    public void Mutate(double mutationRate)
    {
        for (int i = 0; i < Genes.Length; i++)
        {
            Genes[i] = _random.NextDouble() < mutationRate ? _getRandomGene() : Genes[i];
        }
    }



}

