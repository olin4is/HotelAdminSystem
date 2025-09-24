using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HotelAdminSystem.Interfaces;
using HotelAdminSystem.Models;
using HotelAdminSystem.Services;

namespace HotelAdminSystem
{
    /// <summary>
    /// Логика взаимодействия для AddBookingWindow.xaml
    /// </summary>
    public partial class AddBookingWindow : Window
    {
        private IBookingService bookingService;
        private IGuestService guestService;
        private IRoomService roomService;

        public AddBookingWindow(IHotelService hotelService)
        {
            InitializeComponent();
            this.bookingService = hotelService;
            this.guestService = hotelService;
            this.roomService = hotelService;
            LoadData();
        }

        private void LoadData()
        {
            RoomComboBox.ItemsSource = roomService.GetAllRooms();
            GuestComboBox.ItemsSource = guestService.GetAllGuests();

            StartDatePicker.SelectedDate = DateTime.Today;
            EndDatePicker.SelectedDate = DateTime.Today.AddDays(1);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (RoomComboBox.SelectedItem == null || GuestComboBox.SelectedItem == null ||
                StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            var room = (Room)RoomComboBox.SelectedItem;
            var guest = (Guest)GuestComboBox.SelectedItem;
            DateTime startDate = StartDatePicker.SelectedDate.Value;
            DateTime endDate = EndDatePicker.SelectedDate.Value;

            if (startDate >= endDate)
            {
                MessageBox.Show("Дата начала должна быть раньше даты окончания");
                return;
            }

            bool success = bookingService.AddBooking(
                room.RoomNumber,
                guest.Id,
                startDate,
                endDate,
                SpecialRequestsTextBox.Text
            );

            if (success)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении бронирования. Возможно номер уже занят на выбранные даты.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
