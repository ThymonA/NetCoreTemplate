namespace NetCoreTemplate.Providers.General
{
    using System.Collections.Generic;
    using System.Linq;

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Base;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;

    public sealed class MailQueueProvider : BaseProvider<MailQueue>, IMailQueueProvider
    {
        public MailQueueProvider(IPersistenceLayer persistence)
            : base(persistence)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Gives a list of all outgoing mails, these have not reached the maximum failure.
        /// </summary>
        /// <returns>List of <see cref="MailQueue" /> items.</returns>
        public IList<MailQueue> OutboundMails() => OutboundMails(false);

        /// <inheritdoc />
        /// <summary>
        /// Gives a list of all outgoing mails, <see cref="onlyFailure" /> filter the maximum number of failure.
        /// </summary>
        /// <param name="onlyFailure">Filter the maximum number of failure.</param>
        /// <returns>List of <see cref="MailQueue" /> items.</returns>
        public IList<MailQueue> OutboundMails(bool onlyFailure) => OutboundMailsQuery(onlyFailure).ToList();

        /// <inheritdoc />
        /// <summary>
        /// Gives a queryable of all outgoing mails, these have not reached the maximum failure.
        /// </summary>
        /// <returns>List of <see cref="MailQueue" /> items.</returns>
        public IQueryable<MailQueue> OutboundMailsQuery() => OutboundMailsQuery(false);

        /// <inheritdoc />
        /// <summary>
        /// Gives a queryable of all outgoing mails, <see cref="onlyFailure" /> filter the maximum number of failure.
        /// </summary>
        /// <param name="onlyFailure">Filter the maximum number of failure.</param>
        /// <returns>List of <see cref="MailQueue" /> items.</returns>
        public IQueryable<MailQueue> OutboundMailsQuery(bool onlyFailure)
        {
            if (onlyFailure)
            {
                return Persistence.Get<MailQueue>()
                    .Where(x => !x.SentOn.HasValue && x.NumberOfTimesFailed >= 3);
            }

            return Persistence.Get<MailQueue>()
                .Where(x => !x.SentOn.HasValue && x.NumberOfTimesFailed < 3);
        }
    }
}
