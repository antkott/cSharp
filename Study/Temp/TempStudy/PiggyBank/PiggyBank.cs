using System;

namespace PiggyBank
{
    internal class PiggyBank
    {
        private int _balance = 0;
        public int Balance
        {
            get => _balance;
            set
            {
                int currentBalance = _balance;
                currentBalance += value;
                if (currentBalance < 0)
                {
                    Console.WriteLine($"Error, you can't get/add '{value}' to deposit. Current balance is '{Balance}'");
                }
                else
                {
                    _balance = currentBalance;
                    Notify?.Invoke(value);
                    NotifyGoal?.Invoke();
                }
            }
        }

        public int SavingGoal { get; }
        private delegate void AccountHandler(int amount);
        private event AccountHandler Notify;
        private delegate void GoalHandler();
        private event GoalHandler NotifyGoal;

        public PiggyBank(int savingGoal)
        {
            SavingGoal = savingGoal;
            Notify += (amount) => Console.WriteLine($"The balance was deposited on '{amount}', current state is '{Balance}'");
            NotifyGoal += () =>
            {
                if (Balance > SavingGoal)
                {
                    Console.WriteLine($"Saving goal '{SavingGoal}' was achived");
                }
            };
        }
    }
}
