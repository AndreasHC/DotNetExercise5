﻿using System.Collections.Immutable;
using TextMenuInterface;

namespace DotNetExercise5
{
    internal class MenuUI : IUI
    {
        private TextRepeatingMenu MainMenu { get; init; }
        // I feel like there should be a more object-oriented way. Or something.
        private static ImmutableDictionary<Type, string> TypeNames { get; } = new Dictionary<Type, string>()
        {
            {typeof(Vehicle), "Obeskrivligt fordon"},
            {typeof(Car), "Bil" },
            {typeof(Boat), "Båt" },
            {typeof(Airplane), "Flygplan" },
            {typeof(Bus), "Buss" },
            {typeof(Motorcycle), "Motorcykel" }
        }.ToImmutableDictionary<Type, string>();
        private static ImmutableDictionary<Type, string> DefiniteTypeNames { get; } = new Dictionary<Type, string>()
        {
            {typeof(Vehicle), "det obeskrivliga fordonet"},
            {typeof(Car), "bilen" },
            {typeof(Boat), "båten" },
            {typeof(Airplane), "flygplanet" },
            {typeof(Bus), "bussen" },
            {typeof(Motorcycle), "motorcykeln" }
        }.ToImmutableDictionary<Type, string>();
        internal MenuUI(IHandler handler, ITextUI lowerUI, ISearchHandler searchHandler, IVehicleFactory vehicleFactory)
        {
            MainMenu = new TextRepeatingMenu("Välkommen", "Välj ett alternativ", "Det var inte ett valbart alternativ.", lowerUI);
            MainMenu.Add(new RepeatingMenuEntry<Action>("Avsluta", () => { }));
            MainMenu.Add(CreateGarageCreationOption(handler, lowerUI));
            MainMenu.Add(CreateListGarageContentOption(handler, lowerUI));
            MainMenu.Add(CreateShowTypeDistributionOption(handler, lowerUI));
            MainMenu.Add(CreateAddVehicleOption(handler, vehicleFactory, lowerUI));
            MainMenu.Add(CreateRemoveVehicleOption(handler, lowerUI));

            MainMenu.Add(CreateRetrieveVehicleOption(handler, lowerUI));
            MainMenu.Add(CreateSearchOption(handler, searchHandler, lowerUI));
        }

        private MenuEntry<Action> CreateShowTypeDistributionOption(IHandler handler, ITextUI lowerUI)
        {
            return new MenuEntry<Action>(
                "Visa fördelning av fordonstyper",
                () =>
                {
                    IEnumerable<IVehicle>? content = handler.GetEnumerable();
                    if (content == null)
                    {
                        lowerUI.ShowAndWaitForReadySignal("Det finns inget garage.");
                        return;
                    }
                    Dictionary<Type, uint> statistics = new Dictionary<Type, uint>();
                    foreach (IVehicle vehicle in content)
                    {
                        Type type = vehicle.GetType();
                        statistics[type] = (statistics.ContainsKey(type) ? statistics[type] : 0) + 1;
                    }
                    List<string> presentation = new List<string>();
                    foreach (KeyValuePair<Type, uint> statistic in statistics)
                    {
                        Type Key = statistic.Key;
                        string typeString = TypeNames.ContainsKey(Key) ? TypeNames[Key] : $"Oidentifierad fordonstyp: {Key}"; 
                        presentation.Add($"{statistic.Value} fordon av typ {typeString}");
                    }
                    lowerUI.ShowListAndWaitForReadySignal(presentation);
                });
        }

        private static MenuEntry<Action> CreateSearchOption(IHandler handler, ISearchHandler searchHandler, ITextUI lowerUI)
        {
            TextRepeatingMenu SearchMenu = new TextRepeatingMenu("Hantera sökning", "Välj ett alternativ", "Det var inte ett valbart alternativ.", lowerUI);
            SearchMenu.Add(new RepeatingMenuEntry<Action>("Återvänd till huvudmenyn", () => { }));
            SearchMenu.Add(CreateAddCriterionOption(searchHandler, lowerUI));
            SearchMenu.Add(CreateListCriteriaOption(searchHandler, lowerUI));
            SearchMenu.Add(CreateDoSearchOption(searchHandler, lowerUI));
            return new MenuEntry<Action>(
                "Starta en sökning",
                 () =>
                 {
                     IEnumerable<IVehicle>? enumerable = handler.GetEnumerable();
                     if (enumerable == null)
                     {
                         lowerUI.ShowAndWaitForReadySignal("Det finns inget garage.");
                         return;
                     }
                     searchHandler.Init(enumerable);
                     SearchMenu.Run();
                 });
        }

