using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using sample_project.Views;
using System;
using System.Windows.Navigation;
using Prism.Navigation;
using sample_project.Data_access;
using System.Collections.Generic;
using sample_project.Model;

namespace sample_project.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private readonly IRegionManager _regionmanager;
        private readonly IContainerExtension _containerExtension;
       // private readonly INavigationService _navigationService;

        loginpage _loginpage = new loginpage();
        signuppage _signuppage = new signuppage();
        public DelegateCommand gotosignup  { get; set; }
        public DelegateCommand gotologin { get; set; }


        public MainWindowViewModel(IRegionManager regionmanager,IContainerExtension containerExtension)
        {
            _regionmanager=regionmanager;
            _containerExtension=containerExtension;
            gotosignup = new DelegateCommand(goto_signup);
            gotologin = new DelegateCommand(goto_login); 
            _loginpage = _containerExtension.Resolve<loginpage>();
            _signuppage = _containerExtension.Resolve<signuppage>();


        }

        private void goto_login()
        {
            var regionleft = _regionmanager.Regions["ContentRegion"];
            regionleft.RemoveAll();
            regionleft.Add(_loginpage);
        }

        private void goto_signup()
        {
            var regionleft = _regionmanager.Regions["ContentRegion"];
            regionleft.RemoveAll();
            regionleft.Add(_signuppage);

        }
    }
}
