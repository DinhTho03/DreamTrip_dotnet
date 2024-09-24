namespace TravelItineraryProject.Exceptions
{
    public class TravelExceptions : Exception
    {
        public TravelExceptions() { }

        public TravelExceptions(string message)
            : base(message)
        {
        }

        public TravelExceptions(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
