namespace PaymentProcessor
{
    public interface IProcessPayment
    {
        Task<bool> Pay();
    }
}