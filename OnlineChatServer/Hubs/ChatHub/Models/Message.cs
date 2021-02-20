namespace OnlineChatServer.Hubs.ChatHub.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string RecipientID { get; set; }
        public string SenderID { get; set; }
        public string FullNameSender { get; set; }
        public string DispatchTime { get; set; }
        public string TextMessage { get; set; }
    }
}