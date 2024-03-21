namespace DotNetExercise5
{
    internal class ConsoleUI : IUI
    {
        private ConsoleMenu MainMenu { get; init; }
        internal ConsoleUI(IHandler handler)
        {
            MainMenu = new ConsoleMenu("Välkommen", "Välj ett alternativ", "Det var inte ett valbart alternativ. Tryck Enter för att komma tillbaka.");
            MainMenu.Add(new MenuEntry("Avsluta", () => { }, true));
            ConsoleUIntQuestion capacityQuestion = new ConsoleUIntQuestion("Vilken kapacitet ska garaget ha?", "Det är inte en giltig kapacitet.");
            MainMenu.Add(new MenuEntry("Öppna ett nytt garage", () => handler.MakeNewGarage(capacityQuestion.Ask())));
            MainMenu.Add(new MenuEntry(
                "Lista innehållet i garaget",
                () =>
                {
                    if (handler.DoToEach((IVehicle vehicle) => Console.WriteLine(vehicle)))
                        Console.WriteLine("Tryck Enter för att fortsätta.");
                    else
                        Console.WriteLine("Det finns inget garage. Tryck Enter för att fortsätta.");
                    Console.ReadLine();
                }));
            ConsoleStringQuestion registrationNumberQuestion = new ConsoleStringQuestion("Vilket registreringsnummer ska fordonet ha?");
            ConsoleUIntQuestion numberofWheelsQuestion = new ConsoleUIntQuestion("Hur många hjul ska fordonet ha?", "Det är inte ett giltigt antal hjul.");
            MainMenu.Add(new MenuEntry(
                "Lägg till ett fordon",
                () =>
                {
                    switch (handler.Add(new Vehicle(registrationNumberQuestion.Ask(), VehicleColor.Black, numberofWheelsQuestion.Ask())))
                    {
                        case AddResult.Success:
                            Console.WriteLine("Det gick bra");
                            break;
                        case AddResult.NoGarage:
                            Console.WriteLine("Det finns inget garage.");
                            break;
                        case AddResult.FullGarage:
                            Console.WriteLine("Garaget är fullt.");
                            break;
                    }
                    Console.WriteLine("Tryck Enter för att fortsätta.");
                    Console.ReadLine();
                }));
        }
        public void Run()
        {
            MainMenu.Run();
        }
    }
}
