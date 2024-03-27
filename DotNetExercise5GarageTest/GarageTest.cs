using DotNetExercise5;

namespace DotNetExercise5GarageTest
{
    public class GarageTest
    {
        [Fact]
        public void GetEnumeratorGeneric_EmptyGarage_EmptyEnumerator()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(0);

            //Act
            IEnumerator<IVehicle> enumerator = garage.GetEnumerator();

            //Assert
            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void GetEnumeratorGeneric_GarageWithVehicle_EnumeratorWithContent()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            _ = garage.Add(new MockVehicle("abc123"));

            //Act
            IEnumerator<IVehicle> enumerator = garage.GetEnumerator();

            //Assert
            Assert.True(enumerator.MoveNext());
        }

        [Fact]
        public void GetEnumeratorNonGeneric_EmptyGarage_EmptyEnumerator()
        {

            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(0);

            //Act
            System.Collections.IEnumerator enumerator = ((System.Collections.IEnumerable)garage).GetEnumerator();

            //Assert
            Assert.False(enumerator.MoveNext());
        }
        [Fact]
        public void GetEnumeratorNonGeneric_GarageWithVehicle_EnumeratorWithContent()
        {

            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            _ = garage.Add(new MockVehicle("abc123"));

            //Act
            System.Collections.IEnumerator enumerator = ((System.Collections.IEnumerable)garage).GetEnumerator();

            //Assert
            Assert.True(enumerator.MoveNext());
        }
        [Fact]
        public void Add_GarageWithSpace_Success()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            AddResult expected = AddResult.Success;
            //Act
            AddResult result = garage.Add(new MockVehicle("abc123"));
            //Assert
            Assert.Equal(result, expected);
        }
        [Fact]
        public void Add_GarageWithoutSpace_FullGarageFailure()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(0);
            AddResult expected = AddResult.FullGarage;
            //Act
            AddResult result = garage.Add(new MockVehicle("abc123"));
            //Assert
            Assert.Equal(result, expected);
        }
        [Fact]
        public void Add_GarageWithSpaceAndConflictingRegistrationNumber_DuplicateRegistrationNumberFailure()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(2);
            _ = garage.Add(new MockVehicle("abc123"));
            AddResult expected = AddResult.DuplicateRegistrationNumber;
            //Act
            AddResult result = garage.Add(new MockVehicle("abc123"));

            //Assert
            Assert.Equal(result, expected);
        }
        [Fact]
        public void Add_NoParticularObstacles_VehicleCanBeFound()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            IVehicle vehicle = new MockVehicle("abc123");
            //Act
            _ = garage.Add(vehicle);
            //Assert
            Assert.True(garage.Where((IVehicle v) => v == vehicle).Any());
        }
        [Fact]
        public void Add_RegistrationNumberConflict_OldStaysNewNotAdded()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(2);
            IVehicle vehicle1 = new MockVehicle("abc123");
            IVehicle vehicle2 = new MockVehicle("abc123");
            _ = garage.Add(vehicle1);
            //Act
            _ = garage.Add(vehicle2);
            //Assert
            Assert.True(garage.Where((IVehicle v) => v == vehicle1).Any());
            Assert.False(garage.Where((IVehicle v) => v == vehicle2).Any());
        }
        [Fact]
        public void Remove_EmptyGarage_NotFound()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(0);
            string registrationNumber = "abc123";
            //Act
            bool result = garage.Remove(registrationNumber);
            //Assert
            Assert.False(result);
        }
        [Fact]
        public void Remove_RegistrationNumberPresent_Success()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            string registrationNumber = "abc123";
            garage.Add(new MockVehicle(registrationNumber));
            //Act
            bool result = garage.Remove(registrationNumber);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void Remove_RegistrationNumberPresentInDifferentCase_Success()

        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            string registrationNumber = "aBc123";
            garage.Add(new MockVehicle(registrationNumber.ToLower()));
            //Act
            bool result = garage.Remove(registrationNumber.ToUpper());
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void Remove_RegistrationNumberPresent_RegistrationNumberNoLongerPresent()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            string registrationNumber = "abc123";
            garage.Add(new MockVehicle(registrationNumber));
            //Act
            _ = garage.Remove(registrationNumber);
            //Assert
            Assert.False(garage.Where((IVehicle v) => v.RegistrationNumber == registrationNumber).Any());
        }
        [Fact]
        public void Retrieve_EmptyGarage_NotFound()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(0);
            string registrationNumber = "abc123";
            //Act
            bool result = garage.Retrieve(registrationNumber, out _);
            //Assert
            Assert.False(result);
        }
        [Fact]
        public void Retrieve_RegistrationNumberPresent_Success()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            string registrationNumber = "abc123";
            garage.Add(new MockVehicle(registrationNumber));
            //Act
            bool result = garage.Retrieve(registrationNumber, out _);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void Retrieve_RegistrationNumberPresentInDifferentCase_Success()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            string registrationNumber = "aBc123";
            garage.Add(new MockVehicle(registrationNumber.ToLower()));
            //Act
            bool result = garage.Retrieve(registrationNumber.ToUpper(), out _);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void Retrieve_RegistrationNumberPresent_ElementRetrieved()
        {
            //Arrange
            Garage<IVehicle> garage = new Garage<IVehicle>(1);
            string registrationNumber = "abc123";
            IVehicle vehicle = new MockVehicle(registrationNumber);
            garage.Add(vehicle);
            //Act
            _ = garage.Retrieve(registrationNumber, out IVehicle? result);
            //Assert
            Assert.Equal(vehicle, result);
        }

        private class MockVehicle : IVehicle
        {
            public string RegistrationNumber { get; init; }
            public VehicleColor Color => VehicleColor.Green;
            public uint NumberOfWheels => 4;
            public MockVehicle(string registrationNumber)
            {
                RegistrationNumber = registrationNumber;
            }
        }
    }
}