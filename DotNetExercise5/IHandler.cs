namespace DotNetExercise5
{
    internal interface IHandler
    {
        void MakeNewGarage(uint capacity);
        PopulateResult MakeNewGarage(uint capacity, string startingPopulation);
        List<string> AvailableStartingPopulations();
        bool DoToEach(Action<IVehicle> action);
        AddResult Add(IVehicle vehicle);
        RemoveResult Remove(string v);
        IEnumerable<IVehicle>? GetEnumerable();
    }

    // This might be better done with exceptions? I do not really have a good sense of how costly exceptions are in this environment.
    internal enum AddResult { Success, NoGarage, FullGarage, DuplicateRegistrationNumber}
    internal enum RemoveResult { Success, NoGarage, NotFound}
    internal enum PopulateResult { Success, UnknownPopulation, InsufficientCapacity}
}
