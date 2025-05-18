namespace Rzx.Crm.Api.SignalR
{
    public class NotificationMessage
    {
        public Envelope Envelope{ get; set; }
        public Payload Payload { get; set; }
    }

    public class Envelope
    {
        public string topic { get; set; }
    }

    public class Payload
    {
        public string EventType { get; set; }
        public object Entity { get; set; }
    }
}
