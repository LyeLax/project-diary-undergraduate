using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using ReactiveUI;
using ProjectDiary.ViewModels;
using System;
using Microsoft.Data.Sqlite;
using System.IO;



namespace ProjectDiary.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        //Open new task dialog
        this.WhenActivated(action =>
            action(ViewModel!.ShowTaskDialog.RegisterHandler(DoShowDialogAsync)));


    }
    private async Task DoShowDialogAsync(IInteractionContext<NewTaskViewModel, TaskControlViewModel?> interaction){
        var dialog = new NewTask
        {
            DataContext = interaction.Input
        };
        var result = await dialog.ShowDialog<TaskControlViewModel?>(this);
        interaction.SetOutput(result);
    }
}