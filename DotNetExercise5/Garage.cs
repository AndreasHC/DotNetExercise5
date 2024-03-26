using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DotNetExercise5GarageTest")]
namespace DotNetExercise5
{
    internal class Garage<T> : IEnumerable<T> where T : class, IVehicle
    {
        // The array is supposed to have objects up to and excluding position Count, and nulls from position Count and upwards, whenever the object is not executing a method.
        private T?[] Vehicles { get; init; }
        private uint Capacity { get; init; }
        private uint Count { get; set; } = 0;

        private CaseInsensitiveStringDictionary<uint> RegistrationNumberIndex { get; init; } = new CaseInsensitiveStringDictionary<uint>();

        internal Garage(uint capacity)
        {
            Vehicles = new T[capacity];
            Capacity = capacity;
        }
        public IEnumerator<T> GetEnumerator()
        {
            // I imagine that there is some combination of used and unused capacity where using the registration number index as the basis for this would give better performance.
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
            if (RegistrationNumberIndex.ContainsKey(vehicle.RegistrationNumber))
                return AddResult.DuplicateRegistrationNumber;
            Vehicles[Count] = vehicle;
            RegistrationNumberIndex[vehicle.RegistrationNumber] = Count;
            Count++;
            return AddResult.Success;
        }

        internal bool Remove(string registrationNumber)
        {
            if (!RegistrationNumberIndex.ContainsKey(registrationNumber))
                return false;
            uint index = RegistrationNumberIndex[registrationNumber];
            Count--;
            if (Count != index)
            {
                T vehicleToMove = Vehicles[Count]!;//If the array has a null value at a position lower than Count at the start of a method, something has gone wrong already.
                // If we want to preserve insertion order, we need to do this a little differently.
                Vehicles[index] = vehicleToMove;
                RegistrationNumberIndex[vehicleToMove.RegistrationNumber] = index;
            }
            Vehicles[Count] = null;
            RegistrationNumberIndex.Remove(registrationNumber);
            return true;
        }

        internal bool Retrieve(string registrationNumber, [MaybeNullWhen(false)] out IVehicle vehicle)
        {
            if (!RegistrationNumberIndex.ContainsKey(registrationNumber))
            {
                vehicle = null;
                return false;
            }
            uint index = RegistrationNumberIndex[registrationNumber];
            vehicle = Vehicles[index]!;// The registration number index is not supposed to point at null values.
            return true;
        }
        // Subclassing Dictionary would have been less writing, but at a higher risk of missing something.
        private class CaseInsensitiveStringDictionary<TInner> : IDictionary<string, TInner>
        {
            private Dictionary<string, TInner> Inner { get; init; } = new Dictionary<string, TInner>();
            public TInner this[string key] { get => Inner[key.ToLower()]; set => Inner[key.ToLower()] = value; }

            public ICollection<string> Keys => Inner.Keys;

            public ICollection<TInner> Values => Inner.Values;

            public int Count => Inner.Count;

            public bool IsReadOnly => ((ICollection<KeyValuePair<string, TInner>>)Inner).IsReadOnly;

            public void Add(string key, TInner value)
            {
                Inner.Add(key.ToLower(), value);
            }

            public void Add(KeyValuePair<string, TInner> item)
            {
                Add(item.Key, item.Value);
            }

            public void Clear()
            {
                Inner.Clear();
            }

            public bool Contains(KeyValuePair<string, TInner> item)
            {
                return Inner.Contains(new KeyValuePair<string, TInner>(item.Key.ToLower(), item.Value));
            }

            public bool ContainsKey(string key)
            {
                return Inner.ContainsKey(key.ToLower());
            }

            public void CopyTo(KeyValuePair<string, TInner>[] array, int arrayIndex)
            {
                ((ICollection<KeyValuePair<string, TInner>>)Inner).CopyTo(array, arrayIndex);
            }

            public IEnumerator<KeyValuePair<string, TInner>> GetEnumerator()
            {
                return ((ICollection<KeyValuePair<string, TInner>>)Inner).GetEnumerator();
            }

            public bool Remove(string key)
            {
                return Inner.Remove(key.ToLower());
            }

            public bool Remove(KeyValuePair<string, TInner> item)
            {
                return ((ICollection<KeyValuePair<string, TInner>>)Inner).Remove(new KeyValuePair<string, TInner>(item.Key.ToLower(), item.Value));
            }

            public bool TryGetValue(string key, [MaybeNullWhen(false)] out TInner value)
            {
                return Inner.TryGetValue(key.ToLower(), out value);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return Inner.GetEnumerator();
            }
        }

    }
}
