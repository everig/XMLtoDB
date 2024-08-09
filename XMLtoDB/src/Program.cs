using XMLtoDB.src.Models;
using System.Xml.Serialization;
using System.Xml;
using XMLtoDB.src.Db;



List<Order> orders;

XmlSerializer serializer = new XmlSerializer(typeof(Orders));

using (FileStream reader = new FileStream("..\\..\\..\\orders.xml", FileMode.OpenOrCreate))
{
    var result = (Orders)serializer.Deserialize(reader);
    orders = result.orders;

    foreach(var order in orders)
    {
        order.User.Id = Guid.NewGuid();
    }
}


DbContext db = new DbContext();
try
{
    foreach (var order in orders)
    {
        db.AddUser(order.User.Id.ToString(), order.User.Fio, order.User.Email);

        db.AddOrder(order.No.ToString(), order.Reg_Date, order.Sum.ToString(), order.User.Id.ToString());

        foreach (var product in order.Products)
        {
            db.AddGoods(product.Name, (product.Price / product.Quantity).ToString());

            db.AddProduct(product.Name, product.Quantity.ToString(), product.Price.ToString(), order.No.ToString());
        }
        
    }
    db.ExecuteQuery();

}
finally
{
    db.ResetQuery();
}

var user = orders[0].User;

db.AddUser(user.Id.ToString(), user.Fio, user.Email);


Console.ReadLine();


[XmlRoot("Orders")]
public class Orders
{
    [XmlElement("order")]
    public List<Order> orders { get; set; }
}