using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjectDiary.Models;
using ProjectDiary.ViewModels;
using ProjectDiary.Views;
using ReactiveUI;

namespace ProjectDiary.ViewModels;

public class TaskControlViewModel : ViewModelBase
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Priority { get; set; }
    public DateTime Deadline { get; set; }
    public DateTimeOffset DeadlineOffset {get;}
    public string? State { get; set; }
    //deadline to string for TaskView Binding
    public string DeadlineString { get; set; }
    public int TaskID { get; set; }
    public ObservableCollection<string> StateList {get;} = ["To-Do","Doing", "Done"];
    public ObservableCollection<string> PriorityList {get;} = ["High","Medium","Low"];
    //constructor
    public TaskControlViewModel(string name, string priority, string description, DateTime deadline, string state, int id){
        Name = name;
        Priority = priority;
        Description = description;
        Deadline = deadline;
        State = state;
        DeadlineString = Deadline.ToString("D");
        DeadlineOffset = new DateTimeOffset(Deadline);
        TaskID = id;
    }

    
}