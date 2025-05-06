using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Controls;
using ProjectDiary.Views;
using ReactiveUI;

namespace ProjectDiary.ViewModels;

public class NewTaskViewModel{
    public NewTaskViewModel(){
        //New Task Button logic, returns new task object to main window
        CreateTask = ReactiveCommand.Create(() =>{
            return new TaskControlViewModel(nameInput, prioInput, descInput,
            dateInput.DateTime, stateSelect, 6);
        });

    }
    public ReactiveCommand<Unit, TaskControlViewModel> CreateTask { get; }
    public string nameInput { get; set; } = "";
    public string descInput { get; set; } = "";
    public string prioInput { get; set; } = "";
    public string stateSelect { get; set; } = "";
    public DateTimeOffset dateInput { get; set; } = DateTime.Now;
    public ObservableCollection<string> Priorities { get; } = ["High", "Medium", "Low"];
    public ObservableCollection<string> States { get; } = ["To-Do", "Doing", "Done"];
}