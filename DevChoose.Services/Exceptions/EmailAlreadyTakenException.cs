using System;
namespace DevChoose.Services.Exceptions
{
    public class EmailAlreadyTakenException : ServiceException
    {
        public EmailAlreadyTakenException()
            : base("User with such email already exists") { }
    }
}
