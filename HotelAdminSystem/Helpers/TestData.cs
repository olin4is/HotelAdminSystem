using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelAdminSystem.Enums;
using HotelAdminSystem.Models;

namespace HotelAdminSystem.Helpers
{
    internal static class TestData
    {
        public static List<Room> Rooms = new List<Room>() {
            new Room { RoomNumber = 101, Type = RoomType.Single, PricePerNight = 2500, Capacity = 1 },
            new Room { RoomNumber = 102, Type = RoomType.Double, PricePerNight = 3500, Capacity = 2 },
            new Room { RoomNumber = 103, Type = RoomType.Double, PricePerNight = 3500, Capacity = 2 },
            new Room { RoomNumber = 104, Type = RoomType.Double, PricePerNight = 3500, Capacity = 2 },
            new Room { RoomNumber = 201, Type = RoomType.Suite, PricePerNight = 5000, Capacity = 3 },
            new Room { RoomNumber = 202, Type = RoomType.Suite, PricePerNight = 5000, Capacity = 3 },
            new Room { RoomNumber = 203, Type = RoomType.Suite, PricePerNight = 5000, Capacity = 3 },
            new Room { RoomNumber = 204, Type = RoomType.Deluxe, PricePerNight = 7500, Capacity = 4 },
            new Room { RoomNumber = 301, Type = RoomType.Presidential, PricePerNight = 15000, Capacity = 6 }
        };
        public static List<Guest> Guests = new List<Guest>
        {
            new Guest { Id = 1, FirstName = "Иван", LastName = "Иванов", MiddleName = "Иванович", PhoneNumber = "+79161234567", Passport = "123456789", DateOfBirth = DateTime.Today, Email = "ivanov@mail.ru" },
            new Guest { Id = 2, FirstName = "Мария", LastName = "Петрова", MiddleName = "Ивановна", PhoneNumber = "+79169876543", Passport = "987654321", DateOfBirth = DateTime.Today, Email = "petrova@mail.ru" },
            new Guest { Id = 3, FirstName = "Елена", LastName = "Попова", MiddleName = "Ивановна", PhoneNumber = "+79169457679", Passport = "987654321", DateOfBirth = DateTime.Today, Email = "popova@mail.ru" },
            new Guest { Id = 4, FirstName = "Юрий", LastName = "Гончаров", MiddleName = "Иванович", PhoneNumber = "+791698675473", Passport = "987654321", DateOfBirth = DateTime.Today, Email = "goncharov@mail.ru" },
            new Guest { Id = 5, FirstName = "Александр", LastName = "Денисов", MiddleName = "Иванович", PhoneNumber = "+79169876675", Passport = "987654321", DateOfBirth = DateTime.Today, Email = "denisov@mail.ru" },
            new Guest { Id = 6, FirstName = "Алексей", LastName = "Сидоров", MiddleName = "Иванович", PhoneNumber = "+79161112233", Passport = "456789123", DateOfBirth = DateTime.Today, Email = "sidorov@mail.ru" }
        };
        public static List<Booking> Bookings = new List<Booking> 
        {
            new Booking { Id = 1, RoomNumber = 101, GuestId = 1, StartDate = DateTime.Today,
                         EndDate = DateTime.Today.AddDays(3), Status = BookingStatus.Pending,
                         BookingDate = DateTime.Now, TotalPrice = 7500 },
            new Booking { Id = 2, RoomNumber = 102, GuestId = 2, StartDate = DateTime.Today.AddDays(1),
                         EndDate = DateTime.Today.AddDays(5), Status = BookingStatus.Pending,
                         BookingDate = DateTime.Now, TotalPrice = 14000 }
        };
    }
}
