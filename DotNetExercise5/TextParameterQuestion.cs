
using System.Diagnostics.CodeAnalysis;

namespace DotNetExercise5
{
    internal abstract class TextParameterQuestion<T>
    {

        private string Wording { get; init; }
        private string Rejection {  get; init; }
        private ITextUI TextUI { get; init; }
        protected abstract bool TryParse(string answer, [MaybeNullWhen(false)] out T value);
        protected TextParameterQuestion(string wording, string rejection, ITextUI textUI)
        {
            Wording = wording;
            Rejection = rejection;
            TextUI = textUI;
        }
        internal T Ask()
        {
            do
            {
                TextUI.EraseHistoryAndShow(Wording);
                string input = TextUI.GetStringFromUser();
                if (TryParse(input, out T? value))
                {
                    return value;
                }
                TextUI.ShowAndWaitForReadySignal(Rejection);
            } while (true);

        }
    }
}
