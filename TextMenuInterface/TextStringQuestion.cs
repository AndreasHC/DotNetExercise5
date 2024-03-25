using System.Diagnostics.CodeAnalysis;

namespace TextMenuInterface
{
    public class TextStringQuestion : TextParameterQuestion<string>
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
