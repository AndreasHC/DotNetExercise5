using System.Diagnostics.CodeAnalysis;

namespace DotNetExercise5
{
    internal class TextStringQuestion : TextParameterQuestion<string>
    {
        public TextStringQuestion(string wording, ITextUI textUI) : base(wording, "", textUI)
        {
        }

        protected override bool TryParse(string answer, [MaybeNullWhen(false)] out string value)
        {
           value = answer;
            return true;
        }
    }
}
