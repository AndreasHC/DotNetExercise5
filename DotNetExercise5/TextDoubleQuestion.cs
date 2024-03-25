using System.Diagnostics.CodeAnalysis;

namespace DotNetExercise5
{
    internal class TextDoubleQuestion : TextParameterQuestion<double>
    {
        public TextDoubleQuestion(string wording, string rejection, ITextUI textUI) : base(wording, rejection, textUI)
        {
        }

        protected override bool TryParse(string answer, [MaybeNullWhen(false)] out double value)
        {
            return double.TryParse(answer, out value);
        }
    }
}
