﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SDV701AdminClient
{
    public static class ServiceClient
    {

        private static string constructServiceURI(string command, Dictionary<string, Object> parameters)
        {
            string paramString = "";
            if (parameters != null)
            {
                paramString += "?";
                foreach (string s in parameters.Keys)
                {
                    paramString += $"{s}={parameters[s]}";
                    if (s != parameters.Keys.Last())
                        paramString += "&";
                }
            }
            return $"http://localhost:60064/api/Inventory/{command}{paramString}";
        }

        public async static Task<bool> getConnectionTestAsync()
        {
            bool result;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    result = JsonConvert.DeserializeObject<bool>(await client.GetStringAsync(constructServiceURI("getConnectionTest", null)));
                }
                catch (HttpRequestException)
                {
                    result = false;
                }
            }
            return result;
        }

        public async static Task<Dictionary<int, string>> getManufacturersNamesAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                return JsonConvert.DeserializeObject<Dictionary<int, string>>(await client.GetStringAsync(constructServiceURI("GetManufacturersNames", null)));
            }
            //return (from man in testData select man.name).ToList<string>();
        }

        public async static Task<ComputerManufacturer> GetManufacturerAsync(int code)
        {
            using (HttpClient client = new HttpClient())
            {
                return JsonConvert.DeserializeObject<ComputerManufacturer>(await client.GetStringAsync(constructServiceURI("GetManufacturer", new Dictionary<string, object>() { { "code", code } })));
            }
        }

        public async static Task<ComputerModel> GetComputerModelAsync(string name)
        {
            using (HttpClient client = new HttpClient())
            {
                return JsonConvert.DeserializeObject<ComputerModel>(await client.GetStringAsync(constructServiceURI("GetComputerModel", new Dictionary<string, object>() { { "name", name } })));
            }
        }

        public async static Task<ModelDetailPrexistingFieldData> GetModelDetailPrexistingFieldDataAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                return JsonConvert.DeserializeObject<ModelDetailPrexistingFieldData>(await client.GetStringAsync(constructServiceURI("GetModelDetailPrexistingFieldData", null)));
            }
        }

        public async static Task<DateTime> GetModifiedDateAsync(string modelName)
        {
            using (HttpClient client = new HttpClient())
            {
                return JsonConvert.DeserializeObject<DateTime>(await client.GetStringAsync(constructServiceURI("GetModifiedDate", new Dictionary<string, object>() { { "modelName", modelName } })));
            }
        }

        public async static Task<int> GetComputerModelStockQuantityAsync(string name)
        {
            using (HttpClient client = new HttpClient())
            {
                return JsonConvert.DeserializeObject<int>(await client.GetStringAsync(constructServiceURI("GetComputerModelStockQuantity", new Dictionary<string, object>() { { "name", name } })));
            }
        }

        private async static Task<string> insertOrUpdateAsync<TItem>(TItem item, string url, string request)
        {
            using (HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod(request), url))
            using (requestMessage.Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.Default, "application/json"))
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(requestMessage);
                return JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
            }
        }

        public async static Task<string> PostComputerModelAsync(ComputerModel computerModel)
        {

            return await insertOrUpdateAsync<ComputerModel>(computerModel, constructServiceURI("PostComputerModel", new Dictionary<string, object>() { { "computerModel", computerModel } }), "POST");
        }

        public async static Task<string> PutComputerModelAsync(ComputerModel computerModel)
        {
            return await insertOrUpdateAsync<ComputerModel>(computerModel, constructServiceURI("PutComputerModel", new Dictionary<string, object>() { { "computerModel", computerModel } }), "PUT");
        }

        public async static Task<string> DeleteComputerModelAsync(string modelName)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.DeleteAsync(constructServiceURI("DeleteComputerModel", new Dictionary<string, object>() { { "modelName", modelName } }));
                return JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
            }
        }

        public async static Task<List<PurchaseOrder>> GetPurchaseOrdersAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                return JsonConvert.DeserializeObject<List<PurchaseOrder>>(await client.GetStringAsync(constructServiceURI("GetPurchaseOrders", null)));
            }
        }

        public async static Task<string> DeletePurchaseOrderAsync(int orderNumber)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.DeleteAsync(constructServiceURI("DeletePurchaseOrder", new Dictionary<string, object>() { { "orderNumber", orderNumber } }));
                return JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
            }
        }

    }
}
