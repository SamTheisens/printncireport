namespace PrintNCIAgent
{
    /// <summary>
    /// Interaction logic for AlarmScreen.xaml
    /// </summary>
    public class NotifyObject
    {
        public NotifyObject(string message, string title)
        {
            Message = message;
            Title = title;
        }

        public string Title { get; set; }

        public string Message { get; set; }
    }

}
