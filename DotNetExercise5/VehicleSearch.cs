using System.Text;

namespace DotNetExercise5
{
    internal class VehicleSearch
    {
        private IEnumerable<IVehicle> DataSet { get; init; }
        private List<VehicleCriterion> Criteria { get; init; } = new List<VehicleCriterion>();
        internal VehicleSearch(IEnumerable<IVehicle> dataSet)
        {
            DataSet = dataSet;
        }
        internal IEnumerable<IVehicle> Run()
        {
            IEnumerable<IVehicle> data = DataSet;
            foreach (VehicleCriterion criterion in Criteria)
            {
                data = data.Where(criterion.FilterFunction);
            }
            return data;
        }
        internal void AddCriterion(VehicleCriterion criterion)
        {
            Criteria.Add(criterion);
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            foreach (VehicleCriterion criterion in Criteria)
            {
                buffer.Append(criterion.ToString());
                buffer.Append(Environment.NewLine);
            }
            return buffer.ToString();
        }
    }
}
