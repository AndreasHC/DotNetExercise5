
namespace DotNetExercise5
{
    internal class SearchHandler : ISearchHandler
    {
        private VehicleSearch? Search { get; set; }
        internal SearchHandler() { }

        public void Init(IEnumerable<IVehicle> enumerable)
        {
            Search = new VehicleSearch(enumerable);
        }

        public void AddCriterion(VehicleCriterion vehicleCriterion)
        {
            (Search ?? throw new InvalidOperationException("Tried to add criterion to uninitialized search handler.")).AddCriterion(vehicleCriterion);
        }

        public string GetCriteriaString()
        {
            return (Search ?? throw new InvalidOperationException("Tried to list criteria from uninitialized search handler.")).ToString();
        }

        public IEnumerable<IVehicle> Run()
        {
            return (Search ?? throw new InvalidOperationException("Tried to execute search through uninitialized search handler.")).Run();
        }
    }
}
