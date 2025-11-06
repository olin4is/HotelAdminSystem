using System;
using System.Linq;
using HotelAdminSystem.Enums;
using HotelAdminSystem.Interfaces;
using HotelAdminSystem.Models;
using HotelAdminSystem.Services;
using HotelAdminSystem.Tests.Interfaces;
using HotelAdminSystem.Tests.Services;
using NUnit.Framework;

namespace HotelAdminSystem.Tests
{
    [TestFixture]
    public class BookingServiceTests
    {
        private ITestHotelService _servive;

        [SetUp]
        public void SetUp()
        {
            _servive = new TestHotelService();
        }

        [TestCase(101, 1, "2025-11-02", "2025-11-05")]
        [TestCase(102, 2, "2025-04-02", "2025-06-05", "второе полотенце")]
        [TestCase(103, 3, "2024-11-25", "2025-01-01")]
        public void AddValid_Booking(int roomNumber, int guestId, string startDate, string endDate, string specialRequests = "")
        {
            var dateStart = DateTime.Parse(startDate);
            var dateEnd = DateTime.Parse(endDate);
            
            var result = _servive.AddBooking(roomNumber, guestId, dateStart, dateEnd);
            Assert.IsTrue(result);
        }

        [TestCase(333, 1, "2025-11-02", "2025-11-05")]
        [TestCase(101, -2, "2025-11-02", "2025-11-05")]
        [TestCase(101, 3, "2025-11-02", "2025-10-05")]
        public void AddInvalid_Booking(int roomNumber, int guestId, string startDate, string endDate, string specialRequests = "")
        {
            var dateStart = DateTime.Parse(startDate);
            var dateEnd = DateTime.Parse(endDate);
            
            var result = _servive.AddBooking(112, 1, DateTime.Now, DateTime.Now.AddDays(3));
            Assert.IsFalse(result);
        }
        
        [Test]
        public void UpdateBookingStatus_Pending()
        {
            var booking = _servive.GetBookingById(1);
            _servive.UpdateBookingStatus(booking.Id, BookingStatus.Pending);
        }
        
        [Test]
        public void UpdateBookingStatus_Confirmed()
        {
            var booking = _servive.GetBookingById(1);
            _servive.UpdateBookingStatus(booking.Id, BookingStatus.Confirmed);
        }
        
        [Test]
        public void UpdateBookingStatus_Cancelled()
        {
            var booking = _servive.GetBookingById(1);
            _servive.UpdateBookingStatus(booking.Id, BookingStatus.Cancelled);
        }
        
        [Test]
        public void UpdateBookingStatus_Completed()
        {
            var booking = _servive.GetBookingById(1);
            _servive.UpdateBookingStatus(booking.Id, BookingStatus.Completed);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void GetValidBooking(int id)
        {
            var booking = _servive.GetBookingById(id);
            Assert.IsTrue(booking != null);
        }

        [Test]
        public void GetAllBookings()
        {
            var bookings = _servive.GetAllBookings();
            Assert.IsTrue(bookings.Any());
        }

        [Test]
        public void CheckInIfPending()
        {
            var booking = new Booking
            {
                Id = 11, RoomNumber = 101, GuestId = 1, StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3), Status = BookingStatus.Pending,
                BookingDate = DateTime.Now, TotalPrice = 7500
            };
            _servive.AddBooking(booking);
            var result = _servive.CanCheckIn(booking.Id);
            
            Assert.AreEqual(result, false);
        }
        
        [Test]
        public void CheckInIfConfirmed()
        {
            var booking = new Booking
            {
                Id = 11, RoomNumber = 101, GuestId = 1, StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3), Status = BookingStatus.Confirmed,
                BookingDate = DateTime.Now, TotalPrice = 7500
            };
            _servive.AddBooking(booking);
            var result = _servive.CanCheckIn(booking.Id);
            
            Assert.AreEqual(result, true);
        }
        
        [Test]
        public void CheckInIfCancelled()
        {
            var booking = new Booking
            {
                Id = 11, RoomNumber = 101, GuestId = 1, StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3), Status = BookingStatus.Cancelled,
                BookingDate = DateTime.Now, TotalPrice = 7500
            };
            _servive.AddBooking(booking);
            var result = _servive.CanCheckIn(booking.Id);
            
