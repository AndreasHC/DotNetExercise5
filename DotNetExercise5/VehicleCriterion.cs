namespace DotNetExercise5
{
    internal class VehicleCriterion
    {
        private string Description { get; init; }
        internal Func<IVehicle, bool> FilterFunction { get; init; }

        public VehicleCriterion(string description, Func<IVehicle, bool> filterFunction)
        {
            Description = description;
            FilterFunction = filterFunction;
        }
        public override string ToString()
        {
            return Description;
        }
    }
}
