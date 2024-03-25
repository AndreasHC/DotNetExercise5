using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DotNetExercise5
{
    internal class Garage<T> : IEnumerable<T> where T : class, IVehicle
    {
        // The array is supposed to have objects up to and excluding position Count, and nulls from position Count and upwards, whenever the object is not executing a method.
        private T?[] Vehicles { get; init; }
        private uint Capacity { get; init; }
        private uint Count { get; set; } = 0;

        // There might be a better place for this.
        // All keys should be return values from string.ToLower. (Which would almost certainly be easier to enforce in that better place.)
        private Dictionary<string, uint> RegistrationNumberIndex { get; init; } = new Dictionary<string, uint>();

        internal Garage(uint capacity)
        {
            Vehicles = new T[capacity];
            Capacity = capacity;
        }
        public IEnumerator<T> GetEnumerator()
        {
            foreach (T? vehicle in Vehicles)
                if (vehicle != null)
                    yield return vehicle;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal AddResult Add(T vehicle)
        {
            if (Count >= Capacity)
                return AddResult.FullGarage;
            if (RegistrationNumberIndex.ContainsKey(vehicle.RegistrationNumber.ToLower()))
                return AddResult.DuplicateRegistrationNumber;
            Vehicles[Count] = vehicle;
            RegistrationNumberIndex[vehicle.RegistrationNumber.ToLower()] = Count;
            Count++;
            return AddResult.Success;
        }

        internal bool Remove(string registrationNumber)
        {
            if (!RegistrationNumberIndex.ContainsKey(registrationNumber.ToLower()))
                return false;
            uint index = RegistrationNumberIndex[registrationNumber.ToLower()];
            Count--;
            if (Count != index)
            {
                T vehicleToMove = Vehicles[Count]!;//If the array has a null value at a position lower than Count at the start of a method, something has gone wrong already.
                // If we want to preserve insertion order, we need to do this a little differently.
                Vehicles[index] = vehicleToMove;
                RegistrationNumberIndex[vehicleToMove.RegistrationNumber.ToLower()] = index;
            }
            Vehicles[Count] = null;
            RegistrationNumberIndex.Remove(registrationNumber.ToLower());
            return true;
        }

        internal bool Retrieve(string registrationNumber, [MaybeNullWhen(false)] out IVehicle vehicle)
        {
            if (!RegistrationNumberIndex.ContainsKey(registrationNumber.ToLower()))
            {
                vehicle = null;
                return false;
            }
            uint index = RegistrationNumberIndex[registrationNumber.ToLower()];
            vehicle = Vehicles[index]!;// The registration number index is not supposed to point at null values.
            return true;
        }
    }
}
