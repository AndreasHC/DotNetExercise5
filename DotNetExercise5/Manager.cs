using TextMenuInterface;

namespace DotNetExercise5
{
    internal class Manager
    {
        private IUI TheUI { get; init; }
        private IHandler TheHandler { get; init; }

        internal Manager()
        {
            TheHandler = new GarageHandler();
            TheUI = new MenuUI(TheHandler, new ConsoleTextUI(), new SearchHandler());
        }

        internal void Run()
        {
            TheUI.Run();
        }
    }
}
