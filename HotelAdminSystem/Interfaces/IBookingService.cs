using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelAdminSystem.Enums;
using HotelAdminSystem.Models;

namespace HotelAdminSystem.Interfaces
{
    public interface IBookingService
    {
        bool AddBooking(int roomNumber, int guestId, DateTime startDate, DateTime endDate, string specialRequests = "");
        bool UpdateBookingStatus(int bookingId, BookingStatus newStatus);
        Booking GetBookingById(int id);
        List<Booking> GetAllBookings();
        bool CanCheckIn(int bookingId);
        bool CanCheckOut(int bookingId);
        bool CanCancel(int bookingId);
    }
}
