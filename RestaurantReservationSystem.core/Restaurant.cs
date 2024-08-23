using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservationSystem.core
{
    public class Restaurant
    {
        public string Name { get; set; }
        public List<Table> Tables { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Reservation> Reservations { get; set; }

        public static int NumberOfTables = 0;

        public Restaurant()
        {
            Tables = new List<Table>();
            Customers = new List<Customer>();
            Reservations = new List<Reservation>();
        }

        public void AddTable(Table table)
        {
            if (Customers.Count == 0)
            {
                throw new InvalidOperationException("Customer details should be filled first before adding a table.");
            }
            Tables.Add(table);
            NumberOfTables += 1;
        }

        public string RemoveTable(int tableId)
        {
            var table = Tables.Find(table => table.TableId == tableId);
            if (table == null)
            {
                return "Warning: Table does not exists!";
            }

            Tables.Remove(table);
            NumberOfTables -= 1;
            return "Success: Table removed successfully!";
        }

        public void DisplayTables()
        {
            if (Tables.Count == 0)
            {
                Console.WriteLine("0 tables.");
                return;
            }

            Console.WriteLine("\n====Display Tables====");
            foreach (Table table in Tables)
            {
                Console.WriteLine(table);
            }
            Console.WriteLine();
        }

        public void AddCustomer(Customer customer)
        {
            Customers.Add(customer);
        }
        public void AddReservation(Reservation reservation)
        {
            if (reservation.Customer == null || reservation.Table == null || reservation.DateTime == default)
            {
                throw new InvalidReservationException("Reservation details are incomplete.");
            }
            if (Reservations.Any(r => r.Table.TableId == reservation.Table.TableId && r.DateTime == reservation.DateTime))
            {
                throw new OverBookingException("This table is already booked for the specified time.");
            }
            if (Reservations.Any(r => r.Customer.CustomerId == reservation.Customer.CustomerId && r.DateTime == reservation.DateTime))
            {
                throw new DoubleBookingException("Customer already has a reservation at the specified time.");
            }

            Reservations.Add(reservation);
        }
        public void UpdateReservation(int reservationId, Reservation updatedReservation)
        {
            var reservation = Reservations.FirstOrDefault(r => r.ReservationId == reservationId);
            if (reservation != null)
            {
                reservation.Customer = updatedReservation.Customer;
                reservation.Table = updatedReservation.Table;
                reservation.DateTime = updatedReservation.DateTime;
            }
        }

        public void CancelReservation(int reservationId)
        {
            var reservation = Reservations.FirstOrDefault(r => r.ReservationId == reservationId);
            if (reservation != null)
            {
                Reservations.Remove(reservation);
            }
        }

        public List<Reservation> SearchReservations(Func<Reservation, bool> predicate)
        {
            return Reservations.Where(predicate).ToList();
        }

    }
}
