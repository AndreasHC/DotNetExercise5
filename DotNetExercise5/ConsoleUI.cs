using System.Collections.Immutable;

namespace DotNetExercise5
{
    internal class ConsoleUI : IUI
    {
        private ConsoleRepeatingMenu MainMenu { get; init; }
        // I feel like there should be a more object-oriented way.
        private static ImmutableDictionary<Type, string> TypeNames { get; } = new Dictionary<Type, string>()
        {
            {typeof(Vehicle), "Obskrivligt fordon"},
            {typeof(Car), "Bil" },
            {typeof(Boat), "Båt" },
            {typeof(Airplane), "Flygplan" },
            {typeof(Bus), "Buss" },
            {typeof(Motorcycle), "Motorcykel" }
        }.ToImmutableDictionary<Type, string>();
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
            ConsoleMenu<Func<IVehicle>> AddVehicleMenu = CreateAddVehicleMenu(registrationNumberQuestion);
            MainMenu.Add(new MenuEntry<Action>(
                "Lägg till ett fordon",
                () =>
                {
                    DescribeAddResultToUser(handler.Add(AddVehicleMenu.Ask()()));
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
                    // TODO It might be worth the effort to restructure the data structure to create the menu object once an only replace (or even reset) the search object. If we build a "SearchHandler" and let the delegates be aware only of that handler, the handler could then replace the search object itself with a new one when a new search is started.
                    // TODO A criterion factory would improve separation of concerns.
                    ConsoleRepeatingMenu SearchMenu = new ConsoleRepeatingMenu("Hantera sökning", "Välj ett alternativ", "Det var inte ett valbart alternativ. Tryck Enter för att komma tillbaka.");
                    VehicleSearch theSearch = new VehicleSearch(enumerable);
                    SearchMenu.Add(new RepeatingMenuEntry<Action>("Återvänd till huvudmenyn", () => { }));
                    ConsoleMenu<Func<VehicleCriterion>> addCriterionMenu = new ConsoleMenu<Func<VehicleCriterion>>("Hurdant kriterium?", "Välj en typ av kriterium.", "Det var inte en giltig kriteriumtyp.");
                    ConsoleStringQuestion registrationNumberToSearchQuestion = new ConsoleStringQuestion("Vilket registreringsnummer vill du söka efter?");
                    addCriterionMenu.Add(new MenuEntry<Func<VehicleCriterion>>(
                        "Registreringsnummerkriterium",
                        () =>
                        {
                            string registrationNumberToSearch = registrationNumberToSearchQuestion.Ask();
                            return new VehicleCriterion($"Registreringsnummer = {registrationNumberToSearch}", (IVehicle vehicle) => vehicle.RegistrationNumber == registrationNumberToSearch);
                        }));
                    ConsoleMenu<VehicleColor> colorSearchMenu = new ConsoleMenu<VehicleColor>("Vilken färg ska det sökas efter?", "Välj en färg.", "Det var inte en valbar färg. Tryck Enter för att försöka igen.");
                    foreach (VehicleColor color in Enum.GetValues(typeof(VehicleColor)))
                        colorSearchMenu.Add(new MenuEntry<VehicleColor>(color.Swedish(), color));
                    addCriterionMenu.Add(new MenuEntry<Func<VehicleCriterion>>(
                        "Färgkriterium",
                        () =>
                        {
                            VehicleColor color = colorSearchMenu.Ask();
                            return new VehicleCriterion($"Färg = {color.Swedish()}", (IVehicle vehicle) => vehicle.Color == color);
                        }));
                    ConsoleMenu<Type> vehicleTypeSearchMenu = new ConsoleMenu<Type>("Vilken typ ska det sökas efter?", "Välj en typ.", "Det var inte en valbar typ");
                    foreach (KeyValuePair<Type, string> entry in TypeNames)
                        vehicleTypeSearchMenu.Add(new MenuEntry<Type>(entry.Value, entry.Key));
                    addCriterionMenu.Add(new MenuEntry<Func<VehicleCriterion>>(
                        "Typkriterium",
                        () =>
                        {
                            Type selectedType = vehicleTypeSearchMenu.Ask();
                            // Exact match, not "is". Although there might be a use case for "is" as well.
                            return new VehicleCriterion($"Typ = {TypeNames[selectedType]}", (IVehicle vehicle) => vehicle.GetType() == selectedType);
                        }));
                    ConsoleUIntQuestion wheelCriterionQuestion = new ConsoleUIntQuestion("Vilket antal hjul ska det sökas efter?", "Det var inte ett giltigt antal hjul");
                    addCriterionMenu.Add(new MenuEntry<Func<VehicleCriterion>>(
                        "Hjulkriterium",
                        () =>
                        {
                            uint selectedNumber = wheelCriterionQuestion.Ask();
                            return new VehicleCriterion($"Antal hjul = {selectedNumber}", (IVehicle vehicle) => vehicle.NumberOfWheels == selectedNumber);
                        }));
                    SearchMenu.Add(new MenuEntry<Action>(
                        "Lägg till ett urvalskriterium",
                        () =>
                        {
                            theSearch.AddCriterion(addCriterionMenu.Ask()());
                        }));
                    SearchMenu.Add(new MenuEntry<Action>(
                        "Lista urvalskriterier",
                        () =>
                            {
                                Console.WriteLine(theSearch);
                                Console.WriteLine("Tryck Enter för att fortsätta.");
                                Console.ReadLine();
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

        private static ConsoleMenu<Func<IVehicle>> CreateAddVehicleMenu(ConsoleStringQuestion registrationNumberQuestion)
        {
            ConsoleMenu<VehicleColor> colorMenu = new ConsoleMenu<VehicleColor>("Vilken färg ska fordonet ha?", "Välj en färg.", "Det var inte en valbar färg. Tryck Enter för att försöka igen.");
            foreach (VehicleColor color in Enum.GetValues(typeof(VehicleColor)))
                colorMenu.Add(new MenuEntry<VehicleColor>(color.Swedish(), color));

            ConsoleUIntQuestion numberofWheelsQuestion = new ConsoleUIntQuestion("Hur många hjul ska fordonet ha?", "Det är inte ett giltigt antal hjul.");
            ConsoleMenu<Func<IVehicle>> AddVehicleMenu = new ConsoleMenu<Func<IVehicle>>("Lägga till hurdant fordon?", "Välj en fordonstyp.", "Det var inte en valbar fordonstyp.");
            AddVehicleMenu.Add(new MenuEntry<Func<IVehicle>>(
                TypeNames[typeof(Vehicle)],
                () =>
                {
                    return new Vehicle(registrationNumberQuestion.Ask(), colorMenu.Ask(), numberofWheelsQuestion.Ask());
                }));
            ConsoleDoubleQuestion wingSpanQuestion = new ConsoleDoubleQuestion("Vilket vingspann ska flygplanet ha?", "Det var inte ett giltigt vingspann.");
            AddVehicleMenu.Add(new MenuEntry<Func<IVehicle>>(
                TypeNames[typeof(Airplane)],
                () =>
                {
                    return new Airplane(registrationNumberQuestion.Ask(), colorMenu.Ask(), numberofWheelsQuestion.Ask(), wingSpanQuestion.Ask());
                }));
            ConsoleUIntQuestion numberOfDoorsQuestion = new ConsoleUIntQuestion("Hur många dörrar ska bilen ha?", "Det var inte ett giltigt antal dörrar.");
            AddVehicleMenu.Add(new MenuEntry<Func<IVehicle>>(
                TypeNames[typeof(Car)],
                () =>
                {
                    return new Car(registrationNumberQuestion.Ask(), colorMenu.Ask(), numberofWheelsQuestion.Ask(), numberOfDoorsQuestion.Ask());
                }));
            ConsoleUIntQuestion numberOfSeatsQuestion = new ConsoleUIntQuestion("Hur många säten ska bussen ha?", "Det var inte ett giltigt antal säten.");
            AddVehicleMenu.Add(new MenuEntry<Func<IVehicle>>(
               TypeNames[typeof(Bus)],
                () =>
                {
                    return new Bus(registrationNumberQuestion.Ask(), colorMenu.Ask(), numberofWheelsQuestion.Ask(), numberOfSeatsQuestion.Ask());
                }));
            ConsoleDoubleQuestion lengthQuestion = new ConsoleDoubleQuestion("Vad ska båten ha för längd?", "Det var inte en giltig längd.");
            AddVehicleMenu.Add(new MenuEntry<Func<IVehicle>>(
                TypeNames[typeof(Boat)],
                () =>
                {
                    return new Boat(registrationNumberQuestion.Ask(), colorMenu.Ask(), numberofWheelsQuestion.Ask(), lengthQuestion.Ask());
                }));
            ConsoleUIntQuestion gearQuestion = new ConsoleUIntQuestion("Hur många växlar ska motorcykeln ha?", "Det var inte ett giltigt antal växlar.");
            AddVehicleMenu.Add(new MenuEntry<Func<IVehicle>>(
                TypeNames[typeof(Motorcycle)],
                () =>
                {
                    return new Motorcycle(registrationNumberQuestion.Ask(), colorMenu.Ask(), numberofWheelsQuestion.Ask(), gearQuestion.Ask());
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
