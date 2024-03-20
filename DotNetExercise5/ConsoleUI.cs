namespace DotNetExercise5
{
    internal class ConsoleUI : IUI
    {
        private ConsoleMenu MainMenu { get; init; }
        internal ConsoleUI(IHandler handler)
        {
            MainMenu = new ConsoleMenu("Välkommen", "Välj ett alternativ", "Det var inte ett valbart alternativ. Truck Enter för att komma tillbaka.");
            MainMenu.Add(new MenuEntry("Avsluta", () => { }, true));
            ConsoleUIntQuestion capacityQuestion = new ConsoleUIntQuestion("Vilken kapacitet ska garaget ha?", "Det är inte en giltig kapacitet.");
            MainMenu.Add(new MenuEntry("Öppna ett nytt garage", () => handler.MakeNewGarage(capacityQuestion.Ask())));
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
