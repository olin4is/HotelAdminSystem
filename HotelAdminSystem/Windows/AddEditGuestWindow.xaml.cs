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

namespace HotelAdminSystem
{
    /// <summary>
    /// Логика взаимодействия для AddEditGuestWindow.xaml
    /// </summary>
    public partial class AddEditGuestWindow : Window
    {
        private IHotelService hotelService;
        private Guest guestToEdit;
        private bool isEditMode;

        public AddEditGuestWindow(IHotelService hotelService)
        {
            InitializeComponent();
            this.hotelService = hotelService;
            isEditMode = false;
            Title = "Добавление гостя";
            BirthDatePicker.SelectedDate = DateTime.Today.AddYears(-30);
        }

        public AddEditGuestWindow(IHotelService hotelService, Guest guest)
        {
            InitializeComponent();
            this.hotelService = hotelService;
            guestToEdit = guest;
            isEditMode = true;
            Title = "Редактирование гостя";
            LoadGuestData();
        }

        private void LoadGuestData()
        {
            if (guestToEdit != null)
            {
                FirstNameTextBox.Text = guestToEdit.FirstName;
                LastNameTextBox.Text = guestToEdit.LastName;
                MiddleNameTextBox.Text = guestToEdit.MiddleName; 
                PhoneTextBox.Text = guestToEdit.PhoneNumber;
                EmailTextBox.Text = guestToEdit.Email;
                PassportTextBox.Text = guestToEdit.Passport;
                BirthDatePicker.SelectedDate = guestToEdit.DateOfBirth;
            }
        }

        private void ValidateForm(object sender, EventArgs e)
        {
            bool isValid = !string.IsNullOrWhiteSpace(FirstNameTextBox.Text) &&
                          !string.IsNullOrWhiteSpace(LastNameTextBox.Text) &&
                          !string.IsNullOrWhiteSpace(PhoneTextBox.Text) &&
                          !string.IsNullOrWhiteSpace(PassportTextBox.Text) &&
                          BirthDatePicker.SelectedDate != null;

            SaveButton.IsEnabled = isValid;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFormFields())
                return;

            bool success;
            if (isEditMode)
            {
                success = hotelService.UpdateGuest(
                    guestToEdit.Id,
                    FirstNameTextBox.Text,
                    LastNameTextBox.Text,
                    MiddleNameTextBox.Text,
                    PhoneTextBox.Text,
                    EmailTextBox.Text,
                    PassportTextBox.Text,
                    BirthDatePicker.SelectedDate.Value
                );
            }
            else
            {
                success = hotelService.AddGuest(
                    FirstNameTextBox.Text,
                    LastNameTextBox.Text,
                    MiddleNameTextBox.Text,
                    PhoneTextBox.Text,
                    EmailTextBox.Text,
                    PassportTextBox.Text,
                    BirthDatePicker.SelectedDate.Value
                );
            }

            if (success)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show(isEditMode ?
                    "Не удалось обновить данные гостя. Возможно паспортный номер уже используется." :
                    "Не удалось добавить гостя. Возможно паспортный номер уже используется.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateFormFields()
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
            {
                MessageBox.Show("Введите имя гостя");
                return false;
            }

            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text))
            {
                MessageBox.Show("Введите фамилию гостя");
                return false;
            }

            if (string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            {
                MessageBox.Show("Введите телефон гостя");
                return false;
            }

            if (string.IsNullOrWhiteSpace(PassportTextBox.Text))
            {
                MessageBox.Show("Введите номер паспорта");
                return false;
            }

            if (BirthDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату рождения");
                return false;
            }

            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
