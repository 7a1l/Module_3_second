using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MOdule3.Context;
using MOdule3.Models;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using static MsBox.Avalonia.MessageBoxManager;

namespace MOdule3;

public partial class AdminWindow : Window
{
    private MyDbContext _dbContext;
    public AdminWindow()
    {
        InitializeComponent();
        _dbContext = new MyDbContext();
        UsersListBox.ItemsSource = _dbContext.Users.ToList();
    }

    private async void EditUserButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (UsersListBox.SelectedItem is User user)
        {
            await new EditAddUserWindow(user).ShowDialog(this);
        }
    }

    private async void AddUserButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await new EditAddUserWindow().ShowDialog(this);
    }

    private async void UnblockUserButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (UsersListBox.SelectedItem is User user)
        {
            if (user.Isblocked)
            {
                try
                {
                    user.Isblocked = false;
                    _dbContext.Users.Update(user);
                    await _dbContext.SaveChangesAsync();
                    await GetMessageBoxStandard("Успех", "Вы успешно разблокировали пользователя", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success).ShowAsync();
                    UsersListBox.ItemsSource = _dbContext.Users.ToList();
                    return;
                }
                catch (Exception exception)
                {
                    await GetMessageBoxStandard("Ошибка", exception.Message, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
                }
            }
            await GetMessageBoxStandard("Ошибка", "Выберите заблокированного пользователя!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
        }
    }
}