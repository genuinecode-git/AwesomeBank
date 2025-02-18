namespace AwesomeBank.Test.FunctionalTests.ServiceMocks
{
    public interface ITransactionServiceMock
    {
        public Task InputTransactionsAsync();
    }

    public class TransactionServiceMock : ITransactionServiceMock
    {
        public Task InputTransactionsAsync()
        {
            throw new NotImplementedException();
        }
    }
}