using GenesisCoreInterface.Customer.Models;
using GenesisCoreInterface.Customer.RequestResponse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WingsManager.ExternalService.Genesis
{
    public interface IGenesisCoreApi
    {
        Task<CompanyListGet_RS> GetCustomerListAsync(CompanyListGet_RQ request, CancellationToken cancellationToken);
    }
    public class GenesisCoreApi : IGenesisCoreApi
    {
        private readonly HttpClient _httpClient = null;

        public GenesisCoreApi(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<CompanyListGet_RS> GetCustomerListAsync(CompanyListGet_RQ request, CancellationToken cancellationToken)
        {
            CompanyListGet_RS response = null;

            using (StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"))
            using (HttpResponseMessage responseMessage = await this._httpClient.PostAsync("Customer/GetCompanies", content, cancellationToken))
            {
                string contentString = await responseMessage.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<CompanyListGet_RS>(contentString);
            }
            return response;
        }
    }
}
