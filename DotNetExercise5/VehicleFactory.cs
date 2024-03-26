
using System.Collections.Generic;

namespace DotNetExercise5
{
    internal class VehicleFactory : IVehicleFactory
    {
        public List<Type> AvaliableTypes()
        {
            return new List<Type>()
            {
                typeof(Vehicle),
                typeof(Airplane),
                typeof(Boat),
                typeof(Bus),
                typeof(Car),
                typeof(Motorcycle),
            };
        }

        public IVehicle Create(Type vehicleType, Dictionary<VehicleCreationParameter, object> parameters)
        {
            if (vehicleType == typeof(Vehicle))
            {
                return new Vehicle((string)parameters[VehicleCreationParameter.RegistrationNumber], (VehicleColor)parameters[VehicleCreationParameter.Color], (uint)parameters[VehicleCreationParameter.NumberOfWheels]);
            }
            else if (vehicleType == typeof(Airplane))
            {
                return new Airplane((string)parameters[VehicleCreationParameter.RegistrationNumber], (VehicleColor)parameters[VehicleCreationParameter.Color], (uint)parameters[VehicleCreationParameter.NumberOfWheels], (double)parameters[VehicleCreationParameter.WingSpan]);
            }
            else if (vehicleType == typeof(Boat))
            {
                return new Boat((string)parameters[VehicleCreationParameter.RegistrationNumber], (VehicleColor)parameters[VehicleCreationParameter.Color], (uint)parameters[VehicleCreationParameter.NumberOfWheels], (double)parameters[VehicleCreationParameter.Length]);
            }
            else if (vehicleType == typeof(Bus))
            {
                return new Bus((string)parameters[VehicleCreationParameter.RegistrationNumber], (VehicleColor)parameters[VehicleCreationParameter.Color], (uint)parameters[VehicleCreationParameter.NumberOfWheels], (uint)parameters[VehicleCreationParameter.NumberOfSeats]);
            }
            else if (vehicleType == typeof(Car))
            {
                return new Car((string)parameters[VehicleCreationParameter.RegistrationNumber], (VehicleColor)parameters[VehicleCreationParameter.Color], (uint)parameters[VehicleCreationParameter.NumberOfWheels], (uint)parameters[VehicleCreationParameter.NumberOfDoors]);
            }
            else if (vehicleType == typeof(Motorcycle))
            {
                return new Motorcycle((string)parameters[VehicleCreationParameter.RegistrationNumber], (VehicleColor)parameters[VehicleCreationParameter.Color], (uint)parameters[VehicleCreationParameter.NumberOfWheels], (uint)parameters[VehicleCreationParameter.NumberOfGears]);
            }

            else
                throw new ArgumentException($"Vehicle factory asked to create vehicle of unknown type: {vehicleType}.");
        }

        public HashSet<VehicleCreationParameter> RequiredParameters(Type vehicleType)
        {
            if (vehicleType == typeof(Vehicle))
                return new HashSet<VehicleCreationParameter>() { VehicleCreationParameter.RegistrationNumber, VehicleCreationParameter.Color, VehicleCreationParameter.NumberOfWheels };
            else if (vehicleType == typeof(Airplane))
                return new HashSet<VehicleCreationParameter>() { VehicleCreationParameter.RegistrationNumber, VehicleCreationParameter.Color, VehicleCreationParameter.NumberOfWheels, VehicleCreationParameter.WingSpan };
            else if (vehicleType == typeof(Boat))
                return new HashSet<VehicleCreationParameter>() { VehicleCreationParameter.RegistrationNumber, VehicleCreationParameter.Color, VehicleCreationParameter.NumberOfWheels, VehicleCreationParameter.Length };
            else if (vehicleType == typeof(Bus))
                return new HashSet<VehicleCreationParameter>() { VehicleCreationParameter.RegistrationNumber, VehicleCreationParameter.Color, VehicleCreationParameter.NumberOfWheels, VehicleCreationParameter.NumberOfSeats };
            else if (vehicleType == typeof(Car))
                return new HashSet<VehicleCreationParameter>() { VehicleCreationParameter.RegistrationNumber, VehicleCreationParameter.Color, VehicleCreationParameter.NumberOfWheels, VehicleCreationParameter.NumberOfDoors };
            else if (vehicleType == typeof(Motorcycle))
                return new HashSet<VehicleCreationParameter>() { VehicleCreationParameter.RegistrationNumber, VehicleCreationParameter.Color, VehicleCreationParameter.NumberOfWheels, VehicleCreationParameter.NumberOfGears };
            else
                throw new ArgumentException($"Vehicle factory queried about unknown vehicle type: {vehicleType}.");
        }
    }
}

