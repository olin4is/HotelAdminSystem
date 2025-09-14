using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAdminSystem.Enums
{
    public enum RoomType
    {
        [Description("Одноместный")]
        Single,
        [Description("Двухместный")]
        Double,
        [Description("Сюит")]
        Suite,
        [Description("Люкс")]
        Deluxe,
        [Description("Президентский")]
        Presidential
    }
}
