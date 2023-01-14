using System;

namespace Interstellar.Domain
{

    public class StatusChangeOptions
    {
        private Action stateToBecome;

        public void Become(Action toBecome)
        {
            stateToBecome = toBecome;
        }
    }
}