            Assert.AreEqual(result, false);
        }
        
        [Test]
        public void CheckInIfCompleted()
        {
            var booking = new Booking
            {
                Id = 11, RoomNumber = 101, GuestId = 1, StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3), Status = BookingStatus.Completed,
                BookingDate = DateTime.Now, TotalPrice = 7500
            };
            _servive.AddBooking(booking);
            var result = _servive.CanCheckIn(booking.Id);
            
            Assert.AreEqual(result, false);
        }

        [Test]
        public void CheckInIfIncorrectDate()
        {
            var booking = new Booking
            {
                Id = 11, RoomNumber = 101, GuestId = 1, StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3), Status = BookingStatus.Confirmed,
                BookingDate = DateTime.Now, TotalPrice = 7500
            };
            _servive.AddBooking(booking);
            var result = _servive.CanCheckIn(booking.Id);
            
            Assert.AreEqual(result, false);
        }
        
        [Test]
        public void CheckOutIfPending()
        {
            var booking = new Booking
            {
                Id = 11, RoomNumber = 101, GuestId = 1, StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3), Status = BookingStatus.Pending,
                BookingDate = DateTime.Now, TotalPrice = 7500
            };
            _servive.AddBooking(booking);
            var result = _servive.CanCheckOut(booking.Id);
            
            Assert.AreEqual(result, false);
        }
        
        [Test]
        public void CheckOutIfConfirmed()
        {
            var booking = new Booking
            {
                Id = 11, RoomNumber = 101, GuestId = 1, StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3), Status = BookingStatus.Confirmed,
                BookingDate = DateTime.Now, TotalPrice = 7500
            };
            _servive.AddBooking(booking);
            var result = _servive.CanCheckOut(booking.Id);
            
            Assert.AreEqual(result, true);
        }
        
        [Test]
        public void CheckOutIfCancelled()
        {
            var booking = new Booking
            {
                Id = 11, RoomNumber = 101, GuestId = 1, StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3), Status = BookingStatus.Cancelled,
                BookingDate = DateTime.Now, TotalPrice = 7500
            };
            _servive.AddBooking(booking);
            var result = _servive.CanCheckOut(booking.Id);
            
            Assert.AreEqual(result, false);
        }
        
        [Test]
        public void CheckOutIfCompleted()
        {
            var booking = new Booking
            {
                Id = 11, RoomNumber = 101, GuestId = 1, StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3), Status = BookingStatus.Completed,
                BookingDate = DateTime.Now, TotalPrice = 7500
            };
            _servive.AddBooking(booking);
            var result = _servive.CanCheckOut(booking.Id);
            
            Assert.AreEqual(result, false);
        }

        [Test]
        public void CheckOutIfIncorrectDate()
        {
            var booking = new Booking
            {
                Id = 11, RoomNumber = 101, GuestId = 1, StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3), Status = BookingStatus.Confirmed,
                BookingDate = DateTime.Now, TotalPrice = 7500
            };
            _servive.AddBooking(booking);
            var result = _servive.CanCheckOut(booking.Id);
            
            Assert.AreEqual(result, false);
        }
        
        [Test]
        public void CancelIfPending()
        {
            var booking = new Booking
            {
                Id = 11, RoomNumber = 101, GuestId = 1, StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3), Status = BookingStatus.Pending,
                BookingDate = DateTime.Now, TotalPrice = 7500
            };
            _servive.AddBooking(booking);
            var result = _servive.CanCancel(booking.Id);
            
            Assert.AreEqual(result, true);
        }
        
        [Test]
        public void CancelIfConfirmed()
        {
            var booking = new Booking
            {
                Id = 11, RoomNumber = 101, GuestId = 1, StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3), Status = BookingStatus.Confirmed,
                BookingDate = DateTime.Now, TotalPrice = 7500
            };
            _servive.AddBooking(booking);
            var result = _servive.CanCancel(booking.Id);
            
            Assert.AreEqual(result, true);
        }
        
        [Test]
        public void CancelIfCancelled()
        {
            var booking = new Booking
            {
                Id = 11, RoomNumber = 101, GuestId = 1, StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3), Status = BookingStatus.Cancelled,
                BookingDate = DateTime.Now, TotalPrice = 7500
            };
            _servive.AddBooking(booking);
            var result = _servive.CanCancel(booking.Id);
            
            Assert.AreEqual(result, false);
        }
        
        [Test]
        public void CancelIfCompleted()
        {
            var booking = new Booking
            {
                Id = 11, RoomNumber = 101, GuestId = 1, StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3), Status = BookingStatus.Completed,
                BookingDate = DateTime.Now, TotalPrice = 7500
            };
            _servive.AddBooking(booking);
            var result = _servive.CanCancel(booking.Id);
            
            Assert.AreEqual(result, false);
        }
    }
}