using MangoPay.SDK.Entities.GET;
using MangoPay.SDK.Entities.POST;
using System.Threading.Tasks;

namespace MangoPay.SDK.APIs.PayOuts
{
    public interface IPayOutsApi
    {
        /// <summary>Creates new PayOut object.</summary>
        /// <param name="payOut">The PayOut object to be created.</param>
        /// <returns>Created PayOut object returned from API.</returns>
        Task<PayOutBankWireDTO> CreateBankWire(PayOutBankWirePostDTO payOut);

        /// <summary>Creates new PayOut object.</summary>
        /// <param name="idempotencyKey">Idempotency key for this request.</param>
        /// <param name="payOut">The PayOut object to be created.</param>
        /// <returns>Created PayOut object returned from API.</returns>
        Task<PayOutBankWireDTO> CreateBankWire(string idempotencyKey, PayOutBankWirePostDTO payOut);

        /// <summary>Gets PayOut entity by its identifier.</summary>
        /// <param name="payOutId">PayOut identifier.</param>
        /// <returns>PayOut instance returned from API.</returns>
        Task<PayOutDTO> Get(string payOutId);

        /// <summary>Gets PayOut entity by its identifier.</summary>
        /// <param name="payOutId">PayOutBankWire identifier.</param>
        /// <returns>PayOutBankWire instance returned from API.</returns>
        Task<PayOutBankWireDTO> GetBankWire(string payOutId);
    }
}
