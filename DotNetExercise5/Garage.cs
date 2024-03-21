using System.Collections;

namespace DotNetExercise5
{
    internal class Garage<T> : IEnumerable<T> where T : IVehicle
    {
        // We are not going to replace the array without replacing the object.
        private T[] Vehicles { get; init; }
        private uint Capacity { get; set; }
        private uint Count { get; set; } = 0;

        internal Garage(uint capacity)
        {
            Vehicles = new T[capacity];
            Capacity = capacity;
        }
        public IEnumerator<T> GetEnumerator()
        {
            foreach (T vehicle in Vehicles)
                if (vehicle != null)
                    yield return vehicle;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal bool Add(T vehicle)
        {
            if (Count < Capacity)
            {
                Vehicles[Count] = vehicle;
                Count++;
                return true;
            }
            else
                return false;
        }
    }
}
