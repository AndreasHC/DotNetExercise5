namespace DotNetExercise5
{
    internal class Airplane : Vehicle
    {
        public Airplane(string registrationNumber, VehicleColor color, uint numberOfWheels) : base(registrationNumber, color, numberOfWheels)
        {
        }

        protected override string TypeDescription()
        {
            return "Flygplan";
        }
    }
}
