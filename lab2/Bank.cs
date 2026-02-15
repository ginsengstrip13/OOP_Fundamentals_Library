using System;

namespace SOLID_Fundamentals
{
    public abstract class Account
    {
        public decimal Balance { get; protected set; }
        public void Deposit(decimal amount) => Balance += amount;
    }

    public interface IWithdrawable
    {
        void Withdraw(decimal amount);
    }

    public class CheckingAccount : Account, IWithdrawable
    {
        public void Withdraw(decimal amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                Console.WriteLine($"Снято {amount}. Остаток: {Balance}");
            }
            else
                Console.WriteLine("Недостаточно средств");
        }
    }

    public class FixedDepositAccount : Account
    {
        public DateTime MaturityDate { get; private set; }
        public FixedDepositAccount(DateTime maturityDate) => MaturityDate = maturityDate;

    }

    public class BankService
    {
        public void WithdrawFromAccount(IWithdrawable account, decimal amount)
        {
            account.Withdraw(amount);
        }
    }
}