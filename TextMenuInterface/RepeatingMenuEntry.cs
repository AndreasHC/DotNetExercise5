namespace TextMenuInterface
{
    public class RepeatingMenuEntry<T> : MenuEntry<T>
    {
        private bool IsClosingEntry { get; init; }
        public RepeatingMenuEntry(string text, T value, bool isClosingEntry = true) : base(text, value)
        {
            IsClosingEntry = isClosingEntry;
        }
        internal bool CloseAfter()
        {
            return IsClosingEntry;
        }

    }
}
