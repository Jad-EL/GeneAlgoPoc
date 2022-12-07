using GeneticSharp;
using System.Collections;
using System.Text;

namespace GeneticSharpPoc;

internal class TextBinChromosome : BinaryChromosomeBase
{
    private readonly int _targetTextLength;
    private readonly string _validChars;
    private readonly BitArray _originalValue;
    private string _originalText;

    public TextBinChromosome(int targetTextLength, string validChars) : base(targetTextLength*8)
    {
        _targetTextLength=targetTextLength;
        _validChars=validChars;

        char[] text = new char[targetTextLength];

        for (int i = 0; i < targetTextLength; i++)
        {
            int rand = RandomizationProvider.Current.GetInt(0, _validChars.Length);
            text[i] = _validChars[rand];
        }
        _originalText = string.Concat(text);
        _originalValue = new BitArray(Encoding.UTF8.GetBytes(text));

        CreateGenes();
    }

    public override IChromosome CreateNew()
    {
        return new TextBinChromosome(_targetTextLength, _validChars);
    }

    public override Gene GenerateGene(int geneIndex)
    {
        var value = _originalValue[geneIndex];

        return new Gene(value);
    }

    public override string ToString()
    {
        var array = new byte[_targetTextLength];
        var genes = GetGenes().Select(g => (bool)g.Value).ToArray();
        var bitArray = new BitArray(genes);
        bitArray.CopyTo(array, 0);
        var str = Encoding.UTF8.GetString(array);
        return str;
    }

    public override void FlipGene(int index)
    {
        var realIndex = Math.Abs(31 - index);
        var value = (bool)GetGene(realIndex).Value;

        ReplaceGene(realIndex, new Gene(!value));
    }
}
