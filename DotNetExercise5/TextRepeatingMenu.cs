namespace DotNetExercise5
{
    internal class TextRepeatingMenu : TextMenu<Action>
    {
        internal TextRepeatingMenu(string textAbove, string textBelow, string textForInvalidInput, ITextUI textUI) : base(textAbove, textBelow, textForInvalidInput, textUI)
        {
        }

        internal void Run()
        {
            do
            {
                MenuEntry<Action> entry = GetPickFromUser();
                entry.Value();
                // Regular menu entry: keep going
                // RepeatingMenuEntry with closing behavior turned off: keep going
                // RepeatingMenuEntry with closing behavior turned on: return control of thread to caller
                if ((entry as RepeatingMenuEntry<Action>)?.CloseAfter() ?? false)
                    return;
            } while (true);
        }
    }
}
