namespace DotNetExercise5
{
    internal class ConsoleUI : IUI
    {
        private ConsoleRepeatingMenu MainMenu { get; init; }
        internal ConsoleUI(IHandler handler)
        {
            MainMenu = new ConsoleRepeatingMenu("Välkommen", "Välj ett alternativ", "Det var inte ett valbart alternativ. Tryck Enter för att komma tillbaka.");
            MainMenu.Add(new RepeatingMenuEntry<Action>("Avsluta", () => { }));
            ConsoleUIntQuestion capacityQuestion = new ConsoleUIntQuestion("Vilken kapacitet ska garaget ha?", "Det är inte en giltig kapacitet.");
            ConsoleMenu<string> populationQuestion = new ConsoleMenu<string>("Vilken startpopulation ska garaget ha", "Välj en startpopulation.", "Det var inte en valbar startpopulation.");
            foreach (string populationString in handler.AvailableStartingPopulations())
                populationQuestion.Add(new MenuEntry<string>(populationString, populationString));
            MainMenu.Add(new MenuEntry<Action>(
                "Öppna ett nytt garage",
                () =>
                {
                    switch (handler.MakeNewGarage(capacityQuestion.Ask(), populationQuestion.Ask()))
                    {
                        case PopulateResult.Success:
                            Console.WriteLine("Det gick bra.");
                            break;
                        case PopulateResult.InsufficientCapacity:
                            Console.WriteLine("Kapaciteten och startpopulationen är oförenliga. Garaget har skapats tomt.");
                            break;
                        case PopulateResult.UnknownPopulation:
                            Console.WriteLine("Du verkar ha hittat ett menyvalsalternativ för startpopulation som det här systemet inte kan hantera. Det ska inte hända. Garaget har skapats tomt.");
                            break;

                    };
                    Console.WriteLine("Tryck enter för att fortsätta.");
                    Console.ReadLine();

                }));
            MainMenu.Add(new MenuEntry<Action>(
                "Lista innehållet i garaget",
                () =>
                {
                    if (handler.DoToEach((IVehicle vehicle) => { Console.WriteLine(vehicle); Console.WriteLine(); }))
                        Console.WriteLine("Tryck Enter för att fortsätta.");
                    else
                        Console.WriteLine("Det finns inget garage. Tryck Enter för att fortsätta.");
                    Console.ReadLine();
                }));
            ConsoleStringQuestion registrationNumberQuestion = new ConsoleStringQuestion("Vilket registreringsnummer ska fordonet ha?");
            ConsoleMenu<Action> AddVehicleMenu = CreateAddVehicleMenu(handler, registrationNumberQuestion);
            MainMenu.Add(new MenuEntry<Action>(
                "Lägg till ett fordon",
                () =>
                {
                    AddVehicleMenu.Ask()();//Gods, this looks stupid.
                }));
            MainMenu.Add(new MenuEntry<Action>(
                "Ta bort ett fordon",
                () =>
                {
                    switch (handler.Remove(registrationNumberQuestion.Ask()))
                    {
                        case RemoveResult.Success:
                            Console.WriteLine("Det gick bra.");
                            break;
                        case RemoveResult.NoGarage:
                            Console.WriteLine("Det finns inget garage.");
                            break;
                        case RemoveResult.NotFound:
                            Console.WriteLine("fordonet finns inte");
                            break;
                    }
                    Console.WriteLine("Tryck Enter för att fortsätta.");
                    Console.ReadLine();
                }));
            MainMenu.Add(new MenuEntry<Action>(
                "Starta en sökning",
                () =>
                {
                    IEnumerable<IVehicle>? enumerable = handler.GetEnumerable();
                    if (enumerable == null)
                    {
                        Console.WriteLine("Det finns inget garage. Tryck Enter för att fortsätta.");
                        Console.ReadLine();
                        return;
                    }
                    // TODO It might be worth the effort to restructure the data structure to create the menu object once an only replace (or even reset) the search object.
                    ConsoleRepeatingMenu SearchMenu = new ConsoleRepeatingMenu("Hantera sökning", "Välj ett alternativ", "Det var inte ett valbart alternativ. Tryck Enter för att komma tillbaka.");
                    VehicleSearch theSearch = new VehicleSearch(enumerable);
                    SearchMenu.Add(new RepeatingMenuEntry<Action>("Återvänd till huvudmenyn", () => { }));
                    ConsoleMenu<Action> addCriterionMenu = new ConsoleMenu<Action>("Hurdant kriterium?", "Välj en typ av kriterium.", "Det var inte en giltig kriteriumtyp.");
                    ConsoleStringQuestion registrationNumberToSearchQuestion = new ConsoleStringQuestion("Vilket registreringsnummer vill du söka efter?");
                    addCriterionMenu.Add(new MenuEntry<Action>(
                        "Registreringsnummerkriterium",
                        () =>
                        {
                            string registrationNumberToSearch = registrationNumberToSearchQuestion.Ask();
                            theSearch.AddCriterion(new VehicleCriterion($"Registreringsnummer = {registrationNumberToSearch}", (IVehicle vehicle) => vehicle.RegistrationNumber == registrationNumberToSearch));
                        }));
                    SearchMenu.Add(new MenuEntry<Action>(
                        "Lägg till ett urvalskriterium.",
                        () =>
                        {
                            addCriterionMenu.Ask()();
                        }));
                    SearchMenu.Add(new MenuEntry<Action>(
                        "Gör sökningen",
                        () =>
                        {
                            foreach (Vehicle vehicle in theSearch.Run())
                            {
                                Console.WriteLine(vehicle);
                                Console.WriteLine();

                            }
                            Console.WriteLine("Tryck Enter för att fortsätta.");
                            Console.ReadLine();
                        }));
                    SearchMenu.Run();
                }));
        }

