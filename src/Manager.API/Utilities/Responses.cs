using Manager.API.ViewModels;

namespace Manager.API.Utilities
{
    public class Responses
    {
        public static ResultViewModel ApplicationErrorMessage()
        {
            return new ResultViewModel
            {
                Message = "Unexpected application error, please try again",
                Success = false,
                Data = null
            };
        }

        public static ResultViewModel DomainErrorMessage(string message) 
        {
            return new ResultViewModel
            {
                Message = message,
                Success = false,
                Data = null
            };    
        }

        public static ResultViewModel DomainErrorMessage(string message, List<string> errors)
        {
            return new ResultViewModel
            {
                Message = message,
                Success = false,
                Data = errors
            };
        }

        public static ResultViewModel UnauthorizedErrorMessage()
        {
            return new ResultViewModel
            {
                Message = "The token is invalid, unauthorized access",
                Success = false,
                Data = null
            };
        }
    }
}
