namespace DotNetExercise5
{
    internal class Vehicle : IVehicle
    {
        private string RegistrationNumber { get; init; }
        private VehicleColor Color { get; init; }
        private uint NumberOfWheels { get; init; }

        public Vehicle(string registrationNumber, VehicleColor color, uint numberOfWheels)
        {
            RegistrationNumber = registrationNumber;
            Color = color;
            NumberOfWheels = numberOfWheels;
        }

        protected virtual string TypeDescription()
        {
            return "Obeskrivligt fordon";
        }

        public override string ToString()
        {
            return $"{TypeDescription()}{Environment.NewLine}Registreringsnummer: {RegistrationNumber}{Environment.NewLine}Färg: {Color.Swedish()}{Environment.NewLine}Antal hjul: {NumberOfWheels}";
        }
    }
}
