using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace March18.Models
{
    public class TaskDB
    {
        private string _connectionString;
        public TaskDB(string cs)
        {
            _connectionString = cs;
        }
        public List<ToDoItems> GetIncomplete()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"select * from tasks t
                                join categories c on t.categoryID = c.id
                    where t.completeddate is null";
            connection.Open();
            List<ToDoItems> result = new List<ToDoItems>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new ToDoItems
                {
                    Id = (int)reader["id"],
                    CategooryId = (int)reader["categoryID"],
                    CompletedDate = (DateTime)reader.GetOrNull<DateTime>("CompletedDate"),
                    DueDate = (DateTime)reader["DueDate"],
                    Title = (string)reader["Title"],
                    CategoryName = (string)reader["Name"]
                    
                });
            }
            connection.Close();
            connection.Dispose();
            return result;
            
        }
        public void MarkAsComplete(int id)
        {
            DateTime dt = DateTime.Now;
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"update tasks set CompletedDate = @complete where id = @id";
            cmd.Parameters.AddWithValue("@complete", dt);
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            connection.Dispose();
        }
        public List<ToDoItems> GetCompleted()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"select * from tasks t
                                join categories c on t.categoryID = c.id
                    where t.completeddate is not null";
            connection.Open();
            List<ToDoItems> result = new List<ToDoItems>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new ToDoItems
                {
                    Id = (int)reader["id"],
                    CategooryId = (int)reader["categoryID"],
                    CompletedDate = (DateTime)reader.GetOrNull<DateTime>("CompletedDate"),
                    DueDate = (DateTime)reader["DueDate"],
                    Title = (string)reader["Title"],
                    CategoryName = (string)reader["Name"]

                });
            }
            connection.Close();
            connection.Dispose();
            return result;
            
        }
        public List<Category> GetCategories()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"select * from categories";
            connection.Open();
            List<Category> result = new List<Category>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Category
                {
                    Id = (int)reader["Id"],
                   Name = (string)reader["Name"]
                });
            }
            connection.Close();
            connection.Dispose();
            return result;

        }
        public void EditCategory(Category c)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"update categories set name = @name where id = @id";
            cmd.Parameters.AddWithValue("@name", c.Name);
            cmd.Parameters.AddWithValue("@id", c.Id);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            connection.Dispose();
        }
        public void AddToDo(ToDoItems t, Category c)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"insert into tasks(Title, DueDate, CategoryId)
                            values(@title, @due, @catId)";
            cmd.Parameters.AddWithValue("@title", t.Title);
            cmd.Parameters.AddWithValue("@due", t.DueDate);
            cmd.Parameters.AddWithValue("@catId", c.Id);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            connection.Dispose();
        }
        public Category GetCategoryId(string name)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"select * from categories where Name= @name";
            cmd.Parameters.AddWithValue("@name", name);
            connection.Open();
            Category result = new Category();          
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result = new Category
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"]
                };
            }
            connection.Close();
            connection.Dispose();
            return result;

        }
        public void AddCategory(string name)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"insert into categories(name)
                                values(@name)";
            cmd.Parameters.AddWithValue("@name", name);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            connection.Dispose();
        }

    }
    public class ToDoItems
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int CategooryId { get; set; }
        public string CategoryName { get; set; }
    }
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public static class Extensions
    {
        public static T GetOrNull<T>(this SqlDataReader reader, string column)
        {
            object obj = reader[column];
            if (obj == DBNull.Value)
            {
                return default(T);
            }
            return (T)obj;
        }
    }
}
