using System;
namespace DevChoose.Services.Exceptions
{
    public class InvalidAuthDataException : ServiceException
    {
        public InvalidAuthDataException()
            : base("No user with such data") { }
    }
}
