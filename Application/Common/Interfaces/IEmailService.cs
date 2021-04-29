namespace Application.Common.Interfaces
{
    public interface IEmailService
    {
        public void SendEmail(string destinationMail, string name, string subject, string emailTitle,
            string emailContent);
    }
}