namespace DotNetExercise5
{
    internal class GarageHandler : IHandler
    {
        Garage<IVehicle>? TheGarage { get; set; }

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
