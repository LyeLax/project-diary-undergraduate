using System;
using System.Diagnostics;
using System.IO;
using Avalonia;
using Microsoft.Data.Sqlite;

namespace ProjectDiary.Models;
public class DBConnect {
    public SqliteConnection? sqliteConnection;
    //database connection method, takes the filename of the database
    //file name needs to be full path, will add something later
    //includes a method of finding the database path in the build directory
    public void DatabaseConnect(string filename){
        var constring = new SqliteConnectionStringBuilder(){
            DataSource = filename,
            ForeignKeys = true
        }.ToString();
        sqliteConnection = new SqliteConnection(constring);
        try{
            sqliteConnection.Open();
        } catch (SqliteException){
            Debug.WriteLine("Database Connection Failed");
        } catch (Exception){
            Debug.WriteLine("Error in database connect");
        }
    }
    //disconnects from a database
    public void Disconnect(){
        sqliteConnection?.Close();
    }
    //used to run database queries that don't require data passed back
    public bool SQLQuery (string sql){
        if(sqliteConnection!=null){
            try{
                SqliteCommand command = sqliteConnection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();
                return true;
            } catch(SqliteException){
                Debug.WriteLine("Error found in command");
                return false;
            }
        } else {
            Debug.WriteLine("No database connected");
            return false;
        }
    }
    //to get data out of the database, currently returns null
    public SqliteDataReader? SQLGet(string sql) {
        SqliteDataReader? dataReader = null;
        if(sqliteConnection != null){
            try{
                var command = new SqliteCommand(sql, sqliteConnection);
                dataReader = command.ExecuteReader();                
            } catch(SqliteException){
                Debug.WriteLine("Error found in command");
            }
        } else {
            Debug.WriteLine("No database connected");
        }
        return dataReader;
    }
}