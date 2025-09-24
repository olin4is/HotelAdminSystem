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
using System.Windows.Navigation;
using System.Windows.Shapes;
using HotelAdminSystem.Enums;
using HotelAdminSystem.Interfaces;
using HotelAdminSystem.Models;
using HotelAdminSystem.Services;

namespace HotelAdminSystem
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IHotelService hotelService;
        private Room selectedRoomForBooking;
        private Guest selectedGuest;
        private dynamic selectedBooking;

        public MainWindow()
        {
            InitializeComponent();
            hotelService = ServiceFactory.CreateHotelService();
            LoadData();
        }

        public MainWindow(IHotelService service) // Конструктор для dependency injection
        {
            InitializeComponent();
            hotelService = service;
            LoadData();
        }

        private void LoadData()
        {
            RoomsGrid.ItemsSource = hotelService.GetAllRooms();
            RefreshGuestsGrid();
            RefreshBookingsGrid();
        }

        private void RefreshGuestsGrid()
        {
            GuestsGrid.ItemsSource = hotelService.GetAllGuests();
        }

        private void RefreshBookingsGrid()
        {
            var bookingView = hotelService.GetAllBookings().Select(b => new
            {
                b.Id,
                b.RoomNumber,
                GuestName = hotelService.GetGuestById(b.GuestId)?.FullName,
                b.StartDate,
                b.EndDate,
                b.Status,
                b.TotalPrice,
                b.BookingDate,
                b.StatusDisplay
            }).ToList();

            BookingsGrid.ItemsSource = bookingView;
            UpdateBookingButtonsState();
        }

        private void BookingsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedBooking = BookingsGrid.SelectedItem as dynamic;
            UpdateBookingButtonsState();
        }

        private void UpdateBookingButtonsState()
        {
            if (selectedBooking == null)
            {
                CheckInButton.IsEnabled = false;
                CheckOutButton.IsEnabled = false;
                CancelBookingButton.IsEnabled = false;
                return;
            }

            int bookingId = selectedBooking.Id;
            CheckInButton.IsEnabled = hotelService.CanCheckIn(bookingId);
            CheckOutButton.IsEnabled = hotelService.CanCheckOut(bookingId);
            CancelBookingButton.IsEnabled = hotelService.CanCancel(bookingId);
        }

        private void SearchAvailableRooms_Click(object sender, RoutedEventArgs e)
        {
            if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите даты для поиска");
                return;
            }

            DateTime startDate = StartDatePicker.SelectedDate.Value;
            DateTime endDate = EndDatePicker.SelectedDate.Value;

            if (startDate >= endDate)
            {
                MessageBox.Show("Дата начала должна быть раньше даты окончания");
                return;
            }

            var availableRooms = hotelService.GetAvailableRooms(startDate, endDate);
            AvailableRoomsGrid.ItemsSource = availableRooms;
        }

        private void AvailableRoomsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedRoomForBooking = AvailableRoomsGrid.SelectedItem as Room;
            BookButton.IsEnabled = selectedRoomForBooking != null;
        }

        private void GuestsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedGuest = GuestsGrid.SelectedItem as Guest;
            EditGuestButton.IsEnabled = selectedGuest != null;
            DeleteGuestButton.IsEnabled = selectedGuest != null;
        }

        private void BookSelectedRoom_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRoomForBooking == null || StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите номер и даты для бронирования");
                return;
            }

            // Открываем окно выбора гостя
            var guestSelectionWindow = new GuestSelectionWindow(hotelService.GetAllGuests());
            if (guestSelectionWindow.ShowDialog() == true)
            {
                var selectedGuest = guestSelectionWindow.SelectedGuest;
                DateTime startDate = StartDatePicker.SelectedDate.Value;
                DateTime endDate = EndDatePicker.SelectedDate.Value;

                bool success = hotelService.AddBooking(
                    selectedRoomForBooking.RoomNumber,
                    selectedGuest.Id,
                    startDate,
                    endDate
                );

                if (success)
                {
                    MessageBox.Show("Бронирование успешно добавлено!");
                    RefreshBookingsGrid();
                    // Обновляем список доступных номеров
                    SearchAvailableRooms_Click(null, null);
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении бронирования. Возможно номер уже занят.");
                }
            }
        }
        private void AddBooking_Click(object sender, RoutedEventArgs e)
        {
            // Открываем окно создания бронирования
            var addBookingWindow = new AddBookingWindow(hotelService);
            if (addBookingWindow.ShowDialog() == true)
            {
                RefreshBookingsGrid();
                MessageBox.Show("Бронирование успешно добавлено!");
            }
        }

        private void AddGuest_Click(object sender, RoutedEventArgs e)
        {
            // Открываем окно добавления гостя
            var addGuestWindow = new AddEditGuestWindow(hotelService);
            if (addGuestWindow.ShowDialog() == true)
            {
                RefreshGuestsGrid();
                MessageBox.Show("Гость успешно добавлен!");
            }
        }

        private void EditGuest_Click(object sender, RoutedEventArgs e)
        {
            if (selectedGuest == null)
            {
                MessageBox.Show("Выберите гостя для редактирования");
                return;
            }

            // Открываем окно редактирования гостя
            var editGuestWindow = new AddEditGuestWindow(hotelService, selectedGuest);
            if (editGuestWindow.ShowDialog() == true)
            {
                RefreshGuestsGrid();
                MessageBox.Show("Данные гостя успешно обновлены!");
            }
        }

        private void DeleteGuest_Click(object sender, RoutedEventArgs e)
        {
            if (selectedGuest == null)
            {
                MessageBox.Show("Выберите гостя для удаления");
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить гостя {selectedGuest.FullName}?",
                                        "Подтверждение удаления",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                bool success = hotelService.DeleteGuest(selectedGuest.Id);
                if (success)
                {
                    RefreshGuestsGrid();
                    MessageBox.Show("Гость успешно удален!");
                }
                else
                {
                    MessageBox.Show("Не удалось удалить гостя. Возможно у него есть активные бронирования.");
                }
            }
        }
        private void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBooking == null) return;

            var result = MessageBox.Show($"Отметить заезд по бронированию #{selectedBooking.Id}?",
                                        "Подтверждение заезда", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                bool success = hotelService.UpdateBookingStatus(selectedBooking.Id, BookingStatus.Confirmed);
                if (success)
                {
                    RefreshBookingsGrid();
                    MessageBox.Show("Заезд успешно отмечен!");
                }
                else
                {
                    MessageBox.Show("Не удалось отметить заезд.");
                }
            }
        }

        private void CheckOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBooking == null)
            {
                MessageBox.Show("Выберите бронирование для отметки выезда");
                return;
            }

            var result = MessageBox.Show($"Отметить выезд по бронированию #{selectedBooking.Id}?",
                                        "Подтверждение выезда",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                bool success = hotelService.UpdateBookingStatus(selectedBooking.Id, BookingStatus.Completed);
                if (success)
                {
                    RefreshBookingsGrid();
                    MessageBox.Show("Выезд успешно отмечен! Бронирование завершено.");
                }
                else
                {
                    MessageBox.Show("Не удалось отметить выезд. Возможно статус бронирования не позволяет это сделать.");
                }
            }
        }

        private void CancelBookingButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBooking == null)
            {
                MessageBox.Show("Выберите бронирование для отмены");
                return;
            }

            var result = MessageBox.Show($"Отменить бронирование #{selectedBooking.Id}?",
                                        "Подтверждение отмены",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                bool success = hotelService.UpdateBookingStatus(selectedBooking.Id, BookingStatus.Cancelled);
                if (success)
                {
                    RefreshBookingsGrid();
                    MessageBox.Show("Бронирование успешно отменено!");
                }
                else
                {
                    MessageBox.Show("Не удалось отменить бронирование. Возможно статус бронирования не позволяет это сделать.");
                }
            }
        }
    }
}
