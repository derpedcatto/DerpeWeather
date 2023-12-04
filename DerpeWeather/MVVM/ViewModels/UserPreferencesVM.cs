using CommunityToolkit.Mvvm.ComponentModel;
using DerpeWeather.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerpeWeather.MVVM.ViewModels
{
    public partial class UserPreferencesVM : ObservableObject
    {
        private readonly IUserRepo _userRepo;


        [ObservableProperty]
        private string _Username;
        [ObservableProperty]
        private string _AvatarPath;



        public UserPreferencesVM(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }



    }
}
