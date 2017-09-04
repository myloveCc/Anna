namespace Kitty.Errors
{
    public abstract class Error 
    {
        protected Error(string message, KittyErrorCode code)
        {
            Message = message;
            Code = code;
        }

        public string Message { get; private set; }
        public KittyErrorCode Code { get; private set; }
    }
}