using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InterLockedOperations
{
    class BankAccount
    {
        private int balance;

        public int Balance { get => balance; set => balance = value; }
        public void Deposit(int amount)
        {
            Interlocked.Add(ref balance, amount);
        }
        public void Withdraw(int amount)
        {
            Interlocked.Add(ref balance, -amount);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            BankAccount ba = new BankAccount();
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    tasks.Add(Task.Factory.StartNew(() => {
                        ba.Deposit(100);
                    }));
                }
                for (int j = 0; j < 1000; j++)
                {
                    tasks.Add(Task.Factory.StartNew(() => {
                        ba.Withdraw(100);
                    }));
                }
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"The final balance is {ba.Balance}");
            Console.ReadKey();
        }
    }
}
