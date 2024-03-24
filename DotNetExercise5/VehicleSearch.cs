namespace DotNetExercise5
{
    internal class VehicleSearch
    {
        private IEnumerable<IVehicle> DataSet { get; init; }
        private List<VehicleCriterion> Critera { get; init; } = new List<VehicleCriterion>();
        internal VehicleSearch(IEnumerable<IVehicle> dataSet)
        {
            DataSet = dataSet;
        }
        internal IEnumerable<IVehicle> Run()
        {
            IEnumerable<IVehicle> data = DataSet;
            foreach (VehicleCriterion criterion in Critera)
            {
                data = data.Where(criterion.FilterFunction);
            }
            return data;
        }
        internal void AddCriterion(VehicleCriterion criterion)
        {
            Critera.Add(criterion);
        }
    }
}
