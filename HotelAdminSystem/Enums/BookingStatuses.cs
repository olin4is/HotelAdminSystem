using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAdminSystem.Enums
{
    public enum BookingStatus
    {
        [Description("Оформлен")]
        Pending,
        [Description("Заселен")]
        Confirmed,
        [Description("Отменен")]
        Cancelled,
        [Description("Выселен")]
        Completed
    }
}
