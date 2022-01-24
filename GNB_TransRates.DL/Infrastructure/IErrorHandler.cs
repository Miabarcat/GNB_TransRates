using Microsoft.AspNetCore.Identity;

namespace GNB_TransRates.DL.Infrastructure
{
    public interface IErrorHandler
    {
        string GetMessage(ErrorMessagesEnum message);
    }


    public enum ErrorMessagesEnum
    {
        EntityNull = 1,
        ModelValidation = 2,
        NotFound = 3
    }
}
