
namespace DotNetExercise5
{
    internal interface ITextUI
    {
        void EraseHistoryAndShow(string textToShow);
        string GetStringFromUser();
        void ShowAndWaitForReadySignal(string textToShow);
        void ShowListAndWaitForReadySignal(List<string> strings);
    }
}