namespace DotNetExercise5
{
    internal class Motorcycle : Vehicle
    {
        public Motorcycle(string registrationNumber, VehicleColor color, uint numberOfWheels) : base(registrationNumber, color, numberOfWheels)
        {
        }
        protected override string TypeDescription()
        {
            return "Motorcykel";
        }
    }
}
