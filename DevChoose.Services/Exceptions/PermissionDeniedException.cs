using System;
namespace DevChoose.Services.Exceptions
{
    public class PermissionDeniedException : ServiceException
    {
        public PermissionDeniedException()
            : base("You have no access to this information") { }
    }
}
