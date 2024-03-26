
namespace DotNetExercise5
{
    internal interface IVehicleFactory
    {
        List<Type> AvaliableTypes();
        IVehicle Create(Type vehicleType, Dictionary<VehicleCreationParameter, object> parameters);
        HashSet<VehicleCreationParameter> RequiredParameters(Type vehicleType);
    }
    internal enum VehicleCreationParameter
    {
        RegistrationNumber,
        Color,
        NumberOfWheels,
        WingSpan,
        NumberOfSeats,
        NumberOfDoors,
        Length,
        NumberOfGears
    }
}