        private static MenuEntry<Action> CreateDoSearchOption(ISearchHandler searchHandler, ITextUI lowerUI)
        {
            return new MenuEntry<Action>(
                "Gör sökningen",
                () =>
                {
                    List<string> stringRepresentations = new List<string>();
                    foreach (IVehicle vehicle in searchHandler.Run())
                        stringRepresentations.Add(vehicle.ToString() ?? "Fordon utan textrepresentation");
                    lowerUI.ShowListAndWaitForReadySignal(stringRepresentations);
                });
        }

        private static MenuEntry<Action> CreateListCriteriaOption(ISearchHandler searchHandler, ITextUI lowerUI)
        {
            return new MenuEntry<Action>(
                "Lista urvalskriterier",
                () =>
                {
                    lowerUI.ShowAndWaitForReadySignal(searchHandler.GetCriteriaString());
                });
        }

        private static MenuEntry<Action> CreateAddCriterionOption(ISearchHandler searchHandler, ITextUI lowerUI)
        {
            TextMenu<Func<VehicleCriterion>> addCriterionMenu = new TextMenu<Func<VehicleCriterion>>("Hurdant kriterium?", "Välj en typ av kriterium.", "Det var inte en giltig kriteriumtyp.", lowerUI);
            addCriterionMenu.Add(CreateRegistrationNumberCriterionOption(lowerUI));
            addCriterionMenu.Add(CreateColorCriterionOption(lowerUI));
            addCriterionMenu.Add(CreateTypeCriterionOption(lowerUI));
            addCriterionMenu.Add(CreateWheelCriterionOption(lowerUI));

            return new MenuEntry<Action>(
                "Lägg till ett urvalskriterium",
                () =>
                {
                    searchHandler.AddCriterion(addCriterionMenu.Ask()());
                });
        }

        private static MenuEntry<Func<VehicleCriterion>> CreateWheelCriterionOption(ITextUI lowerUI)
        {
            TextUIntQuestion wheelCriterionQuestion = new TextUIntQuestion("Vilket antal hjul ska det sökas efter?", "Det var inte ett giltigt antal hjul", lowerUI);

            return new MenuEntry<Func<VehicleCriterion>>(
                "Hjulkriterium",
                () =>
                {
                    uint selectedNumber = wheelCriterionQuestion.Ask();
                    return new VehicleCriterion($"Antal hjul = {selectedNumber}", (IVehicle vehicle) => vehicle.NumberOfWheels == selectedNumber);
                });
        }

        private static MenuEntry<Func<VehicleCriterion>> CreateTypeCriterionOption(ITextUI lowerUI)
        {
            TextMenu<Type> vehicleTypeSearchMenu = new TextMenu<Type>("Vilken typ ska det sökas efter?", "Välj en typ.", "Det var inte en valbar typ", lowerUI);
            foreach (KeyValuePair<Type, string> entry in TypeNames)
                vehicleTypeSearchMenu.Add(new MenuEntry<Type>(entry.Value, entry.Key));

            return new MenuEntry<Func<VehicleCriterion>>(
                "Typkriterium",
                () =>
                {
                    Type selectedType = vehicleTypeSearchMenu.Ask();
                    // Exact match, not "is". Although there might be a use case for "is" as well.
                    return new VehicleCriterion($"Typ = {TypeNames[selectedType]}", (IVehicle vehicle) => vehicle.GetType() == selectedType);
                });
        }

        private static MenuEntry<Func<VehicleCriterion>> CreateColorCriterionOption(ITextUI lowerUI)
        {
            TextMenu<VehicleColor> colorSearchMenu = new TextMenu<VehicleColor>("Vilken färg ska det sökas efter?", "Välj en färg.", "Det var inte en valbar färg.", lowerUI);
            foreach (VehicleColor color in Enum.GetValues(typeof(VehicleColor)))
                colorSearchMenu.Add(new MenuEntry<VehicleColor>(color.Swedish(), color));

            return new MenuEntry<Func<VehicleCriterion>>(
                "Färgkriterium",
                () =>
                {
                    VehicleColor color = colorSearchMenu.Ask();
                    return new VehicleCriterion($"Färg = {color.Swedish()}", (IVehicle vehicle) => vehicle.Color == color);
                });
        }

        private static MenuEntry<Func<VehicleCriterion>> CreateRegistrationNumberCriterionOption(ITextUI lowerUI)
        {
            TextStringQuestion registrationNumberToSearchQuestion = new TextStringQuestion("Vilket registreringsnummer vill du söka efter?", lowerUI);
            return new MenuEntry<Func<VehicleCriterion>>(
                "Registreringsnummerkriterium",
                () =>
                {
                    string registrationNumberToSearch = registrationNumberToSearchQuestion.Ask();
                    return new VehicleCriterion($"Registreringsnummer = {registrationNumberToSearch}", (IVehicle vehicle) => vehicle.RegistrationNumber == registrationNumberToSearch);
                });
        }

