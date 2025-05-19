using MediatR;
using Rzx.Crm.Core.Models;

namespace Rzx.Crm.Core.Events
{
    public class CustomerStatsNotification : INotification
    {
        public CustomerStats Stats {get;}

        public CustomerStatsNotification(CustomerStats customerStats)
        {
            Stats = customerStats;
        }
    }
}
