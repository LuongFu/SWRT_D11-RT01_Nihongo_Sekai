namespace JapaneseLearningPlatform.Data.ViewModels.Chat
{
    public class SendPrivateMessageRequestVM
    {
        public int ClassroomId { get; set; }
        public string TargetUserId { get; set; }
        public string Message { get; set; }
    }
}
