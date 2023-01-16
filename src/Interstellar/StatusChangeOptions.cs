using System;

namespace Interstellar
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