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
using HotelAdminSystem.Models;

namespace HotelAdminSystem
{
    /// <summary>
    /// Логика взаимодействия для GuestSelectionWindow.xaml
    /// </summary>
    public partial class GuestSelectionWindow : Window
    {
        public Guest SelectedGuest { get; private set; }

        public GuestSelectionWindow(List<Guest> guests)
        {
            InitializeComponent();
            GuestsGrid.ItemsSource = guests;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedGuest = GuestsGrid.SelectedItem as Guest;
            if (SelectedGuest != null)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Выберите гостя");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
