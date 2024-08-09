using Microsoft.Data.Sqlite;

namespace XMLtoDB.src.Db
{
    public class DbContext
    {
        private readonly SqliteConnection _connection;

        public DbContext()
        {
            _connection = new SqliteConnection("Data Source=..\\..\\..\\Test.db");
            CreateTables();
        }

        public void AddUser(string userid, string fio, string email)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText =
                $"INSERT INTO Users (id, Fio, Email) VALUES ('{userid}', '{fio}', '{email}')" +
                $"ON CONFLICT (id) DO NOTHING;";
            command.ExecuteNonQuery();
        }

        public void AddGoods(string name, string price)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText =
                $"INSERT INTO Goods (Name, Price) VALUES ('{name}', '{price}')" +
                $"ON CONFLICT (Name) DO NOTHING";
            command.ExecuteNonQuery();
        }

        public void AddOrder(string no, string reg_date, string sum, string userid)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText =
                $"INSERT INTO Orders (No, Reg_Date, Sum, UserId) VALUES ('{no}', '{reg_date}', '{sum}', '{userid}')" +
                $"ON CONFLICT (no) DO NOTHING";
            command.ExecuteNonQuery();
        }

        public void AddProduct(string id, string name, string quantity, string price, string orderid)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText =
                $"INSERT INTO Products (id, Name, Quantity, Price, OrderId) VALUES ('{id}', '{name}', '{quantity}', '{price}', '{orderid}')" +
                $"ON CONFLICT (id) DO NOTHING";
            command.ExecuteNonQuery();
        }

        private void CreateTables()
        {
            using (_connection)
            {
                _connection.Open();
                SqliteCommand command = _connection.CreateCommand();
                command.CommandText =
                    "CREATE TABLE IF NOT EXISTS Users (id TEXT NOT NULL PRIMARY KEY," +
                    "Fio TEXT NOT NULL," +
                    "Email TEXT NOT NULL);" +
                    "CREATE TABLE IF NOT EXISTS Goods (Name TEXT NOT NULL PRIMARY KEY," +
                    "Price REAL);" +
                    "CREATE TABLE IF NOT EXISTS Orders (No INTEGER NOT NULL PRIMARY KEY," +
                    "Reg_Date TEXT NOT NULL," +
                    "Sum REAL," +
                    "UserId INT," +
                    "FOREIGN KEY (UserId) REFERENCES Users(Id));" +
                    "CREATE TABLE IF NOT EXISTS Products(id TEXT NOT NULL PRIMARY KEY," +
                    "Name TEXT NOT NULL," +
                    "Quantity INT NOT NULL," +
                    "Price REAL NOT NULL," +
                    "OrderId INT NOT NULL," +
                    "FOREIGN KEY (OrderId) REFERENCES Orders(No)," +
                    "FOREIGN KEY (Name) REFERENCES Goods(Name));";
                command.ExecuteNonQuery();
            }
        }
    }
}
