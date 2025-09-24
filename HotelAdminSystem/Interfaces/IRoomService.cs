using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelAdminSystem.Models;

namespace HotelAdminSystem.Interfaces
{
    public interface IRoomService
    {
        List<Room> GetAllRooms();
        Room GetRoomByNumber(int roomNumber);
        List<Room> GetAvailableRooms(DateTime startDate, DateTime endDate);
        bool IsRoomAvailable(int roomNumber, DateTime startDate, DateTime endDate);
    }
}
