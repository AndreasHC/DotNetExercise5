namespace DotNetExercise5
{
    internal class RepeatingMenuEntry<T> : MenuEntry<T>
    {
        private bool IsClosingEntry { get; init; }
        internal RepeatingMenuEntry(string text, T value, bool isClosingEntry = true) : base(text, value)
        {
            IsClosingEntry = isClosingEntry;
        }
        internal bool CloseAfter()
        {
            return IsClosingEntry;
        }

    }
}
