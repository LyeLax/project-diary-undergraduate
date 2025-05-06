using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ProjectDiary.Views;

public partial class TaskControlView : UserControl{
    public TaskControlView(){
        InitializeComponent();
    }
    //Tracks what Task is currently open in Task Edit by ID    
    public static int TaskIdent {get;set;}
    public static string StateIdent {get;set;} ="";
    public void Edit_Task(object sender, PointerPressedEventArgs args){
        TaskIdent = int.Parse(ID.Text!);
        StateIdent = status.ToString()!;
        var dialog = new TaskEdit
        {
            DataContext = DataContext,
        };
        dialog.Show();
    }
}