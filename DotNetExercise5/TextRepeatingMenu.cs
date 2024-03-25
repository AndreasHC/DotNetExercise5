using System.Text;

namespace DotNetExercise5
{
    // Any further attempts to make this thing collection-initializer-compatible at this stage is likely to take up my attention for too long.

    internal class TextRepeatingMenu : TextMenu<Action>
    {
        internal TextRepeatingMenu(string textAbove, string textBelow, string textForInvalidInput, ITextUI textUI):base(textAbove, textBelow, textForInvalidInput, textUI)
        {
        }

        internal void Run()
        {
            do
            {
                MenuEntry<Action> entry = GetPickFromUser();
                entry.Value();
                if ((entry as RepeatingMenuEntry<Action>)?.CloseAfter() ?? false)
                    return;
            } while (true);
        }
    }
}
