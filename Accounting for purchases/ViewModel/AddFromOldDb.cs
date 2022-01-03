using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Microsoft.Win32;
using System.Windows.Forms;


namespace Accounting_for_purchases
{
    public class AddFromOldDb
    {
        public static void AddFromDb()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog() { Filter = "SQLite файлы(*.db)|*.db" };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string stringSQLite = @"Data Source=" + openFileDialog1.FileName;
                var connSQLite = new SQLiteConnection(stringSQLite);
                connSQLite.Open();
                var command = connSQLite.CreateCommand();
                command.CommandText = "select * from sprav";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var sprav = new Model.Sprav { Id = reader.GetInt32(0), Product = reader.GetString(1), Wholesale = reader.GetInt32(2), Retail = reader.GetInt32(3) };
                    using (var db = new ConnectDB())
                    {
                        try
                        {
                            db.Sprav.Add(sprav);
                            db.SaveChanges();
                        }
                        catch (Exception e) { Console.WriteLine(e.Message); Console.WriteLine(e.StackTrace); }

                    }
                }
                command = connSQLite.CreateCommand();
                command.CommandText = "select * from \"order\"";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var sprav = new Model.Order { id = reader.GetInt32(0), Date = reader.GetDateTime(1), EmployeeId = 1 };
                    using (var db = new ConnectDB())
                    {
                        try
                        {
                            db.Order.Add(sprav);
                           db.SaveChanges();
                        }
                        catch (Exception e) { Console.WriteLine(e.Message); Console.WriteLine(e.StackTrace); }

                    }
                }
                command = connSQLite.CreateCommand();
                command.CommandText = "select * from product";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var sprav = new Model.Product{ Id = reader.GetInt32(0), SpravId=reader.GetInt32(1), OrderId=reader.GetInt32(2), Discount= reader.GetInt32(3),Count= reader.GetInt32(4),  CostForClient= reader.GetInt32(5), Profit= reader.GetInt32(6), };
                    using (var db = new ConnectDB())
                    {
                        try
                        {
                            db.Product.Add(sprav);
                            db.SaveChanges();
                        }
                        catch (Exception e) { Console.WriteLine(e.Message); Console.WriteLine(e.StackTrace); }

                    }
                }
            }
        }
    }
}
