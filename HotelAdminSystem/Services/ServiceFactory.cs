using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelAdminSystem.Interfaces;

namespace HotelAdminSystem.Services
{
    public static class ServiceFactory
    {
        public static IHotelService CreateHotelService()
        {
            return new HotelService();
        }

        public static IGuestService CreateGuestService()
        {
            return new HotelService();
        }

        public static IBookingService CreateBookingService()
        {
            return new HotelService();
        }

        public static IRoomService CreateRoomService()
        {
            return new HotelService();
        }
    }
}
