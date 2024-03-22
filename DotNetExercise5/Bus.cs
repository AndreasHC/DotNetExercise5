namespace DotNetExercise5
{
    internal class Bus : Vehicle
    {
        private uint NumberOfSeats {  get; init; }
        public Bus(string registrationNumber, VehicleColor color, uint numberOfWheels, uint numberOfSeats) : base(registrationNumber, color, numberOfWheels)
        {
            NumberOfSeats = numberOfSeats;  
        }
        protected override string TypeDescription()
        {
            return "Buss";
        }
        public override string ToString()
        {
            return base.ToString()+Environment.NewLine+$"Antal säten: {NumberOfSeats}";
        }
    }
}
