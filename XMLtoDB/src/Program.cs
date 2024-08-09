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
        foreach(var product in order.Products)
        {
            product.Id = Guid.NewGuid();
        }
    }
}


DbContext db = new DbContext();

foreach(var order in orders)
{
    db.AddUser(order.User.Id.ToString(), order.User.Fio, order.User.Email);

    db.AddOrder(order.No.ToString(), order.Reg_Date, order.Sum.ToString(), order.User.Id.ToString());

    foreach (var product in order.Products)
    {
        db.AddGoods(product.Name, (product.Price / product.Quantity).ToString());

        db.AddProduct(product.Id.ToString(), product.Name, product.Quantity.ToString(), product.Price.ToString(), order.No.ToString());
    }
}


Console.ReadLine();


[XmlRoot("Orders")]
public class Orders
{
    [XmlElement("order")]
    public List<Order> orders { get; set; }
}