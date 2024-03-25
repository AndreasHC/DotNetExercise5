
namespace TextMenuInterface
{
    public interface ITextUI
    {
        void EraseHistoryAndShow(string textToShow);
        string GetStringFromUser();
        void ShowAndWaitForReadySignal(string textToShow);
        void ShowListAndWaitForReadySignal(List<string> strings);
    }
}