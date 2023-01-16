using System;

namespace Interstellar
{
    public class StatusChangeOptions : IStatusChangeOptions
    {
        private Action stateToBecome = null!;

        public void Become(Action toBecome)
        {
            stateToBecome = toBecome;
        }

        public string? RunBecome()
        {
            string? name = stateToBecome.Method.Name;
            stateToBecome();
            return name;
        }
    }
}