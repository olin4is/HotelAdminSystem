using System.Linq;
using HotelAdminSystem.Models;
using HotelAdminSystem.Services;

namespace HotelAdminSystem.Tests.Services
{
    public class TestHotelService : HotelService
    {
        public void AddBooking(Booking booking)
        {
            bookings.Add(booking);
        }
    }
}