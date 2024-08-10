using Microsoft.Data.Sqlite;


namespace XMLtoDB.src.Db
{
    public class DbContext : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly SqliteCommand _command;

        public DbContext()
        {
            _connection = new SqliteConnection("Data Source=..\\..\\..\\Test.db");
            _connection.Open();
            _command = new SqliteCommand();
            _command.Connection = _connection;
            CreateTables();
            CreateTemporaryTables();
        }

        public void AddUser(string userid, string fio, string email)
        {
            _command.CommandText =
                $"INSERT INTO UsersTemp (id, Fio, Email) VALUES (@UserId, @Fio, @Email)" +
                $"ON CONFLICT (email) DO NOTHING;";
            _command.Parameters.AddWithValue($"@UserId", userid);
            _command.Parameters.AddWithValue($"@Fio", fio);
            _command.Parameters.AddWithValue($"@Email", email);
            _command.ExecuteNonQuery();
        }

        public void AddGoods(string name, string price)
        {
            _command.CommandText =
                $"INSERT INTO GoodsTemp (Name, Price) VALUES (@GoodsName, @GoodsPrice)" +
                $"ON CONFLICT (Name) DO NOTHING;";
            _command.Parameters.AddWithValue($"@GoodsName", name);
            _command.Parameters.AddWithValue($"@GoodsPrice", price);
            _command.ExecuteNonQuery();
            _command.Parameters.Clear();
        }

        public void AddOrder(string no, string reg_date, string sum, string userid)
        {
            _command.CommandText =
                $"INSERT INTO OrdersTemp (No, Reg_Date, Sum, UserId) VALUES (@OrderNo, @Reg_Date, @Sum, @User)" +
                $"ON CONFLICT (No) DO NOTHING;";
            _command.Parameters.AddWithValue($"@OrderNo", no);
            _command.Parameters.AddWithValue($"@Reg_Date", reg_date);
            _command.Parameters.AddWithValue($"@Sum", sum);
            _command.Parameters.AddWithValue($"@User", userid);
            _command.ExecuteNonQuery();
            _command.Parameters.Clear();
        }

        public void AddProduct(string name, string quantity, string price, string orderid)
        {
            _command.CommandText =
                $"INSERT INTO ProductsTemp (Name, Quantity, Price, OrderId) VALUES (@ProductName, @ProductQuantity, @ProductPrice, @OrderId)" +
                $"ON CONFLICT (Name, OrderId) DO NOTHING;";
            _command.Parameters.AddWithValue($"@ProductName", name);
            _command.Parameters.AddWithValue($"@ProductQuantity", quantity);
            _command.Parameters.AddWithValue($"@ProductPrice", price);
            _command.Parameters.AddWithValue($"@OrderId", orderid);
            _command.ExecuteNonQuery();
            _command.Parameters.Clear();
        }
        
        public void ExecuteQuery()
        {
            _command.CommandText =
                "BEGIN;" +
                "INSERT INTO Users SELECT * FROM UsersTemp " +
                "WHERE true " +
                "ON CONFLICT (Email) DO NOTHING; " +
                "INSERT INTO Goods SELECT * FROM GoodsTemp " +
                "WHERE true " +
                "ON CONFLICT (Name) DO NOTHING;" +
                "INSERT INTO Orders SELECT * FROM OrdersTemp " +
                "WHERE true " +
                "ON CONFLICT (No) DO NOTHING; " +
                "INSERT INTO Products SELECT * From ProductsTemp " +
                "WHERE true " +
                "ON CONFLICT (Name, OrderId) DO NOTHING; " +
                "COMMIT;";
            _command.ExecuteNonQuery();   
        }

        private void CreateTables()
        {
            _command.CommandText =
                "CREATE TABLE IF NOT EXISTS Users (id TEXT NOT NULL PRIMARY KEY," +
                "Fio TEXT NOT NULL," +
                "Email TEXT UNIQUE NOT NULL);" +
                "CREATE TABLE IF NOT EXISTS Goods (Name TEXT NOT NULL PRIMARY KEY," +
                "Price REAL);" +
                "CREATE TABLE IF NOT EXISTS Orders (No INTEGER NOT NULL PRIMARY KEY," +
                "Reg_Date TEXT NOT NULL," +
                "Sum REAL," +
                "UserId INT," +
                "FOREIGN KEY (UserId) REFERENCES Users(Id));" +
                "CREATE TABLE IF NOT EXISTS Products(" +
                "Name TEXT NOT NULL," +
                "Quantity INT NOT NULL," +
                "Price REAL NOT NULL," +
                "OrderId INT NOT NULL," +
                "CONSTRAINT Id PRIMARY KEY (Name, OrderId)," +
                "FOREIGN KEY (OrderId) REFERENCES Orders(No)," +
                "FOREIGN KEY (Name) REFERENCES Goods(Name));";
            _command.ExecuteNonQuery();
        }

        private void CreateTemporaryTables()
        {
            _command.CommandText =
                "CREATE TEMP TABLE IF NOT EXISTS UsersTemp (id TEXT NOT NULL PRIMARY KEY," +
                "Fio TEXT NOT NULL," +
                "Email TEXT UNIQUE NOT NULL);" +
                "CREATE TEMP TABLE IF NOT EXISTS GoodsTemp (Name TEXT NOT NULL PRIMARY KEY," +
                "Price REAL);" +
                "CREATE TEMP TABLE IF NOT EXISTS OrdersTemp (No INTEGER NOT NULL PRIMARY KEY," +
                "Reg_Date TEXT NOT NULL," +
                "Sum REAL," +
                "UserId INT," +
                "FOREIGN KEY (UserId) REFERENCES UsersTemp(Id));" +
                "CREATE TEMP TABLE IF NOT EXISTS ProductsTemp (" +
                "Name TEXT NOT NULL," +
                "Quantity INT NOT NULL," +
                "Price REAL NOT NULL," +
                "OrderId INT NOT NULL," +
                "CONSTRAINT Id PRIMARY KEY (Name, OrderId)," +
                "FOREIGN KEY (OrderId) REFERENCES OrdersTemp(No)," +
                "FOREIGN KEY (Name) REFERENCES GoodsTemp(Name));";
            _command.ExecuteNonQuery();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
