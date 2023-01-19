using System.Collections.Generic;
using System.Threading;

namespace Interstellar
{
    public class MessageContext
    {
        private static readonly AsyncLocal<MessageContext> Storage = new AsyncLocal<MessageContext>();

        public static MessageContext Current => Storage.Value ?? (Storage.Value = new MessageContext());

        public IDictionary<string, object> Headers { get; set; } = null!;
    }
}