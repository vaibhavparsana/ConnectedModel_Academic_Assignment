using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;

namespace introtoDB
{
    class Program
    {
        static void Main(string[] args)
        {
            string cs = GetConnectionString("NorthwindLocalDB");
            GetProductsName();
        }

        static string GetConnectionString(string connectionStringName)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("config.json");
            IConfiguration config = configurationBuilder.Build();
            return config["ConnectionString:" + connectionStringName];
        }

        static void GetProductsName()
        {
            Console.Write("\nEnter Category_ID between 1 to 8: ");
            string categoryId = Console.ReadLine();
            int cid = Convert.ToInt32(categoryId);
            while(cid <= 0 || cid >= 9)
            {
                Console.Write("\nEnter Category_ID between 1 to 8: ");
                categoryId = Console.ReadLine();
                cid = Convert.ToInt32(categoryId);
            }

            string cs = GetConnectionString("NorthwindLocalDB");
            string query = "Select ProductID, ProductName, CategoryName from Products inner join Categories on Products.CategoryID = Categories.CategoryID  Where Categories.CategoryID = @CategoryId";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("CategoryId", categoryId);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                Console.WriteLine($"\nProductID\tProductName\t\t\tCategoryName");
                while (reader.Read())
                {
                    int productId = (int)reader["ProductID"];
                    string productName = (string)reader["ProductName"];
                    string categoryName = (string)reader["CategoryName"];
                   
                    Console.WriteLine($"{productId,-15} {productName,-31} {categoryName} ");
                }
            }
        }
    }
}
