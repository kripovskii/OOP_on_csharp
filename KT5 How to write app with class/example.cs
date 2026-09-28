using System;
using System.Collections.Generic;

namespace OOP_IT_SOIP_25_P2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Cart cart = new Cart();
            Product milk =  new Product("milk", 190);
            Product Iphone_Duo = new Product("Iphone_Duo", 700000);
            Product Samsung = new Product("Samsung", 70000);
            cart.AddProduct(Iphone_Duo);
            cart.AddProduct(Samsung);
            cart.AddProduct(milk);
            cart.CalculateTotalPrice();

        }
    }

    public class Product
    {
       public string Name { get; set; }
       public double Price { get; private set; }
       
        public Product(string name, double price)
        {
            Name = name;
            if (price <= 0) Price = 0;
            else Price = price;
        }
    }

    class Cart
    {
        private List<Product> products = new List<Product>();
        // коллекция продуктов = Массив продуктов без ограничений по кол-ву
        public void AddProduct(Product product) //Передали продукт 
        {
            products.Add(product); //добавляем в коллецию
            Console.WriteLine($"{product.Name} has been added to the cart");
        }

        public void CalculateTotalPrice()
        {
            double totalPrice = 0;
            foreach (var product in products) //Цикл перебора коллекции
            {
                totalPrice += product.Price;
            }
            Console.WriteLine($"Total price: {totalPrice}");
        }
    }

}

    
