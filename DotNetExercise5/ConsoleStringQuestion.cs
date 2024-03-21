using System.Diagnostics.CodeAnalysis;

namespace DotNetExercise5
{
    internal class ConsoleStringQuestion : ConsoleParameterQuestion<string>
    {
        public ConsoleStringQuestion(string wording) : base(wording, "")
        {
        }

        protected override bool TryParse(string answer, [MaybeNullWhen(false)] out string value)
        {
           value = answer;
            return true;
        }
    }
}
