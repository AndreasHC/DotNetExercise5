using System.Diagnostics.CodeAnalysis;

namespace DotNetExercise5
{
    internal class ConsoleUIntQuestion : ConsoleParameterQuestion<uint>
    {
        public ConsoleUIntQuestion(string wording, string rejection) : base(wording, rejection)
        {
        }

        protected override bool TryParse(string answer, [MaybeNullWhen(false)] out uint value)
        {
            return uint.TryParse(answer, out value);
        }
    }
}
