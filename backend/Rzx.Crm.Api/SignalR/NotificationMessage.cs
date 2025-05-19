namespace Rzx.Crm.Api.SignalR
{
    public class NotificationMessage
    {
        private static ulong _messageId;

        public NotificationMessage(string topic, Payload payload)
        {
            Envelope = new Envelope
            {
                Id = Interlocked.Increment(ref _messageId),
                Topic = topic,
                Timestamp = DateTime.UtcNow
            };

            Payload = payload;
        }

        public Envelope Envelope{ get; }
        public Payload Payload { get;  }
    }

    public class Envelope
    {
        public string Topic { get; set; }
        public ulong Id { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class Payload
    {
        public Payload(string eventType, object data)
        {
            EventType = eventType;
            Data = data;
        }

        public string EventType { get;  }
        public object Data { get; }
    }
}
