namespace DotNetExercise5
{
    internal class Car : Vehicle
    {
        public Car(string registrationNumber, VehicleColor color, uint numberOfWheels) : base(registrationNumber, color, numberOfWheels)
        {
        }
        protected override string TypeDescription()
        {
            return "Bil";
        }
    }
}
