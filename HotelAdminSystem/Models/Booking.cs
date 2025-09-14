using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using HotelAdminSystem.Enums;
using HotelAdminSystem.Helpers;

namespace HotelAdminSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public int GuestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime BookingDate { get; set; }
        public BookingStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public string SpecialRequests { get; set; }

        public int DurationInDays => (EndDate - StartDate).Days;

        public string StatusDisplay => Status.GetDescription() ?? "Не указано";
    }
}
