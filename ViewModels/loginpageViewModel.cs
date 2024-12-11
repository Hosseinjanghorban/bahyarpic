using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using sample_project.Data_access;
using sample_project.Model;
using sample_project.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Prism.Navigation;
namespace sample_project.ViewModels
{
    public class loginpageViewModel : BindableBase
    {
        public DelegateCommand login { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int user_id { get; set; }
        private readonly NavigationService _navigationService;

        public List<user> list_user { get; set; }

        private readonly ISharedDataService _sharedDataService;
        public user_dataaccess user_Dataaccess = new user_dataaccess();

        public loginpageViewModel(ISharedDataService sharedDataService)
        {
            login = new DelegateCommand(do_login);
            list_user = new List<user>();
            user_Dataaccess.readdata_user(list_user);
            _sharedDataService = sharedDataService;
        }

        private async void do_login()
        {
            //check database
            bool log = false;
            for(int i = 0; i < list_user.Count; i++)
            {
                if (list_user[i].username == username && list_user[i].password==password)
                {
                    //id=i
                    user_id = i;                  
                    MessageBox.Show($"welcome " + list_user[user_id].name, "welcome!", MessageBoxButton.OK, MessageBoxImage.Information);
                    log = true;
                    break;
                }
            }
            if (!log)
            {
                MessageBox.Show($"username or password is incorrect!!", "error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _sharedDataService.user_id = user_id;
            _sharedDataService.list_user = list_user;

            homepage home = new homepage();
            home.Show();
            Application.Current.Windows[0]?.Close();

        }

    }
}
