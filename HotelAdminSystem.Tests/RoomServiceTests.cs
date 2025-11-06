using System;
using HotelAdminSystem.Enums;
using HotelAdminSystem.Interfaces;
using HotelAdminSystem.Models;
using HotelAdminSystem.Tests.Interfaces;
using HotelAdminSystem.Tests.Services;
using NUnit.Framework;

namespace HotelAdminSystem.Tests
{
    public class RoomServiceTests
    {
        private ITestHotelService _service;

        [SetUp]
        public void Setup()
        {
            _service = new TestHotelService();
        }

        [Test]
        public void GettingAllRooms()
        {
            var room1 = new Room { RoomNumber = 901, Type = RoomType.Single, PricePerNight = 2500, Capacity = 1 };
            var room2 = new Room { RoomNumber = 902, Type = RoomType.Double, PricePerNight = 3500, Capacity = 2 };
            var room3 = new Room { RoomNumber = 903, Type = RoomType.Double, PricePerNight = 3500, Capacity = 2 };
            
            _service.AddRoom(room1);
            _service.AddRoom(room2);
            _service.AddRoom(room3);

            var rooms = _service.GetAllRooms();
            Assert.IsNotEmpty(rooms);
        }

        [Test]
        public void GettingRoomByNumber()
        {
            var room1 = new Room { RoomNumber = 901, Type = RoomType.Single, PricePerNight = 2500, Capacity = 1 };
            _service.AddRoom(room1);
            var room = _service.GetRoomByNumber(room1.RoomNumber);
            Assert.IsNotNull(room);
        }

        [TestCase(999)]
        [TestCase(-1)]
        public void GettingNotExistingRoom(int roomNumber)
        {
            var room1 = new Room { RoomNumber = 901, Type = RoomType.Single, PricePerNight = 2500, Capacity = 1 };
            _service.AddRoom(room1);
            var room = _service.GetRoomByNumber(roomNumber);
            Assert.IsNull(room);
        }

        [Test]
        public void GettingAvaliableRoomsByDate()
        {
            var room1 = new Room { RoomNumber = 901, Type = RoomType.Single, PricePerNight = 2500, Capacity = 1 };
            var room2 = new Room { RoomNumber = 902, Type = RoomType.Double, PricePerNight = 3500, Capacity = 2 };
            var room3 = new Room { RoomNumber = 903, Type = RoomType.Double, PricePerNight = 3500, Capacity = 2 };
            
            _service.AddRoom(room1);
            _service.AddRoom(room2);
            _service.AddRoom(room3);
            
            var rooms = _service.GetAvailableRooms(DateTime.Today, DateTime.Today.AddDays(3));
            Assert.IsNotEmpty(rooms);
            
        }

        [Test]
        public void CheckIfRoomIsAvailable()
        {
            var room1 = new Room { RoomNumber = 901, Type = RoomType.Single, PricePerNight = 2500, Capacity = 1 };
            _service.AddRoom(room1);
            var result = _service.IsRoomAvailable(room1.RoomNumber, DateTime.Today, DateTime.Today.AddDays(3));
            Assert.IsTrue(result);
        }
    }
}