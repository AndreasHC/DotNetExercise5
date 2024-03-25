
using System.Diagnostics.CodeAnalysis;

namespace DotNetExercise5
{
    internal class GarageHandler : IHandler
    {
        Garage<IVehicle>? TheGarage { get; set; }

        // I have a strong feeling that creating the actual vehicles along with the handler is just wrong, and I cannot say anything sensible about why.
        private Dictionary<string, List<Func<IVehicle>>> StartingPopulations { get; } =
            new Dictionary<string, List<Func<IVehicle>>>()
            {
                {"tom", new List<Func<IVehicle>>() },
                {"en av varje", new List<Func<IVehicle>>()
                    {
                        () => new Vehicle("abc123", VehicleColor.Black, 12),
                        () => new Airplane("cde345", VehicleColor.White, 3, 12.0),
                        () => new Boat("12345678", VehicleColor.Blue, 0, 12.5),
                        () => new Bus("jkl567", VehicleColor.Red, 12, 60),
                        () => new Car("vanity", VehicleColor.Blue, 4, 4),
                        () => new Motorcycle("12345", VehicleColor.Green, 2, 6)
                    }
                },

            };

        public AddResult Add(IVehicle vehicle)
        {
            if (TheGarage == null)
                return AddResult.NoGarage;
            return TheGarage.Add(vehicle);
        }

        public List<string> AvailableStartingPopulations()
        {
            return StartingPopulations.Keys.ToList();
        }

        public bool DoToEach(Action<IVehicle> action)
        {
            if (TheGarage != null)
            {
                foreach (IVehicle vehicle in TheGarage)
                    action(vehicle);
                return true;
            }
            return false;

        }

        public IEnumerable<IVehicle>? GetEnumerable()
        {
            return TheGarage;
        }

        [MemberNotNull(nameof(TheGarage))]
        public void MakeNewGarage(uint capacity)
        {
            TheGarage = new Garage<IVehicle>(capacity);
        }

        public PopulateResult MakeNewGarage(uint capacity, string startingPopulation)
        {
            MakeNewGarage(capacity);
            if (!StartingPopulations.ContainsKey(startingPopulation))
                return PopulateResult.UnknownPopulation;
            if (StartingPopulations[startingPopulation].Count > capacity)
                return PopulateResult.InsufficientCapacity;
            foreach (Func<IVehicle> generateFun in StartingPopulations[startingPopulation])
                TheGarage.Add(generateFun());
            return PopulateResult.Success;
        }

        public RemoveResult Remove(string registrationNumber)
        {
            if (TheGarage == null)
                return RemoveResult.NoGarage;

            if (TheGarage.Remove(registrationNumber))
                return RemoveResult.Success;
            else
                return RemoveResult.NotFound;
        }
        public bool Retrieve(string registrationNumber, out RetrieveObstacle obstacle, [MaybeNullWhen(false)] out IVehicle vehicle)
        {
            if (TheGarage == null)
            {
                obstacle = RetrieveObstacle.NoGarage;
                vehicle = null;
                return false;
            }
            if (TheGarage.Retrieve(registrationNumber, out vehicle))
            {
                obstacle = RetrieveObstacle.None;
                return true;
            }
            else
                obstacle = RetrieveObstacle.NotFound;
                return false;

        }
    }
}
