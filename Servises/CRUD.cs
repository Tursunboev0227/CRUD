using CRUD.Models;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Servises
{
    public static class CRUD
    {
        public static string CONNECTIOnSTRING = "Host=localhost;Port = 5432;Database = Lesson;User Id = postgres;Password = root;";

        public static void CreateTable()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIOnSTRING))
            {
                connection.Open();
                try
                {
                    using NpgsqlCommand cmd = new NpgsqlCommand($"Create table if not exists Students (id serial,full_name varchar(255),grade int,avg_score float);", connection);
                    var reader = cmd.ExecuteNonQuery();
                }
                catch
                {
                    Console.WriteLine("Something wrong with creating table");
                }
            }
        }

        public static void InsertIntoTable(Students student)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIOnSTRING))
            {

                connection.Open();
                try
                {
                    using NpgsqlCommand cmd = new NpgsqlCommand(@$"insert into Students (full_name,grade,avg_score)  values ('{student.fullName}',{student.grade},{student.avg_score});", connection);
                    var reader = cmd.ExecuteNonQuery();
                }
                catch
                {
                    Console.WriteLine("Something wrong with insert one student");
                }
            }
        }
        public static void InsertSeveralStudents(List<Students> students)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIOnSTRING))
            {

                connection.Open();
                try
                {
                    string query = @$"insert into Students (full_name,grade,avg_score)  values";

                    for (int i = 0; i < students.Count; i++)
                    {
                        query += @$"('{students[i].fullName}', {students[i].grade}, {students[i].avg_score}),";

                    }

                    string modifiedString = query.Substring(0, query.Length - 1);


                    NpgsqlCommand command = new NpgsqlCommand(query.Substring(0, query.Length - 1), connection);

                    command.ExecuteNonQuery();

                }
                catch
                {
                    Console.WriteLine("Something wrong with insert several students");
                }
            }
        }

        public static void GetAll()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIOnSTRING))
            {

                connection.Open();
                try
                {
                    using NpgsqlCommand cmd = new NpgsqlCommand(@$"select * from Students", connection);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID {reader["id"]} | Fullname : {reader["full_name"]} | {reader["grade"]} grade | avarage score : {reader["avg_score"]} ");
                    }

                }
                catch
                {
                    Console.WriteLine("Something wrong with getting all students");
                }
            }
        }
        public static void GetById(int id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIOnSTRING))
            {

                connection.Open();
                try
                {
                    using NpgsqlCommand cmd = new NpgsqlCommand(@$"select * from students where id = {id}", connection);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID {reader["id"]} | Fullname : {reader["full_name"]} | {reader["grade"]} grade | avarage score : {reader["avg_score"]} ");
                    }
                }
                catch
                {
                    Console.WriteLine("Something wrong with getting by id");
                }
            }
        }
        public static void Delete(int id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIOnSTRING))
            {

                connection.Open();
                try
                {
                    using NpgsqlCommand cmd = new NpgsqlCommand(@$"delete from students where id = {id}", connection);
                    var reader = cmd.ExecuteReader();
                    Console.WriteLine("Succesfully deleted");
                }
                catch
                {
                    Console.WriteLine("Something wrong with deleting");
                }
            }
        }
        public static void UpdateStudent(Students student, int id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIOnSTRING))
            {

                connection.Open();
                try
                {
                    using NpgsqlCommand cmd = new NpgsqlCommand(@$"update students set full_name = '{student.fullName}',grade = {student.grade},avg_score = {student.avg_score} where id = {id}", connection);
                    var reader = cmd.ExecuteReader();
                    Console.WriteLine("Succesfully updated");
                }
                catch
                {
                    Console.WriteLine("Something wrong with updateing student");
                }
            }
        }
        public static void UpdateAnyColumn(int id, string column, string value)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIOnSTRING))
            {
                connection.Open();
                try
                {
                    switch (column)
                    {
                        case "full_name":
                            using (NpgsqlCommand cmd = new NpgsqlCommand($@"update students SET full_name = '{value}' where id = {id}", connection))
                            {
                                cmd.ExecuteNonQuery();
                            }
                            break;
                        case "grade":
                            if (int.TryParse(value, out int gradeValue))
                            {
                                using (NpgsqlCommand cmd = new NpgsqlCommand($@"UPDATE students SET grade = {gradeValue} WHERE id = {id}", connection))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid grade value");
                            }
                            break;
                        case "avg_score":
                            if (double.TryParse(value, out double avgScoreValue))
                            {
                                using (NpgsqlCommand cmd = new NpgsqlCommand($@"UPDATE students SET avg_score = {avgScoreValue} WHERE id = {id}", connection))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid average score value");
                            }
                            break;
                        default:
                            Console.WriteLine("Column does not exist");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating student: {ex.Message}");
                }
            }
        }
        public static void GetLike(string column, string value)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIOnSTRING))
            {
                connection.Open();
                try
                {
                    switch (column)
                    {
                        case "full_name":
                            using (NpgsqlCommand cmd = new NpgsqlCommand($@"select * from students where {column} like %{value}%", connection))
                            {
                                var reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    Console.WriteLine($"ID {reader["id"]} | Fullname : {reader["full_name"]} | {reader["grade"]} grade | avarage score : {reader["avg_score"]} ");
                                }
                            }
                            break;
                        case "grade":
                            if (int.TryParse(value, out int gradeValue))
                            {
                                using (NpgsqlCommand cmd = new NpgsqlCommand($@"select * from students where {column} like %{gradeValue}%", connection))
                                {
                                    var reader = cmd.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        Console.WriteLine($"ID {reader["id"]} | Fullname : {reader["full_name"]} | {reader["grade"]} grade | avarage score : {reader["avg_score"]} ");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid grade value");
                            }
                            break;
                        case "avg_score":
                            if (double.TryParse(value, out double avgScoreValue))
                            {
                                using (NpgsqlCommand cmd = new NpgsqlCommand($@"select * from students where {column} like %{avgScoreValue}%", connection))
                                {
                                    var reader = cmd.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        Console.WriteLine($"ID {reader["id"]} | Fullname : {reader["full_name"]} | {reader["grade"]} grade | avarage score : {reader["avg_score"]} ");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid average score value");
                            }
                            break;
                        default:
                            Console.WriteLine("Column does not exist");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating student: {ex.Message}");
                }

            }

        }
        public static void AddNewColumn(string columnName,string type)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIOnSTRING))
            {

                connection.Open();
                try
                {
                    using NpgsqlCommand cmd = new NpgsqlCommand(@$"Alter table students add column {columnName} {type}", connection);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Succesfully added");

                }
                catch
                {
                    Console.WriteLine("Something wrong with adding column");
                }
            }
        }
        public static void AddColumnWithDefaultValue(string columnName,string type,string defaultValue)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIOnSTRING))
            {

                connection.Open();
                try
                {
                    using NpgsqlCommand cmd = new NpgsqlCommand(@$"Alter table students add column {columnName} {type} default {defaultValue}", connection);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Succesfully added");

                }
                catch
                {
                    Console.WriteLine("Something wrong with adding new column with default");
                }
            }
        }
        public static void UpdateColumnName(string oldName,string newName)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIOnSTRING))
            {

                connection.Open();
                try
                {
                    using NpgsqlCommand cmd = new NpgsqlCommand(@$"Alter table students rename column {oldName} to {newName}", connection);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Succesfully updated");

                }
                catch
                {
                    Console.WriteLine("Something wrong with updating ColumnName");
                }
            }
        }
        public static void updateColumnName(string newName)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIOnSTRING))
            {

                connection.Open();
                try
                {
                    using NpgsqlCommand cmd = new NpgsqlCommand(@$"Alter table students rename to {newName}", connection);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Succesfully updated");

                }
                catch
                {
                    Console.WriteLine("Something wrong with updating tableName");
                }
            }
        }
        public static void CreateNewDatabase(string newDatabaseName)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIOnSTRING))
            {

                connection.Open();
                try
                {
                    using NpgsqlCommand cmd = new NpgsqlCommand(@$"Create database {newDatabaseName}", connection);
                    cmd.ExecuteNonQuery();
                    string ConnectionString = @$"Host=localhost;Port = 5432;Database = {newDatabaseName.ToLower()};User Id = postgres;Password = root;";
                    NpgsqlConnection newConnection = new NpgsqlConnection(ConnectionString);
                    newConnection.Open();

                   string  query = $"Create table Users(id serial, fullname varchar(50),age int); Create table Children(id serial, fulla_name varchar(255),group int); Create table employees(id serial, full_name varchar(255),salary real);";

                    NpgsqlCommand newCommand = new NpgsqlCommand(query, newConnection);

                    newCommand.ExecuteNonQuery();

                    newConnection.Close();


                    connection.Close();

                }
                catch
                {
                    Console.WriteLine("Something wrong with ucreating new database");
                }
            }
        }
        public static void Truncate(string tableName)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIOnSTRING))
            {

                connection.Open();
                try
                {
                    using NpgsqlCommand cmd = new NpgsqlCommand(@$"truncate table {tableName}", connection);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Succesfully truncated");

                }
                catch
                {
                    Console.WriteLine("Something wrong with truncating table");
                }
            }
        }
        public static void JoinTwoTables(string table1,string table2)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIOnSTRING))
            {

                connection.Open();
                try
                {
                    using NpgsqlCommand cmd = new NpgsqlCommand(@$"select * from {table1} as t1 join {table2} as t2 on t1.id = t2.id;", connection);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Succesfully joined");

                }
                catch
                {
                    Console.WriteLine("Something wrong with truncating table");
                }
            }
        }
        public static void Indexing(string IndexName,string columnName)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIOnSTRING))
            {

                connection.Open();
                try
                {
                    using NpgsqlCommand cmd = new NpgsqlCommand(@$"Create index {IndexName} on students ({columnName});", connection);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Succesfully created inxed");

                }
                catch
                {
                    Console.WriteLine("Something wrong with  indexing");
                }
            }
        }
    }
}
