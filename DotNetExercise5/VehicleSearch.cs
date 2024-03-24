namespace DotNetExercise5
{
    internal class VehicleSearch
    {
        private IEnumerable<IVehicle> DataSet { get; init; }
        internal VehicleSearch(IEnumerable<IVehicle> dataSet)
        {
            DataSet = dataSet;
        }
        internal IEnumerable<IVehicle> Run()
        {
            return DataSet;
        }
    }
}
