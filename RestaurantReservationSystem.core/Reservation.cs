using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservationSystem.core
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public DateTime DateTime { get; set; }
        public Customer Customer { get; set; }
        public Table Table { get; set; }

    }
}
