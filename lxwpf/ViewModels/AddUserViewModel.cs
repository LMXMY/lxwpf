using lxwpf.Entities;
using lxwpf.Services;
using Prism.Commands;
using Prism.Mvvm;
using lxwpf.Share;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Dialogs;

namespace lxwpf.ViewModels
{
    public class AddUserViewModel : BindableBase, IDialogAware
    {
        private readonly IUserService _userService;

        public AddUserViewModel(IUserService userService)
        {
            _userService = userService;
        }
        public string Title => "新增用户";
        //public event Action<IDialogResult> RequestClose;
        public DialogCloseListener RequestClose { get; }

        public List<UserModel>? Users { get; set; }

        private string? _errorMessage;

        public string? ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; RaisePropertyChanged(); }
        }

        private string? _userName;

        public string? UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged();
            }
        }

        private string? _password;

        public string? Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged();
            }
        }


        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Users") && parameters.GetValue<ObservableCollection<UserModel>>("Users") != null 
                //&& parameters.ContainsKey("Roles") && parameters.GetValue<ObservableCollection<RoleModel>>("Roles") != null
                )
            {
                Users = new List<UserModel>(parameters.GetValue<ObservableCollection<UserModel>>("Users")!);
                //RoleModels = new ObservableCollection<RoleModel>(parameters.GetValue<ObservableCollection<RoleModel>>("Roles")!);
            }
        }

        public ICommand CancelCommand
        {
            get => new DelegateCommand(() =>
            {
                //DialogResult dialogResult = new DialogResult(ButtonResult.Cancel);
                //RequestClose?.Invoke(dialogResult);
                RequestClose.Invoke(ButtonResult.OK);
            });
        }

        public ICommand SaveCommand
        {
            get => new DelegateCommand(() =>
            {
                if (string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Password is empty";
                    return;
                }

                UserModel? _user = new UserModel();
                _user!.UserName = UserName;
                _user!.State = 1;
                _user!.Password = Password;//Cryptography.Encrypt(Password ?? string.Empty);

                var userResult = _userService.AddUser(_user);

                //DialogResult dialogResult = new DialogResult(ButtonResult.OK);
                //RequestClose?.Invoke(dialogResult);
                RequestClose.Invoke(ButtonResult.OK);

            });
        }
    }
}
