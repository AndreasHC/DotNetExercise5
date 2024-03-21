namespace DotNetExercise5
{
    internal interface IHandler
    {
        void MakeNewGarage(uint capacity);
        bool DoToEach(Action<IVehicle> action);
        AddResult Add(IVehicle vehicle);
        RemoveResult Remove(string v);
    }

    internal enum AddResult { Success, NoGarage, FullGarage, DuplicateRegistrationNumber}
    internal enum RemoveResult { Success, NoGarage, NotFound}
}
