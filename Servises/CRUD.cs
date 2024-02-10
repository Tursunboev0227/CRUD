using CRUD.Models;
using Npgsql;
using System;
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


    }
}
