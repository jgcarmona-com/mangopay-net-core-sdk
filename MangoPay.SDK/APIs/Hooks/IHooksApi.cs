using MangoPay.SDK.Core;
using MangoPay.SDK.Entities;
using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using MangoPay.SDK.Entities.PUT;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.Hooks
{
    public interface IHooksApi
    { /// <summary>Creates new hook.</summary>
      /// <param name="hook">Hook instance to be created.</param>
      /// <returns>Hook instance returned from API.</returns>
        Task<HookDTO> Create(HookPostDTO hook);

        /// <summary>Creates new hook.</summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="hook">Hook instance to be created.</param>
        /// <returns>Hook instance returned from API.</returns>
        Task<HookDTO> Create(string idempotencyKey, HookPostDTO hook);

        /// <summary>Gets hook.</summary>
        /// <param name="hookId">Hook identifier.</param>
        /// <returns>Hook instance returned from API.</returns>
        Task<HookDTO> Get(string hookId);

        /// <summary>Saves a hook.</summary>
        /// <param name="hook">Hook instance to save.</param>
        /// <param name="hookId">Hook identifier.</param>
        /// <returns>Hook instance returned from API.</returns>
        Task<HookDTO> Update(HookPutDTO hook, string hookId);

        /// <summary>Gets all hooks.</summary>
        /// <param name="pagination">Pagination.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>List of Hook instances returned from API.</returns>
        Task<ListPaginated<HookDTO>> GetAll(Pagination pagination, Sort sort = null);

        /// <summary>Gets all hooks.</summary>
        /// <returns>List of Hook instances returned from API.</returns>
        Task<ListPaginated<HookDTO>> GetAll();
    }
}