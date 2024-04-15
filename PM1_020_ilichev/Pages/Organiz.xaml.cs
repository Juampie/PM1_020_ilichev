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

namespace PM1_020_ilichev.Pages
{
    /// <summary>
    /// Логика взаимодействия для Organiz.xaml
    /// </summary>
    public partial class Organiz : Page
    {
        private int loggedInUserId;
        public Organiz(int loggedInUserId)
        {
            InitializeComponent();
            this.loggedInUserId = loggedInUserId;

            LoadUserData();
        }


        private void LoadUserData()
        {
            try
            {
                using (var db = ilichevEntities.GetContext())
                {
                    // Получаем пользователя по ID
                    var user = db.Organizers.FirstOrDefault(u => u.ID == loggedInUserId);



                    if (user != null)

                    {

                       


                    }
                    else
                    {
                        MessageBox.Show("Пользователь не найден.");
                    }
                    var gender = user.Gender;

                    switch (gender)
                    {
                        case "м":
                            gender = "Ms";
                            break;
                        case "ж":
                            gender = "Mrs";
                            break;

                    }


                    string userDataString = $"Добрый {GetPeriodOfDay(DateTime.Now)} {gender}, {user.Surname}";

                    nameTxt.Text = userDataString;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }


        public static string GetPeriodOfDay(DateTime time)
        {
            if (time.TimeOfDay >= new TimeSpan(9, 0, 0) && time.TimeOfDay <= new TimeSpan(11, 0, 0))
            {
                return "Утро";
            }
            else if (time.TimeOfDay > new TimeSpan(11, 0, 0) && time.TimeOfDay <= new TimeSpan(18, 0, 0))
            {
                return "День";
            }
            else if (time.TimeOfDay > new TimeSpan(18, 0, 0) && time.TimeOfDay <= new TimeSpan(23, 59, 59))
            {
                return "Вечер";
            }
            else
            {
                return "Ночь";
            }
        }

        private void _profilButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrganizProfil());
        }
    }
}
