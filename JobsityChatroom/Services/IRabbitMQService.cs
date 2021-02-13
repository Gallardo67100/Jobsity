namespace JobsityChatroom.Services
{
    public interface IRabbitMQService
    {
        public void SendToQueue(string message);
    }
}
