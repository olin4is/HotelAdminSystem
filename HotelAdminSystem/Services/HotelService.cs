using System;
using System.Collections.Generic;
using System.Linq;
using HotelAdminSystem.Enums;
using HotelAdminSystem.Helpers;
using HotelAdminSystem.Interfaces;
using HotelAdminSystem.Models;

namespace HotelAdminSystem.Services
{
    public class HotelService : IHotelService
    {
        protected List<Room> rooms;
        protected List<Booking> bookings;
        protected List<Guest> guests;

        public List<Room> Rooms => rooms;
        public List<Booking> Bookings => bookings;
        public List<Guest> Guests => guests;

        public HotelService()
        {
            rooms = new List<Room>();
            bookings = new List<Booking>();
            guests = new List<Guest>();
            InitializeTestData();
        }

        private void InitializeTestData()
        {
            rooms = TestData.Rooms;
            guests = TestData.Guests;
            bookings = TestData.Bookings;
        }

        public bool AddBooking(int roomNumber, int guestId, DateTime startDate, DateTime endDate, string specialRequests = "")
        {
            // Проверяем доступность номера
            var room = GetRoomByNumber(roomNumber);
            var guest = GetGuestById(guestId);

            if (room == null || guest == null)
                return false;

            if (!IsRoomAvailable(roomNumber, startDate, endDate))
                return false;

            var booking = new Booking
            {
                Id = bookings.Count > 0 ? bookings.Max(b => b.Id) + 1 : 1,
                RoomNumber = roomNumber,
                GuestId = guestId,
                StartDate = startDate,
                EndDate = endDate,
                BookingDate = DateTime.Now,
                Status = BookingStatus.Pending,
                TotalPrice = room.PricePerNight * (endDate - startDate).Days,
                SpecialRequests = specialRequests
            };

            bookings.Add(booking);
            return true;
        }

        public List<Room> GetAvailableRooms(DateTime startDate, DateTime endDate)
        {
            return rooms.Where(room => IsRoomAvailable(room.RoomNumber, startDate, endDate)).ToList();
        }

        public bool AddGuest(string firstName, string lastName, string middleName, string phoneNumber, string email,
                            string passportNumber, DateTime dateOfBirth)
        {
            // Проверяем, нет ли уже гостя с таким паспортом
            if (guests.Any(g => g.Passport == passportNumber))
            {
                return false;
            }

            var guest = new Guest
            {
                Id = guests.Count > 0 ? guests.Max(g => g.Id) + 1 : 1,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Email = email,
                Passport = passportNumber,
                DateOfBirth = dateOfBirth
            };

            guests.Add(guest);
            return true;
        }

        public Guest GetGuestById(int id)
        {
            return guests.FirstOrDefault(g => g.Id == id);
        }

        public bool UpdateGuest(int id, string firstName, string lastName, string middleName, string phoneNumber, string email,
                              string passportNumber, DateTime dateOfBirth)
        {
            var guest = GetGuestById(id);
            if (guest == null) return false;

            // Проверяем, нет ли другого гостя с таким паспортом
            if (guests.Any(g => g.Id != id && g.Passport == passportNumber))
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
            if (bookings.Any(b => b.GuestId == id && (b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.Pending)))
            {
                return false;
            }

            guests.Remove(guest);
            return true;
        }
        public List<Guest> GetAllGuests()
        {
            return guests.ToList();
        }
        public List<Room> GetAllRooms()
        {
            return rooms.ToList();
        }
        public List<Booking> GetAllBookings()
        {
            return bookings.ToList();
        }
        public bool IsRoomAvailable(int roomNumber, DateTime startDate, DateTime endDate)
        {
            var room = GetRoomByNumber(roomNumber);
            if (room == null) return false;

            return !bookings.Any(b => b.RoomNumber == roomNumber &&
                                    b.Status == BookingStatus.Confirmed &&
                                    b.StartDate < endDate &&
                                    b.EndDate > startDate);
        }
        public Room GetRoomByNumber(int roomNumber)
        {
            return rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
        }
        public bool UpdateBookingStatus(int bookingId, BookingStatus newStatus)
        {
            var booking = GetBookingById(bookingId);
            if (booking == null)
                return false;

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
                    return newStatus == BookingStatus.Pending || newStatus == BookingStatus.Cancelled;

                case BookingStatus.Completed:
                case BookingStatus.Cancelled:
                    return false; // Завершенные и отмененные бронирования нельзя изменить

                default:
                    return false;
            }
        }

        public Booking GetBookingById(int id)
        {
            return bookings.FirstOrDefault(b => b.Id == id);
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

            return booking.Status == BookingStatus.Pending ||
                   booking.Status == BookingStatus.Confirmed;
        }
    }
}
