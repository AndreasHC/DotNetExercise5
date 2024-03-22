using System.Diagnostics.CodeAnalysis;

namespace DotNetExercise5
{
    internal class ConsoleDoubleQuestion : ConsoleParameterQuestion<double>
    {
        public ConsoleDoubleQuestion(string wording, string rejection) : base(wording, rejection)
        {
        }

        protected override bool TryParse(string answer, [MaybeNullWhen(false)] out double value)
        {
            return double.TryParse(answer, out value);
        }
    }
}
