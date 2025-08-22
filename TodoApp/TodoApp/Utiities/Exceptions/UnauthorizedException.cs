namespace TodoApp.Utiities.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("<h3>Not Authorized</h3> </br>Session Expired.\nTry logging in again.") { }

        public UnauthorizedException(string message) : base(message) { }


        public UnauthorizedException(string message,
                    Exception innerException) :
                    base(message, innerException)
        { }
    }
}
