using System;

namespace ConsoleApplication1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Animal cow = new Animal();
            cow.Name = "Cow";
            cow.age = 15;
            cow.Eat();
            Cat barsik = new Cat();
            barsik.Name = "Barsik";
            barsik.age = 3;
            barsik.Eat();
            barsik.SayMew();
            barsik.poroda = "Сибирсикй";
        }
    }

    public class Animal
    {
        public int age;
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (value != null) name = value;
                else
                {
                    Console.WriteLine("Name is required");
                }
            }
        }

        public void Eat()
        {
            Console.WriteLine("Eating");
        }
    }

    public class Cat : Animal
    {
        public string poroda;

        public void SayMew()
        {
            Console.WriteLine("Mew!");
        }
    }

}