namespace Core.Internal
{
    internal interface IJourneyRepository
    {
        IJourney Read();

        void Write(IJourney journey);
    }
}
