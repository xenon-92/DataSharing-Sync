using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SpinLockingAndRecursion
{
    class BankAccount
    {
        public int Balance { get; set; }
        public void Deposit(int amount)
        {
            Balance += amount;
        }
        public void Withdraw(int amount)
        {
            Balance -= amount;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Task> tasks = new List<Task>();
            BankAccount ba = new BankAccount();
            SpinLock sl = new SpinLock();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    tasks.Add(Task.Factory.StartNew(()=> {
                        bool LockTaken = false;
                        try
                        {
                            sl.Enter(ref LockTaken);
                            ba.Withdraw(1000);
                        }
                        finally
                        {
                            if (LockTaken)
                            {
                                sl.Exit();
                            }
                        }
                    }));
                }
                for (int j = 0; j < 1000; j++)
                {
                    tasks.Add(Task.Factory.StartNew(() => {
                        bool lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken);
                            ba.Deposit(1000);
                        }
                        finally
                        {
                            if (lockTaken)
                            {
                                sl.Exit();
                            }
                            
                        }
                        
                    }));
                }
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"The balance is {ba.Balance}");
            Console.ReadKey();
        }
    }
}
