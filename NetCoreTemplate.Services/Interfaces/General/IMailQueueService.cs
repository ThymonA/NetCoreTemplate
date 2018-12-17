namespace NetCoreTemplate.Services.Interfaces.General
{
    using NetCoreTemplate.DAL.Models.General;

    public interface IMailQueueService : IBaseService<MailQueue>
    {
        /// <summary>
        /// Mark the mail as done, with current DateTime.
        /// </summary>
        /// <param name="mailQueue"><see cref="MailQueue"/> item</param>
        void MarkAsDone(MailQueue mailQueue);

        /// <summary>
        /// Mark the mail as failed, increase number of time failed.
        /// </summary>
        /// <param name="mailQueue"><see cref="MailQueue"/> item</param>
        void MarkAsFailed(MailQueue mailQueue);

        /// <summary>
        /// Add a new <see cref="MailQueue"/> item to the queue.
        /// </summary>
        /// <param name="mailQueue"><see cref="MailQueue"/> item</param>
        void AddNew(MailQueue mailQueue);

        /// <summary>
        /// Add a new <see cref="MailQueue"/> item to the queue.
        /// </summary>
        /// <param name="to">Email</param>
        /// <param name="subject">Subject</param>
        /// <param name="content">Body</param>
        void AddNew(string to, string subject, string content);
    }
}
