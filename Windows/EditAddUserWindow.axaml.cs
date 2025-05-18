using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MOdule3.Context;
using MOdule3.Models;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Tmds.DBus.Protocol;

namespace MOdule3;

public partial class EditAddUserWindow : Window
{
    private MyDbContext _dbContext;
    private User? _user;
    public EditAddUserWindow(User? user = null)
    {
        InitializeComponent();
        _dbContext = new MyDbContext();
        if (user != null)
        {
            _user = user;
            LoginTextBox.Text = user.Login;
            PasswordTextBox.Text = user.Password;
        }
    }

    private async void ConfirmButton_OnClick(object? sender, RoutedEventArgs e)
    {
        string message = "";
        if (string.IsNullOrWhiteSpace(LoginTextBox.Text) || string.IsNullOrWhiteSpace(PasswordTextBox.Text))
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Заполните все поля!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error)
                .ShowAsync();
            return;
        }
        try
        {            
            if (_user == null)
            {
                _user = new User
                {
                    Login = LoginTextBox.Text,
                    Password = PasswordTextBox.Text
                };
                _dbContext.Users.Add(_user);
                message = "Вы добавили пользователя!";
            }
            else
            {
                _user.Login = LoginTextBox.Text;
                _user.Password = PasswordTextBox.Text;
                _dbContext.Users.Update(_user);
                message = "Вы обновили пользователя!";
            }
            await _dbContext.SaveChangesAsync();
            await MessageBoxManager.GetMessageBoxStandard("Успех", message , ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error)
                .ShowAsync();
        }
        catch (Exception exception)
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", exception.Message, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error)
                .ShowAsync();
        }
    }
}