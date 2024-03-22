namespace DotNetExercise5
{
    internal class Bus : Vehicle
    {
        public Bus(string registrationNumber, VehicleColor color, uint numberOfWheels) : base(registrationNumber, color, numberOfWheels)
        {
        }
        protected override string TypeDescription()
        {
            return "Buss";
        }
    }
}
