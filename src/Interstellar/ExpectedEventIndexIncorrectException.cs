namespace Interstellar
{
    using System;

    public class ExpectedEventIndexIncorrectException : Exception
    {
        public ExpectedEventIndexIncorrectException(string streamId, long currentEventIndex, long startIndex)
            : base($"Expected event with index {currentEventIndex} but got {startIndex} when writing to {streamId}")
        {
        }

        public ExpectedEventIndexIncorrectException(string streamId,  long startIndex)
            : base($"Expected a different event index to {startIndex} when writing to {streamId}")
        {
        }
    }
}