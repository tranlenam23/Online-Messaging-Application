using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatOnline.Object
{
    public static class Mediator
    {
        private static IDictionary<string, Action> registeredEvents = new Dictionary<string, Action>();

        public static void Subscribe(string message, Action action)
        {
            if (!registeredEvents.ContainsKey(message))
                registeredEvents[message] = action;
        }

        public static void Notify(string message)
        {
            if (registeredEvents.ContainsKey(message))
            {
                registeredEvents[message]?.Invoke();
            }
        }
    }

}
