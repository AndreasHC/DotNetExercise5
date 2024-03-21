using System.Text;

namespace DotNetExercise5
{
    // Any further attempts to make this thing collection-initializer-compatible at this stage is likely to take up my attention for too long.

    // It might be worthwhile to separate menu entry handling from console handling somehow.
    internal class ConsoleRepeatingMenu : ConsoleMenu<Action>
    {
        internal ConsoleRepeatingMenu(string textAbove, string textBelow, string textForInvalidInput):base(textAbove, textBelow, textForInvalidInput)
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
