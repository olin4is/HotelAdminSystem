using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelAdminSystem.Models;

namespace HotelAdminSystem.Interfaces
{
    public interface IGuestService
    {
        bool AddGuest(string firstName, string middleName, string lastName, string phoneNumber, string email,
                 string passportNumber, DateTime dateOfBirth);
        bool UpdateGuest(int id, string firstName, string middleName, string lastName, string phoneNumber, string email,
                        string passportNumber, DateTime dateOfBirth);
        bool DeleteGuest(int id);
        Guest GetGuestById(int id);
        List<Guest> GetAllGuests();
    }
}
