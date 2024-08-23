using System;
using System.Collections.Generic;
using System.Linq;
using RestaurantReservationSystem.core;

namespace RestaurantReservationSystem.console
{

    class Program
    {
        static void Main()
        {
            var restaurant = new Restaurant
            {
                Name = "SunShine"
            };

            bool running = true;

            while (running)
            {
                Console.WriteLine("\nRestaurant Management System");
                Console.WriteLine("1. Add Table");
                Console.WriteLine("2. Add Customer");
                Console.WriteLine("3. Make Reservation");
                Console.WriteLine("4. Update Reservation");
                Console.WriteLine("5. Cancel Reservation");
                Console.WriteLine("6. List Customers");
                Console.WriteLine("7. List Tables");
                Console.WriteLine("8. Show Number of Reservations");
                Console.WriteLine("9. Search Reservations");
                Console.WriteLine("10. Show Reservations");
                Console.WriteLine("10. Exit");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTable(restaurant);
                        break;
                    case "2":
                        AddCustomer(restaurant);
                        break;
                    case "3":
                        MakeReservation(restaurant);
                        break;
                    case "4":
                        UpdateReservation(restaurant);
                        break;
                    case "5":
                        CancelReservation(restaurant);
                        break;
                    case "6":
                        ListCustomers(restaurant);
                        break;
                    case "7":
                        ListBookedTables(restaurant);
                        break;
                    case "8":
                        ShowNumberOfReservations(restaurant);
                        break;
                    case "9":
                        SearchReservations(restaurant);
                        break;
                    case "10":
                        ListReservations(restaurant);
                        break;
                    case "11":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 10.");
                        break;
                }
            }
        }

        static void AddTable(Restaurant restaurant)
        {
            if (restaurant.Customers.Count == 0)
            {
                Console.WriteLine("Customer details should be filled first before adding a table.");
            }
            Console.WriteLine("Adding a new table");

            Console.Write("Enter Table ID: ");
            int tableId = int.Parse(Console.ReadLine());

            Console.Write("Enter Capacity: ");
            int capacity = int.Parse(Console.ReadLine());

            Console.Write("Enter Cost: ");
            int cost = int.Parse(Console.ReadLine());

            Console.Write("Is the table reserved? (yes/no): ");
            bool isReserved = Console.ReadLine().Trim().ToLower() == "yes";
            if (isReserved)
            {
                Console.WriteLine("Table is reserved. Exit the program.");
                Environment.Exit(0);
            }

            Console.Write("Enter table type (vip/standard): ");
            string tableType = Console.ReadLine().Trim().ToLower();

            Table table;

            if (tableType == "vip")
            {
                Console.Write("Enter Special Service: ");
                string specialService = Console.ReadLine();
                table = new VipTable
                {
                    TableId = tableId,
                    Capacity = capacity,
                    Cost = cost,
                    IsReserved = isReserved,
                    SpecialService = specialService
                };
            }
            else
            {
                Console.Write("Is the table near a window? (yes/no): ");
                bool nearWindow = Console.ReadLine().Trim().ToLower() == "yes";
                if (nearWindow)
                {
                    Console.WriteLine("Table near a window is already there. Please choose another table.");

                }
                table = new StandardTable
                {
                    TableId = tableId,
                    Capacity = capacity,
                    Cost = cost,
                    IsReserved = isReserved,
                    NearWindow = nearWindow
                };
            }

            restaurant.AddTable(table);
            Console.WriteLine("Table added successfully.");
        }

        static void AddCustomer(Restaurant restaurant)
        {
            Console.WriteLine("Adding a new customer");

            Console.Write("Enter Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Phone Number: ");
            string phoneNumber = Console.ReadLine();

            Console.Write("Enter Age: ");
            int age = int.Parse(Console.ReadLine());

            Console.Write("Enter Gender: ");
            string gender = Console.ReadLine();

            var customer = new Customer
            {
                CustomerId = customerId,
                Name = name,
                PhoneNumber = phoneNumber,
                Age = age,
                Gender = gender
            };

            restaurant.AddCustomer(customer);
            Console.WriteLine("Customer added successfully.");
        }

