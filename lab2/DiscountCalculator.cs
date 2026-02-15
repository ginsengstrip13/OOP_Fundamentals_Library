namespace SOLID_Fundamentals
{
    public interface IDiscountStrategy
    {
        decimal CalculateDiscount(decimal amount);
    }

    public class RegularDiscount : IDiscountStrategy
    {
        public decimal CalculateDiscount(decimal amount) => amount * 0.05m;
    }

    public class PremiumDiscount : IDiscountStrategy
    {
        public decimal CalculateDiscount(decimal amount) => amount * 0.10m;
    }

    public class VipDiscount : IDiscountStrategy
    {
        public decimal CalculateDiscount(decimal amount) => amount * 0.15m;
    }

    public class NoDiscount : IDiscountStrategy
    {
        public decimal CalculateDiscount(decimal amount) => 0;
    }

    public class DiscountCalculator
    {
        public decimal Calculate(decimal amount, IDiscountStrategy strategy)
        {
            return strategy.CalculateDiscount(amount);
        }
    }
}