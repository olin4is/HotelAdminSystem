using HotelAdminSystem.Interfaces;
using HotelAdminSystem.Models;

namespace HotelAdminSystem.Tests.Interfaces
{
    public interface ITestHotelService : IHotelService
    {
        void AddBooking(Booking booking);
        void AddRoom(Room room);
        void AddGuest(Guest guest);
    }
}