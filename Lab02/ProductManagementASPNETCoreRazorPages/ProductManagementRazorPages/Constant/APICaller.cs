using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagementRazorPages.Constant
{
    public static class APICaller
    {
        public static string API_ADDRESS => ConstantVariables.APIEndPoint;
        public static string API_ADDRESS_ODATA => ConstantVariables.APIEndPoint.Replace("api/", "odata/");

        public static async Task<T> GetAsync<T>(string path, string jwtToken = "", bool isOdata = false)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(isOdata ? API_ADDRESS_ODATA : API_ADDRESS);

                // Add JWT token to the request headers if provided
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwtToken);
                }

                using (var response = await client.GetAsync(path))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<T>(data);
                        return result;
                    }
                    else
                    {
                        throw new Exception("API call failed with status code: " + response.StatusCode);
                    }
                }
            }
        }

        public static async Task<T> PostAsync<U, T>(string path, U bodyObject, string jwtToken = "")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(API_ADDRESS);

                // Add JWT token to the request headers if provided
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwtToken);
                }

                var json = JsonConvert.SerializeObject(bodyObject);
                var body = new StringContent(json, Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync(path, body))
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<T>(data);
                        return result;
                    }
                    else
                    {
                        throw new Exception("API call failed with status code: " + response.StatusCode);
                    }
            }
        }


        public static async Task<T> PutAsync<T, U>(string path, U bodyObject, string jwtToken = "")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(API_ADDRESS);

                // Add JWT token to the request headers if provided
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwtToken);
                }

                var json = JsonConvert.SerializeObject(bodyObject);
                var body = new StringContent(json, Encoding.UTF8, "application/json");

                using (var response = await client.PutAsync(path, body))
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<T>(data);
                        return result;
                    }
                    else
                    {
                        throw new Exception("API call failed with status code: " + response.StatusCode);
                    }
            }
        }

        public static async Task<bool> DeleteAsync(string path, string jwtToken = "")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(API_ADDRESS);

                // Add JWT token to the request headers if provided
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwtToken);
                }

                using (var response = await client.DeleteAsync(path))
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception("API call failed with status code: " + response.StatusCode);
                    }
            }
        }
    }
}
