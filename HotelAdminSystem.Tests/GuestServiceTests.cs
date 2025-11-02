using System;
using HotelAdminSystem.Models;
using HotelAdminSystem.Tests.Interfaces;
using HotelAdminSystem.Tests.Services;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace HotelAdminSystem.Tests
{
    [TestFixture]
    public class GuestServiceTests
    {
        private ITestHotelService _service;

        [SetUp]
        public void Setup()
        {
            _service = new TestHotelService();
        }

        [TestCase("Иван", "Иванов", "Иванович", "+79161234567", "ivanov@mail.ru", "673456789", "1990-01-01")]
        [TestCase("Мария", "Петрова", "Ивановна", "+79169876543", "petrova@mail.ru", "982744321", "1997-11-21")]
        [TestCase("Елена", "Попова", "Ивановна", "+79169457679", "popova@mail.ru", "117654333", "1956-06-15")]
        public void AdditingValidGuest(string firstName, string middleName, string lastName, string phoneNumber, string email,
            string passportNumber, string dateOfBirth)
        {
            var date = DateTime.Parse(dateOfBirth);
            var result = _service.AddGuest(firstName, middleName, lastName, phoneNumber, email, passportNumber, date);
            Assert.That(result, Is.True);
        }

        [TestCase(25, "Мирослав", "Иванов", "Иванович", "+79161234567", "ivanov@mail.ru", "673456789", "1990-01-01")]
        [TestCase(25, "Мария", "Петрова", "Ивановна", "+79169876543", "petrova@mail.ru", "982744321", "1997-11-21")]
        [TestCase(25, "Елена", "Попова", "Ивановна", "+79169457679", "popova@mail.ru", "117654333", "1956-06-15")]
        public void UpdatingValidGuest(int id, string firstName, string middleName, string lastName, string phoneNumber, string email,
            string passportNumber, string dateOfBirth)
        {
            var guest = new Guest
            {
                Id = id,
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванович",
                PhoneNumber = "+79991234567",
                Email = "ivan.ivanov@example.com",
                Passport = "1234 567890",
                DateOfBirth = new DateTime(1990, 5, 14)
            };
            _service.AddGuest(guest);

            _service.UpdateGuest(id, firstName, middleName, lastName, phoneNumber, email, passportNumber,
                DateTime.Parse(dateOfBirth));
            var newGuest = _service.GetGuestById(guest.Id);
            
            Assert.IsFalse(newGuest.Equals(guest));
        }
        
        [Test]
        public void DeletingGuest()
        {
            var guest = _service.GetGuestById(4);
            if (guest == null)
                Assert.Fail();

            var result = _service.DeleteGuest(4);
            Assert.IsTrue(result);
        }

        [Test]
        public void GettingUserById()
        {
            var guest = _service.GetGuestById(4);
            Assert.IsNotNull(guest);
        }

        [Test]
        public void GettingAllUsers()
        {
            var guest1 = new Guest
            {
                Id = 11,
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванович",
                PhoneNumber = "+79991234567",
                Email = "ivan.ivanov@example.com",
                Passport = "1234 567890",
                DateOfBirth = new DateTime(1990, 5, 14)
            };
            var guest2 = new Guest
            {
                Id = 12,
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванович",
                PhoneNumber = "+79991234567",
                Email = "ivan.ivanov@example.com",
                Passport = "1234 567890",
                DateOfBirth = new DateTime(1990, 5, 14)
            };
            var guest3 = new Guest
            {
                Id = 13,
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванович",
                PhoneNumber = "+79991234567",
                Email = "ivan.ivanov@example.com",
                Passport = "1234 567890",
                DateOfBirth = new DateTime(1990, 5, 14)
            };

            _service.AddGuest(guest1);
            _service.AddGuest(guest2);
            _service.AddGuest(guest3);
            
            var guests = _service.GetAllGuests();
            Assert.IsNotEmpty(guests);
        }
    }
}