namespace DotNetExercise5
{
    internal class Motorcycle : Vehicle
    {
        private uint NumberOfGears {  get; init; }
        public Motorcycle(string registrationNumber, VehicleColor color, uint numberOfWheels, uint numberOfGears) : base(registrationNumber, color, numberOfWheels)
        {
            NumberOfGears = numberOfGears;
        }
        protected override string TypeDescription()
        {
            return "Motorcykel";
        }
        public override string ToString()
        {
            return base.ToString() + Environment.NewLine + $"Antal växlar: {NumberOfGears}";
        }
    }
}
