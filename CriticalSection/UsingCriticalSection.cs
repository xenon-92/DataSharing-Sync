using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CriticalSection
{
    class BankAccount_WithCriticalSectionn
    {
        object padlock = new object();
        public int Balance { get; private set; }
        public void Deposit(int amount)
        {
            //+=
            //op1: temp-> amount+balance
            //op2: balance = temp

            //this basically says no other thread can access the below statement, unless the present thread
            //finishes executing it
            lock (padlock)
            {
                Balance += amount;
            }
        }
        public void WithDraw(int amount)
        {
            lock (padlock)
            {
                Balance -= amount;
            }
        }
    }
    class UsingCriticalSection
    {
        static void Main()
        {
            List<Task> tasks = new List<Task>();
            BankAccount_WithCriticalSectionn ba = new BankAccount_WithCriticalSectionn();
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
                        ba.WithDraw(100);
                    }));
                }
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"the total balance is {ba.Balance}");
            Console.ReadKey();
        }
    }
}
