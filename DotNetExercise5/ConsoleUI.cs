namespace DotNetExercise5
{
    internal class ConsoleUI : IUI
    {
        private ConsoleMenu MainMenu { get; init; }
        internal ConsoleUI()
        {
            MainMenu = new ConsoleMenu("Välkommen", "Välj ett alternativ", "Det var inte ett valbart alternativ. Truck Enter för att komma tillbaka.");
            MainMenu.Add(new MenuEntry("Avsluta", () => { }, true));
        }
        // I feel that I am missing something here.
        void IUI.Run()
        {
            this.Run();
        }
        internal void Run()
        {
            MainMenu.Run();
        }
    }
}
