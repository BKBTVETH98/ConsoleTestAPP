using System.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using ConsoleApp2;
using System.Windows.Forms;

namespace HelloApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (SqlConnection sqlconnection = new SqlConnection(@"Data Source=DECEPTICON; initial catalog = catalogdb; Integrated Security = True"))
            {
                // Создание подключения
                try
                {
                    // Открываем подключение
                    await sqlconnection.OpenAsync();
                    Console.WriteLine("Подключение открыто\b");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                string Expression(string table,int pageOne, int pageTwo,int pageThree, int pageFour)
                {
                    return $"SELECT * FROM {table} WHERE  {table}.orders = {pageOne}  or {table}.orders = {pageTwo} or {table}.orders ={pageThree} or {table}.orders ={pageFour}";
                }
                string ExpressionTableName(string tableName)
                {
                    return $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'" ;
                }



                Console.Write("Введите страницы:");//ввод страниц с клавиатуры
                string page = Console.ReadLine();


               
                if (page.Contains("10") & page.Contains("11") & page.Contains("14") & page.Contains("15")) // проверка на содержание в консоли страниц
                {
                    using (SqlCommand command = new SqlCommand(ExpressionTableName("А"), sqlconnection)) //команда показа имени стеллажа
                    {
                        TableName(command);
                    }
                    using (SqlCommand command1 = new SqlCommand(Expression("А", 10, 11, 14, 15), sqlconnection))// после выполнения первой команды освобождаем reader и выводим основную информацию
                    {
                        ViewItem(command1);
                   
                    }
                    using (SqlCommand command2 = new SqlCommand(ExpressionTableName("Б"), sqlconnection))
                    {

                        TableName(command2);

                    }
                    using (SqlCommand command3  = new SqlCommand(Expression("Б", 10,11,14,15), sqlconnection))
                    {
                        ViewItem(command3);

                    }
                    using (SqlCommand command4 = new SqlCommand(ExpressionTableName("Ж"), sqlconnection))
                    {
                        TableName(command4);

                    }
                    using (SqlCommand command5 = new SqlCommand(Expression("Ж", 10,11,14,15), sqlconnection))
                    {
                        ViewItem(command5);
                    }

                }


                Console.Read();


                void TableName(SqlCommand commands) //отслеживаем имя стеллажа 
                {

                    using (SqlDataReader reader = commands.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {

                            while (reader.Read()) // построчно считываем данные
                            {
                                Console.WriteLine($"===Стеллаж {reader.GetValue(0)}");
                            }
                        }
                    }

                }
                void ViewItem(SqlCommand sqlcommand) //основной метод вывода в консоль
                {
                    using (SqlDataReader reader = sqlcommand.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {

                            while (reader.Read()) // построчно считываем данные
                            {

                                object id = reader.GetValue(0);
                                object orders = reader.GetValue(1);
                                object quantity = reader.GetValue(2);
                                object chelf = reader.GetValue(3);
                                object dopchelf = reader.GetValue(4);
                                object ProductName = reader.GetValue(5);

                                if (reader.GetValue(4) == DBNull.Value)
                                {
                                    Console.WriteLine($"{ProductName} id = {id}\nзаказ {orders},{quantity} шт");
                                }
                                else
                                {
                                    Console.WriteLine($"{ProductName} id = {id}\nзаказ {orders},{quantity} шт\nДоп стеллаж {dopchelf}");
                                }

                                Console.WriteLine();

                            }
                        }
                    }
                }
            }
        }
    }
}