        private static MenuEntry<Action> CreateRetrieveVehicleOption(IHandler handler, ITextUI lowerUI)
        {
            TextStringQuestion registrationNumberQuestion = new TextStringQuestion("Vilket registreringsnummer vill du hämta?", lowerUI);

            return new MenuEntry<Action>(
                "Hämta ett fordon med ett visst registreringsnummer",
                () =>
                {
                    string chosenRegistrationNumber = registrationNumberQuestion.Ask();
                    if (handler.Retrieve(chosenRegistrationNumber, out RetrieveObstacle obstacle, out IVehicle? vehicle))
                        lowerUI.ShowAndWaitForReadySignal(vehicle.ToString() ?? "Fordonet har påträffats, men har ingen textrepresentation.");
                    else if (obstacle == RetrieveObstacle.NoGarage)
                        lowerUI.ShowAndWaitForReadySignal("Det finns inget garage.");
                    else if (obstacle == RetrieveObstacle.NotFound)
                        lowerUI.ShowAndWaitForReadySignal("Fordonet finns inte.");
                });
        }

        private static MenuEntry<Action> CreateRemoveVehicleOption(IHandler handler, ITextUI lowerUI)
        {
            TextStringQuestion registrationNumberQuestion = new TextStringQuestion("Vilket registreringsnummer ska fordonet ha?", lowerUI);

            return new MenuEntry<Action>(
                "Ta bort ett fordon",
                () =>
                {
                    switch (handler.Remove(registrationNumberQuestion.Ask()))
                    {
                        case RemoveResult.Success:
                            lowerUI.ShowAndWaitForReadySignal("Det gick bra.");
                            break;
                        case RemoveResult.NoGarage:
                            lowerUI.ShowAndWaitForReadySignal("Det finns inget garage.");
                            break;
                        case RemoveResult.NotFound:
                            lowerUI.ShowAndWaitForReadySignal("Fordonet finns inte.");
                            break;
                    }
                });
        }

        private static MenuEntry<Action> CreateAddVehicleOption(IHandler handler, IVehicleFactory vehicleFactory, ITextUI lowerUI)
        {
            TextMenu<Func<IVehicle>> AddVehicleMenu = CreateAddVehicleMenu(vehicleFactory, lowerUI);
            return new MenuEntry<Action>(
                "Lägg till ett fordon",
                () =>
                {
                    DescribeAddResultToUser(handler.Add(AddVehicleMenu.Ask()()), lowerUI);
                });
        }

        private static MenuEntry<Action> CreateListGarageContentOption(IHandler handler, ITextUI lowerUI)
        {
            return new MenuEntry<Action>(
                "Lista innehållet i garaget",
                () =>
                {
                    IEnumerable<IVehicle>? enumerable = handler.GetEnumerable();
                    if (enumerable == null)
                    {
                        lowerUI.ShowAndWaitForReadySignal("Det finns inget garage.");
                        return;
                    }
                    List<string> stringRepresentations = new List<string>();
                    foreach (IVehicle vehicle in enumerable)
                        stringRepresentations.Add(vehicle.ToString() ?? "Fordon utan textrepresentation"); // The things we do for semi-nonsensical compiler warnings...
                    lowerUI.ShowListAndWaitForReadySignal(stringRepresentations);
                });
        }

        private static MenuEntry<Action> CreateGarageCreationOption(IHandler handler, ITextUI lowerUI)
        {
            TextUIntQuestion capacityQuestion = new TextUIntQuestion("Vilken kapacitet ska garaget ha?", "Det är inte en giltig kapacitet.", lowerUI);
            TextMenu<string> populationQuestion = new TextMenu<string>("Vilken startpopulation ska garaget ha", "Välj en startpopulation.", "Det var inte en valbar startpopulation.", lowerUI);
            foreach (string populationString in handler.AvailableStartingPopulations())
                populationQuestion.Add(new MenuEntry<string>(populationString, populationString));

            return new MenuEntry<Action>(
                "Öppna ett nytt garage",
                () =>
                {
                    switch (handler.MakeNewGarage(capacityQuestion.Ask(), populationQuestion.Ask()))
                    {
                        case PopulateResult.Success:
                            lowerUI.ShowAndWaitForReadySignal("Det gick bra.");
                            break;
                        case PopulateResult.InsufficientCapacity:
                            lowerUI.ShowAndWaitForReadySignal("Kapaciteten och startpopulationen är oförenliga. Garaget har skapats tomt.");
                            break;
                        case PopulateResult.UnknownPopulation:
                            lowerUI.ShowAndWaitForReadySignal("Du verkar ha hittat ett menyvalsalternativ för startpopulation som det här systemet inte kan hantera. Det ska inte hända. Garaget har skapats tomt.");
                            break;

                    };

                });
        }

