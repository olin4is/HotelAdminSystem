using System;
using System.Linq;
using HotelAdminSystem.Models;
using HotelAdminSystem.Services;
using NUnit.Framework;

namespace HotelAdminSystem.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var hotelService = new HotelService();
            var testGuestModel = new Guest()
            {
                Id = 1,
                FirstName = "Мирослав",
                LastName = "Добролюбов",
                MiddleName = "Романович",
                PhoneNumber = "12345678",
                Email = "yarilo@mail.ru",
                Passport = "1234 123456",
                DateOfBirth = new DateTime(1980, 3, 12)
            };
            hotelService.AddGuest("Мирослав", "Добролюбов", "Романович", "12345678", "yarilo@mail.ru", "1234 123456", new DateTime(1980, 3, 12));
            var createdGuest = hotelService.GetAllGuests().LastOrDefault();
            Assert.AreEqual(testGuestModel, createdGuest);
        }
    }
}