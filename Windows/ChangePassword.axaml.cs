using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MOdule3.Context;
using MOdule3.Models;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace MOdule3;

public partial class ChangePassword : Window
{
    private User _user;
    private MyDbContext _dbContext;
    public ChangePassword(User user)
    {
        InitializeComponent();
        _user = user;
        _dbContext = new MyDbContext();
    }
    private async void ChangePasswordButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(OldPasswordTextBox.Text) || string.IsNullOrWhiteSpace(NewPasswordTextBox.Text) ||
            string.IsNullOrWhiteSpace(ConfirmPasswordTextBox.Text))
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Заполните все поля!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
            return;
        }
        if (!_user.Password.Equals(OldPasswordTextBox.Text))
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Старый пароль не верный!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
            return;
        }
        if (!NewPasswordTextBox.Text.Equals(ConfirmPasswordTextBox.Text))
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Пароли не совпадают!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
            return;
        }
        try
        {
            _user.Password = NewPasswordTextBox.Text;
            _dbContext.Users.Update(_user);
            await _dbContext.SaveChangesAsync();
            await MessageBoxManager.GetMessageBoxStandard("Успех", "Вы сменили пароль!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
            Close();
        }
        catch (Exception exception)
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", exception.Message, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
        }
    }
}