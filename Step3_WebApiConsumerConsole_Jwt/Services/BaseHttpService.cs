using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Step3_WebApiConsumerConsole_Jwt.Services
{
    public abstract class BaseHttpService
    {
        protected async Task<TResult> SendRequestAsync<TResult>(
               Uri url,
               HttpMethod httpMethod = null,
               string jwtEncryptedToken = null) => await SendRequestAsync<object, TResult>(url, httpMethod, null, jwtEncryptedToken);


        protected async Task<TResult> SendRequestAsync<T, TResult>(
               Uri url,
               HttpMethod httpMethod = null,
               T requestData = default(T),
               string jwtEncryptedToken = null)
        {
           var result = default(TResult);

            // Default to GET
            var method = httpMethod ?? HttpMethod.Get;

            // Serialize request data, if any
            var data = requestData == null
                ? null
                : JsonConvert.SerializeObject(requestData);

            //Start building the Request
            using (var request = new HttpRequestMessage(method, url))
            {
                // Add request data to request, if any
                if (data != null)
                    request.Content = new StringContent(data, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    //Add Jwt token if any
                    if (jwtEncryptedToken != null)
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtEncryptedToken);

                    //Send and wait for response
                    using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead))
                    {
                        var content = response.Content == null
                            ? null
                            : await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode && content != null)
                        {
                            result = JsonConvert.DeserializeObject<TResult>(content);
                        }
                    }
                }
            }

            return result;
        }
    }
}
