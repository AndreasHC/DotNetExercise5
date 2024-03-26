
namespace DotNetExercise5
{
    internal interface ISearchHandler
    {
        void AddCriterion(VehicleCriterion vehicleCriterion);
        string GetCriteriaString();
        void Init(IEnumerable<IVehicle> enumerable);
        IEnumerable<IVehicle> Run();
    }
}
