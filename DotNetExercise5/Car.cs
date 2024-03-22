namespace DotNetExercise5
{
    internal class Car : Vehicle
    {
        private uint NumberOfDoors {  get; init; }
        public Car(string registrationNumber, VehicleColor color, uint numberOfWheels, uint numberOfDoors) : base(registrationNumber, color, numberOfWheels)
        {
            NumberOfDoors = numberOfDoors;
        }
        protected override string TypeDescription()
        {
            return "Bil";
        }
        public override string ToString()
        {
            return base.ToString() + Environment.NewLine + $"Antal dörrar: {NumberOfDoors}";
        }
    }
}
