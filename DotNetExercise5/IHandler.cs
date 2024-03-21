namespace DotNetExercise5
{
    internal interface IHandler
    {
        void MakeNewGarage(uint capacity);
        bool DoToEach(Action<IVehicle> action);
        AddResult Add(IVehicle vehicle);
    }

    internal enum AddResult { Success, NoGarage, FullGarage}
}
