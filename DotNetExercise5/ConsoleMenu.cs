using System.Text;

namespace DotNetExercise5
{
    // Any further attempts to make this thing collection-initializer-compatible at this stage is likely to take up my attention for too long.

    // It might be worthwhile to separate menu entry handling from console handling somehow.
    internal class ConsoleMenu
    {
        private Dictionary<string, MenuEntry> Entries { get; } = new Dictionary<string, MenuEntry>();
        private int NextKey { get; set; } = 0;
        private string TextAbove { get; init; }
        private string TextBelow { get; init; }
        // This text is responsible for informing the user that they need to press "Enter" to return to the menu. This behavior is not configurable at this time.
        private string TextForInvalidInput { get; init; }
        public ConsoleMenu(string textAbove, string textBelow, string textForInvalidInput)
        {
            TextAbove = textAbove;
            TextBelow = textBelow;
            TextForInvalidInput = textForInvalidInput;
        }
        public void Add(MenuEntry entry)
        {
            Entries.Add(NextKey.ToString(), entry);
            NextKey++;
        }
        public void Run()
        {
            bool done = false;
            do
            {
                Console.Clear();
                Console.WriteLine(MenuText());
                string input = Console.ReadLine() ?? throw new Exception("The input stream seems to have closed.");
                if (Entries.ContainsKey(input))
                {
                    Entries[input].Execute();
                    done |= Entries[input].CloseAfter();
                }
                else
                {
                    Console.WriteLine(TextForInvalidInput);
                    Console.ReadLine();
                }
            } while (!done);
        }
        private string MenuText()
        {
            StringBuilder buffer = new StringBuilder(TextAbove);
            buffer.Append(Environment.NewLine);
            foreach (KeyValuePair<string, MenuEntry> pair in Entries)
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
