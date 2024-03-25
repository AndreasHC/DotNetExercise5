
namespace DotNetExercise5
{
    internal class ConsoleTextUI : ITextUI
    {
        public void EraseHistoryAndShow(string textToShow)
        {
            Console.Clear();
            Console.WriteLine(textToShow);
        }

        public string GetStringFromUser()
        {
            return Console.ReadLine() ?? throw new InvalidOperationException("The input stream seems to have closed.");
        }

        public void ShowAndWaitForReadySignal(string textForInvalidInput)
        {
            Console.WriteLine(textForInvalidInput);
            RequestAndWaitForReadySignal();
        }

        public void ShowListAndWaitForReadySignal(List<string> strings)
        {
            foreach (string s in strings)
            {
                Console.WriteLine(s);
                Console.WriteLine();
            }
            RequestAndWaitForReadySignal();
        }

        private void RequestAndWaitForReadySignal()
        {
            Console.WriteLine("Tryck Enter för att fortsätta.");
            Console.ReadLine();
        }
    }
}
