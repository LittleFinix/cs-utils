namespace Finix.CsUtils
{
    [System.Serializable]
    public class CongruityException : System.Exception
    {
        public CongruityException() { }
        public CongruityException(string message) : base(message) { }
        public CongruityException(string message, System.Exception inner) : base(message, inner) { }
        protected CongruityException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
