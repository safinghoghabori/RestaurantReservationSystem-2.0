using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservationSystem.core
{
    public class OverBookingException : Exception
    {
        public OverBookingException(string message) : base(message) { }
    }
}
