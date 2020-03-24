using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Filters;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Events
{
    public interface IEventsApi
    {
        /// <summary>Gets events.</summary>
        /// <param name="pagination">Pagination.</param>
        /// <param name="filter">Filters for events.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>List of events matching passed filter criteria.</returns>
        Task<ListPaginated<EventDTO>> GetAll(Pagination pagination, FilterEvents filter = null, Sort sort = null);
    }
}
