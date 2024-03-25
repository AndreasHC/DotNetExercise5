using System.Text;

namespace TextMenuInterface
{
    public class TextMenu<T>
    {
        // Any further attempts to make this thing collection-initializer-compatible at this stage is likely to take up my attention for too long.

        private Dictionary<string, MenuEntry<T>> Entries { get; } = new Dictionary<string, MenuEntry<T>>();
        private int NextKey { get; set; } = 0;
        private string TextAbove { get; init; }
        private string TextBelow { get; init; }
        private string TextForInvalidInput { get; init; }
        private ITextUI TextUI { get; init; }
        public TextMenu(string textAbove, string textBelow, string textForInvalidInput, ITextUI textUI)
        {
            TextAbove = textAbove;
            TextBelow = textBelow;
            TextForInvalidInput = textForInvalidInput;
            TextUI = textUI;
        }

        public void Add(MenuEntry<T> entry)
        {
            Entries.Add(NextKey.ToString(), entry);
            NextKey++;
        }

        public MenuEntry<T> GetPickFromUser()
        {
            do
            {
                TextUI.EraseHistoryAndShow(MenuText());
                string input = TextUI.GetStringFromUser();
                if (Entries.ContainsKey(input))
                {
                    return Entries[input];
                }
                else
                {
                    TextUI.ShowAndWaitForReadySignal(TextForInvalidInput);
                }
            } while (true);
        }

        public T Ask()
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