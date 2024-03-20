using System.Collections;

namespace DotNetExercise5
{
    internal class Garage<T> : IEnumerable<T> where T : IVehicle
    {
        // We are not going to replace the array without replacing the object.
        private T[] Vehicles { get; init; }

        internal Garage(uint capacity)
        {
            Vehicles = new T[capacity];
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
    }
}
