namespace DotNetExercise5
{
    internal class GarageHandler : IHandler
    {
        Garage<IVehicle>? TheGarage { get; set; }

        public AddResult Add(IVehicle vehicle)
        {
            if (TheGarage == null)
                return AddResult.NoGarage;
            if (TheGarage.Add(vehicle)) 
                    return AddResult.Success;
            else
                    return AddResult.FullGarage;
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
    }
}