        private static ConsoleMenu<Action> CreateAddVehicleMenu(IHandler handler, ConsoleStringQuestion registrationNumberQuestion)
        {
            ConsoleMenu<VehicleColor> colorMenu = new ConsoleMenu<VehicleColor>("Vilken färg ska fordonet ha?", "Välj en färg.", "Det var inte en valbar färg. Tryck Enter för att försöka igen.");
            foreach (VehicleColor color in Enum.GetValues(typeof(VehicleColor)))
                colorMenu.Add(new MenuEntry<VehicleColor>(color.Swedish(), color));

            ConsoleUIntQuestion numberofWheelsQuestion = new ConsoleUIntQuestion("Hur många hjul ska fordonet ha?", "Det är inte ett giltigt antal hjul.");
            // Exactly the right amount of repetition to repeatedly get the impulse to factor something out while being unable to actually do so...
            ConsoleMenu<Action> AddVehicleMenu = new ConsoleMenu<Action>("Lägga till hurdant fordon?", "Välj en fordonstyp.", "Det var inte en valbar fordonstyp.");
            AddVehicleMenu.Add(new MenuEntry<Action>(
                "Obeskrivligt fordon",
                () =>
                {
                    DescribeAddResultToUser(handler.Add(new Vehicle(registrationNumberQuestion.Ask(), colorMenu.Ask(), numberofWheelsQuestion.Ask())));
                }));
            ConsoleDoubleQuestion wingSpanQuestion = new ConsoleDoubleQuestion("Vilket vingspann ska flygplanet ha?", "Det var inte ett giltigt vingspann.");
            AddVehicleMenu.Add(new MenuEntry<Action>(
                "Flygplan",
                () =>
                {
                    DescribeAddResultToUser(handler.Add(new Airplane(registrationNumberQuestion.Ask(), colorMenu.Ask(), numberofWheelsQuestion.Ask(), wingSpanQuestion.Ask())));
                }));
            ConsoleUIntQuestion numberOfDoorsQuestion = new ConsoleUIntQuestion("Hur många dörrar ska bilen ha?", "Det var inte ett giltigt antal dörrar.");
            AddVehicleMenu.Add(new MenuEntry<Action>(
                "Bil",
                () =>
                {
                    DescribeAddResultToUser(handler.Add(new Car(registrationNumberQuestion.Ask(), colorMenu.Ask(), numberofWheelsQuestion.Ask(), numberOfDoorsQuestion.Ask())));
                }));
            ConsoleUIntQuestion numberOfSeatsQuestion = new ConsoleUIntQuestion("Hur många säten ska bussen ha?", "Det var inte ett giltigt antal säten.");
            AddVehicleMenu.Add(new MenuEntry<Action>(
                "Buss",
                () =>
                {
                    DescribeAddResultToUser(handler.Add(new Bus(registrationNumberQuestion.Ask(), colorMenu.Ask(), numberofWheelsQuestion.Ask(), numberOfSeatsQuestion.Ask())));
                }));
            ConsoleDoubleQuestion lengthQuestion = new ConsoleDoubleQuestion("Vad ska båten ha för längd?", "Det var inte en giltig längd.");
            AddVehicleMenu.Add(new MenuEntry<Action>(
                "Båt",
                () =>
                {
                    DescribeAddResultToUser(handler.Add(new Boat(registrationNumberQuestion.Ask(), colorMenu.Ask(), numberofWheelsQuestion.Ask(), lengthQuestion.Ask())));
                }));
            ConsoleUIntQuestion gearQuestion = new ConsoleUIntQuestion("Hur många växlar ska motorcykeln ha?", "Det var inte ett giltigt antal växlar.");
            AddVehicleMenu.Add(new MenuEntry<Action>(
                "Motorcykel",
                () =>
                {
                    DescribeAddResultToUser(handler.Add(new Motorcycle(registrationNumberQuestion.Ask(), colorMenu.Ask(), numberofWheelsQuestion.Ask(), gearQuestion.Ask())));
                }));
            return AddVehicleMenu;
        }

        private static void DescribeAddResultToUser(AddResult addResult)
        {
            switch (addResult)
            {
                case AddResult.Success:
                    Console.WriteLine("Det gick bra.");
                    break;
                case AddResult.NoGarage:
                    Console.WriteLine("Det finns inget garage.");
                    break;
                case AddResult.FullGarage:
                    Console.WriteLine("Garaget är fullt.");
                    break;
                case AddResult.DuplicateRegistrationNumber:
                    Console.WriteLine("Registreringsnumret finns redan i garaget.");
                    break;
            }
            Console.WriteLine("Tryck Enter för att fortsätta.");
            Console.ReadLine();
        }

        public void Run()
        {
            MainMenu.Run();
        }
    }
}
