
using System.Diagnostics.CodeAnalysis;

namespace DotNetExercise5
{
    internal abstract class ConsoleParameterQuestion<T>
    {

        private string Wording { get; init; }
        private string Rejection {  get; init; }
        protected abstract bool TryParse(string answer, [MaybeNullWhen(false)] out T value);
        protected ConsoleParameterQuestion(string wording, string rejection)
        {
            Wording = wording;
            Rejection = rejection;
        }
        internal T Ask()
        {
            do
            {
                Console.WriteLine(Wording);
                string input = Console.ReadLine() ?? throw new Exception("The input stream seems to have closed.");
                if (TryParse(input, out T? value))
                {
                    return value;
                }
                Console.WriteLine(Rejection);
            } while (true);

        }
    }
}
