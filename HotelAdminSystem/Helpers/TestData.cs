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
            new Guest { Id = 1, FirstName = "Иван", LastName = "Иванов", MiddleName = "Иванович", PhoneNumber = "+79161234567", Passport = "123456789" },
            new Guest { Id = 2, FirstName = "Мария", LastName = "Петрова", MiddleName = "Ивановна", PhoneNumber = "+79169876543", Passport = "987654321" },
            new Guest { Id = 3, FirstName = "Алексей", LastName = "Сидоров", MiddleName = "Иванович", PhoneNumber = "+79161112233", Passport = "456789123" }
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
