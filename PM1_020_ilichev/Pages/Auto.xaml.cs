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
    /// Логика взаимодействия для Auto.xaml
    /// </summary>
    public partial class Auto : Page
    {
        public Auto()
        {
            InitializeComponent();
        }

        private int attemptCount = 0;
        private const int maxAttempts = 3;
        private const int lockoutDurationSeconds = 10;
        private DateTime lockoutEndTime = DateTime.MinValue;

        private void _login_TextChanged(object sender, TextChangedEventArgs e)
        {
            _loginPH.Visibility = string.IsNullOrEmpty(_login.Text) ? Visibility.Visible : Visibility.Hidden;
        }

        private void _password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _password_PH.Visibility = string.IsNullOrEmpty(_password.Password) ? Visibility.Visible : Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AuthUsr(_login.Text, _password.Password);


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


        private bool IsLockedOut()
        {
            return DateTime.Now < lockoutEndTime;
        }

        private void LockoutSystem()
        {

            lockoutEndTime = DateTime.Now.AddSeconds(lockoutDurationSeconds);
        }

        private bool AuthenticateUser(object authenticatedUser, Page nextPage, string name, string patronymic)
        {
            if (authenticatedUser != null)
            {
                RememberCredentials(_login.Text, _password.Password);
                NavigationService?.Navigate(nextPage);
                MessageBox.Show("Добрый" + " " + GetPeriodOfDay(DateTime.Now) + " " + name + " " + patronymic + " " + "время работы сейчас: " + DateTime.Now);

                return true;
            }

            return false;
        }

        private void RememberCredentials(string login, string password)
        {

            UserCredentials.RememberCredentials(login, password);
        }

        public bool AuthUsr(string login, string password)
        {
            


            if (string.IsNullOrEmpty(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль");
                return false;
            }


            using (var db = new ilichevEntities())
            {
                var moderator = db.Moderators.AsNoTracking().FirstOrDefault(u => u.ID.ToString() == login && u.Password == password);
                if (AuthenticateUser(moderator, new Moder(), "", ""))
                    return true;



                var organizer = db.Organizers.AsNoTracking().FirstOrDefault(u => u.ID.ToString() == login && u.Password == password);

                if (organizer == null)
                {
                    MessageBox.Show("Логин или пароль неверный");
                    return false;
                }

                if (AuthenticateUser(organizer, new Organiz(organizer.ID), organizer.Surname, organizer.Patronymic))
                    return true;

                var participant = db.Participants.AsNoTracking().FirstOrDefault(u => u.ID.ToString() == login && u.Password == password);
                if (AuthenticateUser(participant, new Partic(), "", ""))
                    return true;
            }

            // Если попытка входа неудачна, увеличиваем счетчик попыток
            attemptCount++;

            // Проверяем, достигнуто ли максимальное количество попыток, и блокируем при необходимости
            if (attemptCount >= maxAttempts)
            {
                // Блокируем систему на указанное время
                LockoutSystem();
                MessageBox.Show("Неверный логин или пароль. Пожалуйста, подождите несколько секунд и попробуйте снова.");
            }
            return true;
        }

        public static class UserCredentials
        {
            private static string rememberedLogin;
            private static string rememberedPassword;

            public static void RememberCredentials(string login, string password)
            {
                rememberedLogin = login;
                rememberedPassword = password;
            }

            public static bool TryGetRememberedCredentials(out string login, out string password)
            {
                login = rememberedLogin;
                password = rememberedPassword;
                return !string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password);
            }

        }

    }
}
