namespace NetCoreTemplate.Services.General
{
    using System;

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Services.Base;
    using NetCoreTemplate.Services.Interfaces.General;
    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;

    public sealed class MailQueueService : BaseService<MailQueue>, IMailQueueService
    {
        public MailQueueService(IPersistenceLayer persistence)
            : base(persistence)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Mark the mail as done, with current DateTime.
        /// </summary>
        /// <param name="mailQueue"><see cref="T:NetCoreTemplate.DAL.Models.General.MailQueue" /> item</param>
        public void MarkAsDone(MailQueue mailQueue)
        {
            mailQueue.SentOn = DateTime.Now;
            
            base.AddOrUpdate(mailQueue);
        }

        /// <inheritdoc />
        /// <summary>
        /// Mark the mail as failed, increase number of time failed.
        /// </summary>
        /// <param name="mailQueue"><see cref="T:NetCoreTemplate.DAL.Models.General.MailQueue" /> item</param>
        public void MarkAsFailed(MailQueue mailQueue)
        {
            mailQueue.SentOn = null;
            mailQueue.NumberOfTimesFailed += 1;

            base.AddOrUpdate(mailQueue);
        }

        /// <inheritdoc />
        /// <summary>
        /// Add a new <see cref="MailQueue" /> item to the queue.
        /// </summary>
        /// <param name="mailQueue"><see cref="MailQueue" /> item</param>
        public void AddNew(MailQueue mailQueue)
        {
            mailQueue.NumberOfTimesFailed = default(int);
            mailQueue.AddedOn = DateTime.Now;
            mailQueue.SentOn = null;

            base.Add(mailQueue);
        }

        /// <inheritdoc />
        /// <summary>
        /// Add a new <see cref="MailQueue" /> item to the queue.
        /// </summary>
        /// <param name="to">Email</param>
        /// <param name="subject">Subject</param>
        /// <param name="content">Body</param>
        public void AddNew(string to, string subject, string content)
        {
            var mailQueue = new MailQueue
            {
                To = to,
                Subject = subject,
                Content = content,
                AddedOn = DateTime.Now,
                SentOn = null,
                NumberOfTimesFailed = default(int)
            };

            base.Add(mailQueue);
        }
    }
}
