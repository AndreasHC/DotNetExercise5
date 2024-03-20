namespace DotNetExercise5
{
    internal interface IHandler
    {
        void MakeNewGarage(uint capacity);
        bool DoToEach(Action<IVehicle> action);
    }
}
