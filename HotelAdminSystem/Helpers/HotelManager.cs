using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelAdminSystem.Enums;

namespace HotelAdminSystem.Models
{
    public class HotelManager
    {
        public List<Room> Rooms { get; set; }
        public List<Booking> Bookings { get; set; }
        public List<Guest> Guests { get; set; }

        public HotelManager()
        {
            Rooms = new List<Room>();
            Bookings = new List<Booking>();
            Guests = new List<Guest>();
            InitializeTestData();
        }

        private void InitializeTestData()
        {
            // Тестовые номера
            Rooms.AddRange(new[]
            {
            new Room { RoomNumber = 101, Type = RoomType.Single, PricePerNight = 2500, Capacity = 1 },
            new Room { RoomNumber = 102, Type = RoomType.Double, PricePerNight = 3500, Capacity = 2 },
            new Room { RoomNumber = 103, Type = RoomType.Double, PricePerNight = 3500, Capacity = 2 },
            new Room { RoomNumber = 104, Type = RoomType.Double, PricePerNight = 3500, Capacity = 2 },
            new Room { RoomNumber = 201, Type = RoomType.Suite, PricePerNight = 5000, Capacity = 3 },
            new Room { RoomNumber = 202, Type = RoomType.Suite, PricePerNight = 5000, Capacity = 3 },
            new Room { RoomNumber = 203, Type = RoomType.Suite, PricePerNight = 5000, Capacity = 3 },
            new Room { RoomNumber = 204, Type = RoomType.Deluxe, PricePerNight = 7500, Capacity = 4 },
            new Room { RoomNumber = 301, Type = RoomType.Presidential, PricePerNight = 15000, Capacity = 6 }
        });

            // Тестовые гости
            Guests.AddRange(new[]
            {
            new Guest { Id = 1, FirstName = "Иван", LastName = "Иванов", MiddleName = "Иванович", PhoneNumber = "+79161234567", Passport = "123456789" },
            new Guest { Id = 2, FirstName = "Мария", LastName = "Петрова", MiddleName = "Ивановна", PhoneNumber = "+79169876543", Passport = "987654321" },
            new Guest { Id = 3, FirstName = "Алексей", LastName = "Сидоров", MiddleName = "Иванович", PhoneNumber = "+79161112233", Passport = "456789123" }
        });

            // Тестовые бронирования
            Bookings.AddRange(new[]
            {
            new Booking { Id = 1, RoomNumber = 101, GuestId = 1, StartDate = DateTime.Today,
                         EndDate = DateTime.Today.AddDays(3), Status = BookingStatus.Pending,
                         BookingDate = DateTime.Now, TotalPrice = 7500 },
            new Booking { Id = 2, RoomNumber = 102, GuestId = 2, StartDate = DateTime.Today.AddDays(1),
                         EndDate = DateTime.Today.AddDays(5), Status = BookingStatus.Pending,
                         BookingDate = DateTime.Now, TotalPrice = 14000 }
        });
        }

        public bool AddBooking(int roomNumber, int guestId, DateTime startDate, DateTime endDate, string specialRequests = "")
        {
            // Проверяем доступность номера
            var room = Rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
            var guest = Guests.FirstOrDefault(g => g.Id == guestId);

            if (room == null || guest == null)
                return false;

            if (!room.IsAvailableForPeriod(startDate, endDate, Bookings))
                return false;

            // Создаем новое бронирование
            var booking = new Booking
            {
                Id = Bookings.Count > 0 ? Bookings.Max(b => b.Id) + 1 : 1,
                RoomNumber = roomNumber,
                GuestId = guestId,
                StartDate = startDate,
                EndDate = endDate,
                BookingDate = DateTime.Now,
                Status = BookingStatus.Confirmed,
                TotalPrice = room.PricePerNight * (endDate - startDate).Days,
                SpecialRequests = specialRequests
            };

            Bookings.Add(booking);
            return true;
        }

        public List<Room> GetAvailableRooms(DateTime startDate, DateTime endDate)
        {
            return Rooms.Where(room => room.IsAvailableForPeriod(startDate, endDate, Bookings)).ToList();
        }

        public bool AddGuest(string firstName, string lastName, string middleName, string phoneNumber, string email,
                            string passportNumber, DateTime dateOfBirth)
        {
            // Проверяем, нет ли уже гостя с таким паспортом
            if (Guests.Any(g => g.Passport == passportNumber))
            {
                return false;
            }

            var guest = new Guest
            {
                Id = Guests.Count > 0 ? Guests.Max(g => g.Id) + 1 : 1,
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName,
                PhoneNumber = phoneNumber,
                Email = email,
                Passport = passportNumber,
                DateOfBirth = dateOfBirth
            };

            Guests.Add(guest);
            return true;
        }

        public Guest GetGuestById(int id)
        {
            return Guests.FirstOrDefault(g => g.Id == id);
        }

        public bool UpdateGuest(int id, string firstName, string lastName, string middleName, string phoneNumber, string email,
                              string passportNumber, DateTime dateOfBirth)
        {
            var guest = GetGuestById(id);
            if (guest == null) return false;

            // Проверяем, нет ли другого гостя с таким паспортом
            if (Guests.Any(g => g.Id != id && g.Passport == passportNumber))
            {
                return false;
            }

            guest.FirstName = firstName;
            guest.LastName = lastName;
            guest.MiddleName = middleName;
            guest.PhoneNumber = phoneNumber;
            guest.Email = email;
            guest.Passport = passportNumber;
            guest.DateOfBirth = dateOfBirth;

            return true;
        }

        public bool DeleteGuest(int id)
        {
            var guest = GetGuestById(id);
            if (guest == null) return false;

            // Проверяем, нет ли активных бронирований у гостя
            if (Bookings.Any(b => b.GuestId == id && (b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.Pending)))
            {
                return false;
            }

            Guests.Remove(guest);
            return true;
        }
        public bool UpdateBookingStatus(int bookingId, BookingStatus newStatus)
        {
            var booking = Bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null)
                return false;

            // Проверка допустимости перехода статуса
            if (!IsStatusTransitionValid(booking.Status, newStatus))
                return false;

            booking.Status = newStatus;
            return true;
        }

        private bool IsStatusTransitionValid(BookingStatus currentStatus, BookingStatus newStatus)
        {
            // Логика допустимых переходов статусов
            switch (currentStatus)
            {
                case BookingStatus.Pending:
                    return newStatus == BookingStatus.Confirmed || newStatus == BookingStatus.Cancelled;

                case BookingStatus.Confirmed:
                    return newStatus == BookingStatus.Completed || newStatus == BookingStatus.Cancelled;

                case BookingStatus.Completed:
                case BookingStatus.Cancelled:
                    return false; // Завершенные и отмененные бронирования нельзя изменить

                default:
                    return false;
            }
        }

        public Booking GetBookingById(int id)
        {
            return Bookings.FirstOrDefault(b => b.Id == id);
        }

        public bool CanCheckIn(int bookingId)
        {
            var booking = GetBookingById(bookingId);
            if (booking == null) return false;

            return booking.Status == BookingStatus.Pending &&
                   booking.StartDate.Date <= DateTime.Today;
        }

        public bool CanCheckOut(int bookingId)
        {
            var booking = GetBookingById(bookingId);
            if (booking == null) return false;

            return booking.Status == BookingStatus.Confirmed &&
                   DateTime.Today >= booking.StartDate.Date;
        }

        public bool CanCancel(int bookingId)
        {
            var booking = GetBookingById(bookingId);
            if (booking == null) return false;

            return booking.Status == BookingStatus.Pending;
        }
    }
}
