using Prism.Commands;
using Prism.Mvvm;
using sample_project.Data_access;
using sample_project.Model;
using sample_project.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace sample_project.ViewModels
{
    public class signuppageViewModel : BindableBase
    {
        public DelegateCommand signupcommand {  get; set; }
        public CompositeCommand signupcommandbar { get; set; }

        public user_dataaccess user_Dataaccess = new user_dataaccess();
        public img_data_access image_dataaccess = new img_data_access();

        List<user> list_user = new List<user>();
        public string firstname { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string confirmpass { get; set; }
        public int user_id { get; set; }
        private readonly ISharedDataService _sharedDataService;


        private Visibility _pageVisibility;
        public Visibility PageVisibility 
        { 
            get { return _pageVisibility; }
            set { SetProperty(ref _pageVisibility, value); }
        }
        public signuppageViewModel(ISharedDataService sharedDataService)
        {   signupcommandbar = new CompositeCommand();
            signupcommandbar.RegisterCommand(new DelegateCommand(gotohome));
            user_Dataaccess.readdata_user(list_user);
            _sharedDataService = sharedDataService;
        }


        private void gotohome()
        {
            //check database
            bool con = false;
            for (int i = 0; i < list_user.Count; i++)
            {
                if (list_user[i].username == username)
                {
                    MessageBox.Show("Username is already taken by someone else.");
                    con = true;
                    break;
                }
            }
            if (!con)
            {
                if (username==null)
                {
                    MessageBox.Show("Username field is empty!", "error!", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else if (firstname == null)
                {
                    MessageBox.Show("firstname field is empty!", "error!", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else if (password == null)
                {
                    MessageBox.Show("password field is empty!", "error!", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else if (confirmpass == null)
                {
                    MessageBox.Show("confirm password field is empty!", "error!", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else if (email == null)
                {
                    MessageBox.Show("Email field is empty!", "error!", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    if (password == confirmpass)
                    {
                        user tmp_user=new user();
                        tmp_user.username = username;
                        tmp_user.password = password;
                        tmp_user.email = email;
                        tmp_user.name = firstname;
                        list_user.Add(tmp_user);
                        user_id=list_user.Count-1;
                        //add to database
                       user_Dataaccess.add_user(tmp_user);
                        List<image> images = new List<image>();
                        image_dataaccess.add_image(images,user_id);
                        MessageBox.Show("Your signup was successful.", "success!", MessageBoxButton.OK, MessageBoxImage.Information);

                        //goto homepage
                        _sharedDataService.list_user=list_user;
                        _sharedDataService.user_id=user_id;
                        homepage home_p = new homepage();
                        home_p.Show();
                        Application.Current.Windows[0]?.Close();
                    }
                    else
                    {
                        MessageBox.Show("confirm password and password is diffrent!!", "error!", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
            }
         
        }
     
    }
}
