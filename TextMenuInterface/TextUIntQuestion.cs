using System.Diagnostics.CodeAnalysis;

namespace TextMenuInterface
{
    public class TextUIntQuestion : TextParameterQuestion<uint>
    {
        public TextUIntQuestion(string wording, string rejection, ITextUI textUI) : base(wording, rejection, textUI)
        {
        }

        protected override bool TryParse(string answer, [MaybeNullWhen(false)] out uint value)
        {
            return uint.TryParse(answer, out value);
        }
    }
}
