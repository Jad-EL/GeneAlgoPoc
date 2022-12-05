using GeneticSharp;

namespace GeneticSharpPoc;

public class TextChromosome : ChromosomeBase
{
    private readonly int _stringLength;
    private readonly string _validChars;

    public TextChromosome(int targetTextLength, string validChars) : base(targetTextLength)
    {
        _stringLength = targetTextLength;
        _validChars=validChars;

        for (int i = 0; i < targetTextLength; i++)
        {
            ReplaceGene(i, GenerateGene(i));
        }
    }

    public override IChromosome CreateNew()
    {
        return new TextChromosome(_stringLength, _validChars);
    }

    public override Gene GenerateGene(int geneIndex)
    {
        int rand = RandomizationProvider.Current.GetInt(0, _validChars.Length);
        return new Gene(_validChars[rand]);
    }

    public override string ToString()
    {
        return string.Concat(GetGenes().Select(g => g.Value).ToArray());
    }
}
