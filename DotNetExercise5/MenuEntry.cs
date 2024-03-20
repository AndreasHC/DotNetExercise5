namespace DotNetExercise5
{
    internal class MenuEntry
    {
        internal string Text { get; init; }
        private Action TheAction { get; init; }
        private bool IsClosingEntry { get; init; }
        internal MenuEntry(string text, Action theAction, bool isClosingEntry = false)
        {
            Text = text;
            TheAction = theAction;
            IsClosingEntry = isClosingEntry;
        }
        internal void Execute()
        {
            TheAction();
        }
        internal bool CloseAfter()
        {
            return IsClosingEntry;
        }

    }
}