        static void MakeReservation(Restaurant restaurant)
        {
            Console.WriteLine("Making a new reservation");

            Console.Write("Enter Reservation ID: ");
            int reservationId = int.Parse(Console.ReadLine());

            Console.Write("Enter Date and Time (yyyy-mm-dd hh:mm:ss): ");
            DateTime datetime = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());
            var customer = restaurant.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }

            Console.Write("Enter Table ID: ");
            int tableId = int.Parse(Console.ReadLine());
            var table = restaurant.Tables.FirstOrDefault(t => t.TableId == tableId);
            if (table == null)
            {
                Console.WriteLine("Table not found.");
                return;
            }
            else
            {
                table.IsReserved = true;
            }

            try
            {
                var reservation = new Reservation
                {
                    ReservationId = reservationId,
                    DateTime = datetime,
                    Customer = customer,
                    Table = table
                };

                restaurant.AddReservation(reservation);
                Console.WriteLine("Reservation made successfully.");
            }
            catch (Exception ex) when (ex is DoubleBookingException || ex is InvalidReservationException || ex is OverBookingException)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static void UpdateReservation(Restaurant restaurant)
        {
            Console.WriteLine("Updating a reservation");

            Console.Write("Enter Reservation ID to update: ");
            int reservationId = int.Parse(Console.ReadLine());

            Console.Write("Enter new Date and Time (yyyy-mm-dd hh:mm:ss): ");
            DateTime datetime = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter new Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());
            var customer = restaurant.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }

            Console.Write("Enter new Table ID: ");
            int tableId = int.Parse(Console.ReadLine());
            var table = restaurant.Tables.FirstOrDefault(t => t.TableId == tableId);
            if (table == null)
            {
                Console.WriteLine("Table not found.");
                return;
            }

            var updatedReservation = new Reservation
            {
                ReservationId = reservationId,
                DateTime = datetime,
                Customer = customer,
                Table = table
            };

            try
            {
                restaurant.UpdateReservation(reservationId, updatedReservation);
                Console.WriteLine("Reservation updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating reservation: " + ex.Message);
            }
        }

        static void CancelReservation(Restaurant restaurant)
        {
            Console.WriteLine("Canceling a reservation");

            Console.Write("Enter Reservation ID to cancel: ");
            int reservationId = int.Parse(Console.ReadLine());

            try
            {
                restaurant.CancelReservation(reservationId);
                Console.WriteLine("Reservation canceled successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error canceling reservation: " + ex.Message);
            }
        }

        static void ListCustomers(Restaurant restaurant)
        {
            Console.WriteLine("Listing all customers");

            if (restaurant.Customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
            }
            else
            {
                foreach (var customer in restaurant.Customers)
                {
                    Console.WriteLine($"ID: {customer.CustomerId}, Name: {customer.Name}, Phone: {customer.PhoneNumber}, Age: {customer.Age}, Gender: {customer.Gender}");
                }
            }
        }

        static void ListBookedTables(Restaurant restaurant)
        {
            Console.WriteLine("Listing all booked tables");

            var bookedTables = restaurant.Tables.Where(t => t.IsReserved).ToList();

            if (bookedTables.Count == 0)
            {
                Console.WriteLine("No tables are currently booked.");
            }
            else
            {
                foreach (var table in bookedTables)
                {
                    Console.WriteLine($"Table ID: {table.TableId}, Capacity: {table.Capacity}, Cost: {table.Cost}");
                }
            }
        }

        static void ShowNumberOfReservations(Restaurant restaurant)
        {
            Console.WriteLine($"Total number of reservations: {restaurant.Reservations.Count}");
        }

        static void SearchReservations(Restaurant restaurant)
        {
            Console.WriteLine("Searching reservations");

            Console.Write("Enter Date to search (yyyy-mm-dd): ");
            DateTime searchDate = DateTime.Parse(Console.ReadLine());

            var reservations = restaurant.SearchReservations(r => r.DateTime.Date == searchDate.Date);
            Console.WriteLine($"Reservations on {searchDate.ToShortDateString()}:");
            foreach (var reservation in reservations)
            {
                Console.WriteLine($"Reservation ID: {reservation.ReservationId}, Customer: {reservation.Customer.Name}, Table ID: {reservation.Table.TableId}, DateTime: {reservation.DateTime}");
            }
        }

        static void ListReservations(Restaurant restaurant)
        {
            if (restaurant.Reservations.Count == 0)
            {
                Console.WriteLine("No reservations...!");
            }
            else
            {
                Console.WriteLine("Listing all reservations:");
                foreach (var reservation in restaurant.Reservations)
                {
                    Console.WriteLine($"Reservation ID: {reservation.ReservationId}, Date and Time: {reservation.DateTime}, Customer ID: {reservation.Customer.CustomerId}, Table ID: {reservation.Table.TableId}, Is Reserved: {reservation.Table.IsReserved}");
                }
            }
        }
    }

}
