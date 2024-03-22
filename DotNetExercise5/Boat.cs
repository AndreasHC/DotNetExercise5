namespace DotNetExercise5
{
    internal class Boat : Vehicle
    {
        private double Length {  get; init; }
        public Boat(string registrationNumber, VehicleColor color, uint numberOfWheels, double length) : base(registrationNumber, color, numberOfWheels)
        {
            Length = length;
        }

        protected override string TypeDescription()
        {
            return "Båt";
        }
        public override string ToString()
        {
            return base.ToString() + Environment.NewLine+$"Längd: {Length}";
        }
    }
}