        private static TextMenu<Func<IVehicle>> CreateAddVehicleMenu(IVehicleFactory vehicleFactory, ITextUI lowerUI)
        {
            List<Type> availableTypes = vehicleFactory.AvaliableTypes();

            TextMenu<Func<IVehicle>> AddVehicleMenu = new TextMenu<Func<IVehicle>>("Lägga till hurdant fordon?", "Välj en fordonstyp.", "Det var inte en valbar fordonstyp.", lowerUI);
            foreach (Type vehicleType in availableTypes)
            {
                HashSet<VehicleCreationParameter> requiredParameters = vehicleFactory.RequiredParameters(vehicleType);

                TextStringQuestion registrationNumberQuestion = new TextStringQuestion($"Vilket registreringsnummer ska {DefiniteTypeNames[vehicleType]} ha?", lowerUI);
                TextMenu<VehicleColor> colorMenu = new TextMenu<VehicleColor>($"Vilken färg ska {DefiniteTypeNames[vehicleType]} ha?", "Välj en färg.", "Det var inte en valbar färg.", lowerUI);
                foreach (VehicleColor color in Enum.GetValues(typeof(VehicleColor)))
                    colorMenu.Add(new MenuEntry<VehicleColor>(color.Swedish(), color));
                TextUIntQuestion numberOfWheelsQuestion = new TextUIntQuestion($"Hur många hjul ska {DefiniteTypeNames[vehicleType]} ha?", "Det är inte ett giltigt antal hjul.", lowerUI);
                TextDoubleQuestion wingSpanQuestion = new TextDoubleQuestion($"Vilket vingspann ska {DefiniteTypeNames[vehicleType]} ha?", "Det var inte ett giltigt vingspann.", lowerUI);
                TextUIntQuestion numberOfDoorsQuestion = new TextUIntQuestion($"Hur många dörrar ska {DefiniteTypeNames[vehicleType]} ha?", "Det var inte ett giltigt antal dörrar.", lowerUI);
                TextUIntQuestion numberOfSeatsQuestion = new TextUIntQuestion($"Hur många säten ska {DefiniteTypeNames[vehicleType]} ha?", "Det var inte ett giltigt antal säten.", lowerUI);
                TextDoubleQuestion lengthQuestion = new TextDoubleQuestion($"Vad ska {DefiniteTypeNames[vehicleType]} ha för längd?", "Det var inte en giltig längd.", lowerUI);
                TextUIntQuestion numberOfGearsQuestion = new TextUIntQuestion("Hur många växlar ska motorcykeln ha?", "Det var inte ett giltigt antal växlar.", lowerUI);

                Dictionary<VehicleCreationParameter, Func<object>> obtainFuns = new Dictionary<VehicleCreationParameter, Func<object>>()
                {
                    { VehicleCreationParameter.RegistrationNumber, registrationNumberQuestion.Ask },
                    { VehicleCreationParameter.Color, () => {return colorMenu.Ask(); } },
                    { VehicleCreationParameter.NumberOfWheels, () => {return numberOfWheelsQuestion.Ask(); } },
                    { VehicleCreationParameter.WingSpan, () => {return wingSpanQuestion.Ask();} },
                    { VehicleCreationParameter.NumberOfDoors, () => {return numberOfDoorsQuestion.Ask();} },
                    { VehicleCreationParameter.NumberOfSeats, () => {return numberOfSeatsQuestion.Ask();} },
                    { VehicleCreationParameter.Length, () => {return lengthQuestion.Ask();} },
                    { VehicleCreationParameter.NumberOfGears, () => {return numberOfGearsQuestion.Ask();} }
                };

                AddVehicleMenu.Add(new MenuEntry<Func<IVehicle>>(
                    TypeNames[vehicleType],
                    () =>
                    {
                        Dictionary<VehicleCreationParameter, object> parameters = new Dictionary<VehicleCreationParameter, object>();
                        foreach (VehicleCreationParameter parameter in requiredParameters)
                            parameters.Add(parameter, obtainFuns[parameter]());
                        return vehicleFactory.Create(vehicleType, parameters);
                    }));
            }
            return AddVehicleMenu;
        }

        private static void DescribeAddResultToUser(AddResult addResult, ITextUI lowerUI)
        {
            switch (addResult)
            {
                case AddResult.Success:
                    lowerUI.ShowAndWaitForReadySignal("Det gick bra.");
                    break;
                case AddResult.NoGarage:
                    lowerUI.ShowAndWaitForReadySignal("Det finns inget garage.");
                    break;
                case AddResult.FullGarage:
                    lowerUI.ShowAndWaitForReadySignal("Garaget är fullt.");
                    break;
                case AddResult.DuplicateRegistrationNumber:
                    lowerUI.ShowAndWaitForReadySignal("Registreringsnumret finns redan i garaget.");
                    break;
            }
        }

        public void Run()
        {
            MainMenu.Run();
        }
    }
}
