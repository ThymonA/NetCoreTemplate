namespace NetCoreTemplate.Providers.Interfaces.General
{
    using System.Collections.Generic;
    using System.Linq;

    using NetCoreTemplate.DAL.Models.General;

    public interface IMailQueueProvider : IBaseProvider<MailQueue>
    {
        /// <summary>
        /// Gives a list of all outgoing mails, these have not reached the maximum failure.
        /// </summary>
        /// <returns>List of <see cref="MailQueue"/> items.</returns>
        IList<MailQueue> OutboundMails();

        /// <summary>
        /// Gives a list of all outgoing mails, <see cref="onlyFailure"/> filter the maximum number of failure.
        /// </summary>
        /// <param name="onlyFailure">Filter the maximum number of failure.</param>
        /// <returns>List of <see cref="MailQueue"/> items.</returns>
        IList<MailQueue> OutboundMails(bool onlyFailure);

        /// <summary>
        /// Gives a queryable of all outgoing mails, these have not reached the maximum failure.
        /// </summary>
        /// <returns>List of <see cref="MailQueue"/> items.</returns>
        IQueryable<MailQueue> OutboundMailsQuery();

        /// <summary>
        /// Gives a queryable of all outgoing mails, <see cref="onlyFailure"/> filter the maximum number of failure.
        /// </summary>
        /// <param name="onlyFailure">Filter the maximum number of failure.</param>
        /// <returns>List of <see cref="MailQueue"/> items.</returns>
        IQueryable<MailQueue> OutboundMailsQuery(bool onlyFailure);
    }
}
