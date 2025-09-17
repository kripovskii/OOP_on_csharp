using System;

namespace ConsoleApplication1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            BankAccount acc1 = new DebitAccount(123,"John Doe");
            BankAccount acc2 = new CreaditAccount(1337, "Pasha Technic");
            acc1.Deposit(100);
            acc1.Withdraw(200);
            acc2.Withdraw(800);
            acc2.ShowInfo();
            //acc1.Balance = 5000; - Ошибка, класс protected
        }
    }
    abstract class BankAccount
    {
        public int AccountNumber { get; } //Свойство
        public double Balance { get; protected set; }
        public string Owner { get; }

        public BankAccount(int accountNumber, string owner)
        {
            AccountNumber = accountNumber;
            Owner = owner;
            Balance = 0;
        }

        public void Deposit(double amount)
        {
            Balance += amount;
            Console.WriteLine($"Deposited {amount}, Balance: {Balance}");
        }
        public abstract void Withdraw(double amount);

        public virtual void ShowInfo()
        {
            Console.WriteLine($"Account number: {AccountNumber}");
            Console.WriteLine($"Owner: {Owner}");
            Console.WriteLine($"Balance: {Balance}");
        }
    }

    class DebitAccount : BankAccount
    {
        public DebitAccount(int accountNumber, string owner)
            : base(accountNumber, owner){}

        public override void Withdraw(double amount)
        {
            if (amount <= Balance)
            {
                Balance -= amount;
                Console.WriteLine($"Withdrawn {amount}, Balance: {Balance}");
            }
            else
            {
                Console.WriteLine("Insufficient funds");
            }
        }

    }

    class CreaditAccount : BankAccount
    {
        private const double CreditLimit = -1000.0;
        public CreaditAccount(int accountNumber, string owner)
            : base(accountNumber, owner){}

        public override void Withdraw(double amount)
        {
            if (Balance - amount >= CreditLimit)
            {
                Balance -= amount;
                Console.WriteLine($"Withdrawn {amount}, Balance: {Balance}");
            }
            else
            {
                Console.WriteLine("Insufficient funds");
            }
        }
    }

}