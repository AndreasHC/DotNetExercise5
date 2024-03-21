using System.Text;

namespace DotNetExercise5
{
    internal class ConsoleMenu<T>
    {
        private Dictionary<string, MenuEntry<T>> Entries { get; } = new Dictionary<string, MenuEntry<T>>();
        private int NextKey { get; set; } = 0;
        private string TextAbove { get; init; }
        private string TextBelow { get; init; }
        // This text is responsible for informing the user that they need to press "Enter" to return to the menu. This behavior is not configurable at this time.
        private string TextForInvalidInput { get; init; }
        internal ConsoleMenu(string textAbove, string textBelow, string textForInvalidInput)
        {
            TextAbove = textAbove;
            TextBelow = textBelow;
            TextForInvalidInput = textForInvalidInput;
        }

        internal void Add(MenuEntry<T> entry)
        {
            Entries.Add(NextKey.ToString(), entry);
            NextKey++;
        }

        protected MenuEntry<T> GetPickFromUser()
        {
            do
            {
                Console.Clear();
                Console.WriteLine(MenuText());
                string input = Console.ReadLine() ?? throw new Exception("The input stream seems to have closed.");
                if (Entries.ContainsKey(input))
                {
                    return Entries[input];
                }
                else
                {
                    Console.WriteLine(TextForInvalidInput);
                    Console.ReadLine();
                }
            } while (true);
        }

        internal T Ask()
        {
            return GetPickFromUser().Value;
        }

        private string MenuText()
        {
            StringBuilder buffer = new StringBuilder(TextAbove);
            buffer.Append(Environment.NewLine);
            foreach (KeyValuePair<string, MenuEntry<T>> pair in Entries)
            {
                buffer.Append(pair.Key);
                buffer.Append(": ");
                buffer.Append(pair.Value.Text);
                buffer.Append(Environment.NewLine);
            }
            buffer.Append(TextBelow);
            return buffer.ToString();
        }
    }
}