namespace DotNetExercise5
{
    internal class Manager
    {
        private IUI TheUI { get; init; }
        private IHandler TheHandler { get; init; }

        internal Manager()
        {
            TheHandler = new GarageHandler();
            TheUI = new ConsoleUI(TheHandler);
        }

        internal void Run()
        {
            TheUI.Run();
        }
    }
}
