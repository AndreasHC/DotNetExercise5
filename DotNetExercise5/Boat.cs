namespace DotNetExercise5
{
    internal class Boat : Vehicle
    {
        public Boat(string registrationNumber, VehicleColor color, uint numberOfWheels) : base(registrationNumber, color, numberOfWheels)
        {
        }

        protected override string TypeDescription()
        {
            return "Båt";
        }
    }
}
