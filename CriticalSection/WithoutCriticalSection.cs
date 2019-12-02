using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CriticalSection
{
    class WithoutCriticalSection
    {
        static void Main(string[] args)
        {
            BankAccount ba = new BankAccount();
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(()=> {
                    for (int j = 0; j < 1000; j++)
                    {
                        ba.Deposit(10);
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() => {
                    for (int j = 0; j < 1000; j++)
                    {
                        ba.WithDraw(10);
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"the final balance is {ba.Balance}");
            Console.ReadKey();
        }
    }
    class BankAccount
    {
        public int Balance { get; private set; }
        public void Deposit(int amount)
        {
            Balance += amount;
        }
        public void WithDraw(int amount)
        {
            Balance -= amount;
        }
    }
}
