namespace DotNetExercise5
{
    internal class Airplane : Vehicle
    {
        private double Wingspan { get; init; }
        public Airplane(string registrationNumber, VehicleColor color, uint numberOfWheels, double wingspan) : base(registrationNumber, color, numberOfWheels)
        {
            Wingspan = wingspan;
        }

        protected override string TypeDescription()
        {
            return "Flygplan";
        }

        public override string ToString()
        {
            return base.ToString() +  Environment.NewLine + $"Vingspann: {Wingspan}";
        }
    }
}
