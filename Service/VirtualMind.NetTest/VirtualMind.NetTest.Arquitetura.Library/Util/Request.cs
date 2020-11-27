using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VirtualMind.NetTest.Interfaces;

namespace VirtualMind.NetTest.Arquitetura.Library.Util
{
    public class Request : IRequest
    {

        public Request()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static HttpClient _client;

        public T Post<T>(string url, object data, string token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                if (_client.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                }

                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            HttpContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            return PostJson<T>(url, content);
        }

        public async Task<T> PostAsync<T>(string url, object data, string token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                if (_client.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                }

                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            HttpContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            return await PostJsonAsync<T>(url, content);
        }

        private async Task<T> PostJsonAsync<T>(string url, HttpContent content)
        {
            using (HttpResponseMessage response = await _client.PostAsync(url, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    return default(T);
                }
            }
        }

        private static T PostJson<T>(string url, HttpContent content)
        {
            using (HttpResponseMessage response = _client.PostAsync(url, content).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<T> GetAsync<T>(string url, string token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                if (_client.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                }

                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            using (HttpResponseMessage response = await _client.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    return default(T);
                }
            }
        }

        public T Get<T>(string url, string token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                if (_client.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                }

                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            using (HttpResponseMessage response = _client.GetAsync(url).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    return default(T);
                }
            }
        }
    }
}
