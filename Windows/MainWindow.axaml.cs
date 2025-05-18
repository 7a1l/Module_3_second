using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using MOdule3.Context;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace MOdule3;

public partial class MainWindow : Window
{
    private MyDbContext _dbContext;
    public MainWindow()
    {
        InitializeComponent();
        _dbContext = new MyDbContext();
    }

    private async void AuthorizeButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text) || string.IsNullOrWhiteSpace(PasswordTextBox.Text))
            {
                await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Заполните обязательные поля!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
                return;
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.Login.Equals(LoginTextBox.Text));
            if (user == null)
            {
                await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введен неверный логин!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
                return;
            }

            if (!user.Password.Equals(PasswordTextBox.Text))
            {
                if (user.Totalattempts >= 3)
                {
                    await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Вы заблокированы. Абратитесь к админу!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
                    user.Isblocked = true;
                    await _dbContext.SaveChangesAsync();
                    return;
                }
                await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введен неверный пароль!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
                user.Totalattempts++;
                await _dbContext.SaveChangesAsync();
                return;
            }

            if (user.Lastlogin.HasValue && (DateTime.Now - user.Lastlogin.Value).Days >= 30)
            {
                await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введен неверный логин!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
                user.Isblocked = true;
                await _dbContext.SaveChangesAsync();
                return;
            }
            if (user.Isblocked)
            {
                await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Вы заблокированы. Абратитесь к админу", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
                return;
            }
            await MessageBoxManager.GetMessageBoxStandard("Успех", "Вы авторизовались!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success).ShowAsync();
            user.Lastlogin = DateTime.Now;
            user.Totalattempts = 0;
            await _dbContext.SaveChangesAsync();
            if (user.Isfirstlogin)
            {
                await new ChangePassword(user).ShowDialog(this);
                user.Isfirstlogin = false;
                await _dbContext.SaveChangesAsync();
                return;
            }
            if (user.Isadmin)
            {
                new AdminWindow().Show();
                Close();
            }
        }
        catch (Exception exception)
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", exception.Message, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
        }
    }
}