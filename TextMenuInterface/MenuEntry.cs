namespace TextMenuInterface
{
    public class MenuEntry<T>
    {

        internal string Text { get; init; }
        internal T Value { get; init; }
        public MenuEntry(string text, T value)
        {
            Text = text;
            Value = value;
        }
    }
}