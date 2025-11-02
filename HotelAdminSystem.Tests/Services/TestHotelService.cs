using System.Linq;
using HotelAdminSystem.Models;
using HotelAdminSystem.Services;
using HotelAdminSystem.Tests.Interfaces;

namespace HotelAdminSystem.Tests.Services
{
    public class TestHotelService : HotelService, ITestHotelService
    {
        public void AddBooking(Booking booking)
        {
            bookings.Add(new Booking()
            {
                Id = booking.Id,
                BookingDate = booking.BookingDate,
                RoomNumber = booking.RoomNumber,
                TotalPrice = booking.TotalPrice,
                EndDate = booking.EndDate,
                StartDate = booking.StartDate,
                GuestId = booking.GuestId,
                SpecialRequests = booking.SpecialRequests,
                Status = booking.Status,
            });
        }

        public void AddRoom(Room room)
        {
            rooms.Add(new Room()
            {
                RoomNumber = room.RoomNumber,
                PricePerNight = room.PricePerNight,
                Capacity = room.Capacity,
                Type = room.Type,
                Description = room.Description,
            });
        }

        public void AddGuest(Guest guest)
        {
            guests.Add(new Guest()
            {
                Id = guest.Id,
                FirstName = guest.FirstName,
                MiddleName = guest.MiddleName,
                LastName = guest.LastName,
                PhoneNumber = guest.PhoneNumber,
                Email = guest.Email,
                DateOfBirth = guest.DateOfBirth
            });
        }
    }
}