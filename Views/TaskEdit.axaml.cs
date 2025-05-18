using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ProjectDiary.Models;
using ProjectDiary.ViewModels;
using Microsoft.Data.Sqlite;

namespace ProjectDiary.Views;

public partial class TaskEdit : Window
{
    public TaskEdit()
    {
        InitializeComponent();       
    }
    //updates database with new data, refreshes lists
    public void Save_Button (object sender, RoutedEventArgs args){
        //Pull all data into variables to send
        string desc = DescriptionEdit.Text!.ToString();
        string date = DateEdit.SelectedDate!.ToString()!;
        string prio = PriorityEdit.SelectedItem!.ToString()!;
        
        //turn state into int for database FK
        int state = 1;
        switch(StateEdit.SelectedItem!.ToString()!){
            case "To-Do":
            state = 1;
            break;
            case "Doing":
            state = 2;
            break;
            case "Done":
            state = 3;
            break;
        }
        //Database update
        DBConnect connect = new();
        string path = @"C:\Users\Nate\source\repos\LyeLax\project-diary-undergraduate\projectdiary.db";
        connect.DatabaseConnect(path);
        //needs to update tasks set each field with the changable section
        //Description, Date, Priority, State
        string sql =  $"UPDATE tasks SET taskDescription = '{desc}', taskDueDate = '{date}', taskPriority = '{prio}', taskState = {state}  WHERE taskID = {TaskControlView.TaskIdent}";
        connect.SQLQuery(sql);
        connect.Disconnect();
        //Update lists and close window
        MainWindowViewModel.ListUpdate();
        Close();
    }
    //Deletes the task from system, this includes from the database
    public void Delete_Button (object sender, RoutedEventArgs args){
        //Database Deletion
        DBConnect connect = new();
        string path = @"C:\Users\Nate\source\repos\LyeLax\project-diary-undergraduate\projectdiary.db";
        connect.DatabaseConnect(path);
        string sql = $"DELETE FROM tasks WHERE taskID = {TaskControlView.TaskIdent}";
        connect.SQLQuery(sql);
        connect.Disconnect();
        //To Add - Removal of Task from screen, clear all tasks, load all data
        MainWindowViewModel.ListUpdate();
        Close();
    }

}