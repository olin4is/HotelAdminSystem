using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HotelAdminSystem.Models
{
    public class Guest : Person
    {
        public string Passport { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FullName => GetFullName();

        protected override string GetFullName()
        {
            return base.GetFullName();
        }

        public override bool Equals(object obj)
        {
            if (obj is Guest guest)
            {
                return FirstName == guest.FirstName;
            }
            
            return false;
        }
    }
}
