namespace DotNetExercise5
{
    internal class Manager
    {
        private IUI UI { get; init; }

        internal Manager()
        {
            UI = new ConsoleUI();
        }

        internal void Run()
        {
            UI.Run();
        }
    }
}
