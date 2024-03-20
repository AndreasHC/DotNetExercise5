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
            return ((IEnumerable<T>)Vehicles).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
