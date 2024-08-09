using Microsoft.Data.Sqlite;


namespace XMLtoDB.src.Db
{
    public class DbContext
    {
        private readonly SqliteConnection _connection;
        private readonly SqliteCommand _command;
        private int index = 0;

        public DbContext()
        {
            _connection = new SqliteConnection("Data Source=..\\..\\..\\Test.db");
            _command = new SqliteCommand();
            _command.CommandText = "BEGIN;";
            CreateTables();
        }

        public void AddUser(string userid, string fio, string email)
        {
            _command.CommandText +=
                $"INSERT INTO Users (id, Fio, Email) VALUES (@UserId{index}, @Fio{index}, @Email{index})" +
                $"ON CONFLICT (email) DO NOTHING;";
            _command.Parameters.AddWithValue($"@UserId{index}", userid);
            _command.Parameters.AddWithValue($"@Fio{index}", fio);
            _command.Parameters.AddWithValue($"@Email{index}", email);
            index++;
        }

        public void AddGoods(string name, string price)
        {
            _command.CommandText +=
                $"INSERT INTO Goods (Name, Price) VALUES (@GoodsName{index}, @GoodsPrice{index})" +
                $"ON CONFLICT (Name) DO NOTHING;";
            _command.Parameters.AddWithValue($"@GoodsName{index}", name);
            _command.Parameters.AddWithValue($"@GoodsPrice{index}", price);
            index++;
        }

        public void AddOrder(string no, string reg_date, string sum, string userid)
        {
            _command.CommandText +=
                $"INSERT INTO Orders (No, Reg_Date, Sum, UserId) VALUES (@OrderNo{index}, @Reg_Date{index}, @Sum{index}, @User{index})" +
                $"ON CONFLICT (no) DO NOTHING;";
            _command.Parameters.AddWithValue($"@OrderNo{index}", no);
            _command.Parameters.AddWithValue($"@Reg_Date{index}", reg_date);
            _command.Parameters.AddWithValue($"@Sum{index}", sum);
            _command.Parameters.AddWithValue($"@User{index}", userid);
            index++;
        }

        public void AddProduct(string name, string quantity, string price, string orderid)
        {
            _command.CommandText +=
                $"INSERT INTO Products (Name, Quantity, Price, OrderId) VALUES (@ProductName{index}, @ProductQuantity{index}, @ProductPrice{index}, @OrderId{index})" +
                $"ON CONFLICT (Name, OrderId) DO NOTHING;";
            _command.Parameters.AddWithValue($"@ProductName{index}", name);
            _command.Parameters.AddWithValue($"@ProductQuantity{index}", quantity);
            _command.Parameters.AddWithValue($"@ProductPrice{index}", price);
            _command.Parameters.AddWithValue($"@OrderId{index}", orderid);
            index++;
        }
        
        public void ExecuteQuery()
        {
            _connection.Open();
            _command.Connection = _connection;
            _command.CommandText += "COMMIT;";
            _command.ExecuteNonQuery();
            _command.CommandText = "BEGIN;";
            index = 0;
        }

        public void ResetQuery()
        {
            _command.CommandText = "BEGIN;";
            index = 0;
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
                command.ExecuteNonQuery();
            }
        }
    }
}
