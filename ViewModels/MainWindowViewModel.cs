using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using Avalonia.Input;
using Microsoft.Data.Sqlite;
using ProjectDiary.Views;
using ReactiveUI;
using ProjectDiary.Models;

namespace ProjectDiary.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(){
        DataLoad();
        //Creates a link between the view model of the new task form and the task to be created
        ShowTaskDialog = new Interaction<NewTaskViewModel, TaskControlViewModel?>();
        //when button is pressed, shows a new dialog page and waits for the result
        //then adds result to the appropriate task list
        OpenNewTask = ReactiveCommand.CreateFromTask(async() =>{
            var page = new NewTaskViewModel();
            var result = await ShowTaskDialog.Handle(page);
            //passes result of new task page
            //switch case determines where to put the new task based off state
            if (result != null){
                int statepass = 0;
                switch (result.State){
                    case "To-Do":
                    statepass = 1;
                    break;
                    case "Doing":
                    statepass = 2;
                    break;
                    case "Done":
                    statepass = 3;
                    break;
                }
                //task sent to persistent data
                DBConnect database = new();
                //searches for a .db file in the current directory to connect to
                string path = Directory.GetFiles(Directory.GetCurrentDirectory(),"*.db")[0];
                database.DatabaseConnect(path);
                string sql = $"INSERT INTO tasks (taskName,taskPriority,taskDescription,taskDueDate,taskState) VALUES ('{result.Name}', '{result.Priority}', '{result.Description}', '{result.DeadlineString}',{statepass})";
                database.SQLQuery(sql);
                database.Disconnect();
                ListUpdate();
            }
        }); 
        //capacity test of on screen lists
        
        for (int i = 0; i < 100; i++){
            var addition = new TaskControlViewModel(i.ToString(), "Low", "", DateTime.Now, "yes",1000);
            TaskList.Add(addition);
            DoingTaskList.Add(addition);
            DoneTaskList.Add(addition);
        }
    }
    public ICommand OpenNewTask {get;}
    //Data passing system for new task
    public Interaction<NewTaskViewModel, TaskControlViewModel?> ShowTaskDialog {get;}
    //Task Lists
    public static ObservableCollection<TaskControlViewModel> TaskList {get; set;} = [];
    public static ObservableCollection<TaskControlViewModel> DoingTaskList {get; set;} = [];
    public static ObservableCollection<TaskControlViewModel> DoneTaskList {get; set;} = [];
    //Static method to pull all tasks from local storage
    //tasks added into their lists based on taskState
    public static void DataLoad(){
        DBConnect database = new();
        //searches for a .db file in the current directory to connect to
        string path = Directory.GetFiles(Directory.GetCurrentDirectory(),"*.db")[0];
        database.DatabaseConnect(path);
        string sql = "SELECT * FROM tasks";
        SqliteDataReader? load = database.SQLGet(sql); 
        if (load != null){
            while(load.Read()){
                int name = load.GetOrdinal("taskName");
                int priority = load.GetOrdinal("taskPriority");
                int desc = load.GetOrdinal("taskDescription");
                DateTime date = DateTime.Parse(load.GetString(load.GetOrdinal("taskDueDate")));
                int state = load.GetInt32(load.GetOrdinal("taskState"));
                //state names hard coded, could add variable state names in future
                switch (state){
                    case 1:
                    TaskList.Add(new TaskControlViewModel(load.GetString(name),
                    load.GetString(priority),load.GetString(desc), date,"To-Do", load.GetInt32(0)));
                    break;
                    case 2:
                    DoingTaskList.Add(new TaskControlViewModel(load.GetString(name),
                    load.GetString(priority),load.GetString(desc), date,"Doing",load.GetInt32(0)));
                    break;
                    case 3:
                    DoneTaskList.Add(new TaskControlViewModel(load.GetString(name),
                    load.GetString(priority),load.GetString(desc), date,"Done",load.GetInt32(0)));
                    break;
                }
            }
        }
        database.Disconnect();
    }
    public static void ListUpdate(){
        TaskList.Clear();
        DoingTaskList.Clear();
        DoneTaskList.Clear();
        DataLoad();
    }
}