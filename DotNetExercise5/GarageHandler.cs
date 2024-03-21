namespace DotNetExercise5
{
    internal class GarageHandler : IHandler
    {
        Garage<IVehicle>? TheGarage { get; set; }

        public AddResult Add(IVehicle vehicle)
        {
            if (TheGarage == null)
                return AddResult.NoGarage;
            return TheGarage.Add(vehicle);
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

        public void MakeNewGarage(uint capacity)
        {
            TheGarage = new Garage<IVehicle>(capacity);
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
    }
}
