using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System;
using ReactiveUI;
using ProjectDiary.ViewModels;

namespace ProjectDiary.Views;

public partial class NewTask : ReactiveWindow<NewTaskViewModel>
{
    public NewTask()
    {
        InitializeComponent();
        //Dialog Close, sends the task to the main window
        this.WhenActivated(action => action(ViewModel!.CreateTask.Subscribe(Close)));
    }
}