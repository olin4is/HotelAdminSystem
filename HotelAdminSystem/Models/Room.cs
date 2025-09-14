using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelAdminSystem.Enums;
using HotelAdminSystem.Helpers;

namespace HotelAdminSystem.Models
{
    public class Room
    {
        public int RoomNumber { get; set; }
        public RoomType Type { get; set; }
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }

        public string RoomTypeDisplay => Type.GetDescription();
        public bool IsAvailableForPeriod(DateTime startDate, DateTime endDate, List<Booking> bookings)
        {
            return !bookings.Any(b => b.RoomNumber == RoomNumber &&
                                    b.Status == BookingStatus.Confirmed &&
                                    b.StartDate < endDate &&
                                    b.EndDate > startDate);
        }
    }
}
