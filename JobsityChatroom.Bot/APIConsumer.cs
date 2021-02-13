using JobsityChatroom.Bot.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JobsityChatroom.Bot
{
    public static class APIConsumer
    {
        private static HttpClient httpClient { get;  set; }

        /// <summary>
        /// Calls the API to get information about the stock wanted.
        /// </summary>
        /// <param name="stockCode">The desired stock's code</param>
        /// <returns>A message informing the value of the stock</returns>
        public static async Task<string> GetStockInformation(string stockCode)
        {
            try
            {
                var stockInfo = await APIConsumer.CallApi(stockCode);

                return stockInfo.Code + " quote is $" + stockInfo.Value + " per share";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public static HttpClient GetClient()
        {
            if(APIConsumer.httpClient == null)
            {
                APIConsumer.httpClient = new HttpClient();
            }
            
            return APIConsumer.httpClient;

        }

        private static async Task<StockDTO> CallApi(string stockCode)
        {
            var url = "https://stooq.com/q/l/?s=" + stockCode + "&f=sd2t2ohlcv&h&e=csv";

            var response = await APIConsumer.GetClient().GetAsync(url);

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var csvString = await response.Content.ReadAsStringAsync();
                return APIConsumer.ProcessCSV(csvString);
            }
            else
            {
                throw new FileNotFoundException("I couldn't find the information you need, please try again later.");
            }
        }

        private static StockDTO ProcessCSV(string csvAsString)
        {
            var stockInfo = new StockDTO();
            int stockCodeIndex, stockValueIndex = 0;
            float stockValue;

            // Split the CSV on each end of line 
            var stringsArray = csvAsString.Split("\n");

            // If the stock couldn't be found, there's an exception
            if (stringsArray.Length <= 1)
            {
                throw new ArgumentException("I think that stock code doesn't exists, please try again with a valid stock code.");
            }


            // Find the indexes for the needed information inside the CSV using the headers as reference
            stockCodeIndex = Array.IndexOf(stringsArray[0].Split(','), "Symbol");
            stockValueIndex = Array.IndexOf(stringsArray[0].Split(','), "Close");

            // If there's any index that couldn't be found, there's an exception
            if(stockCodeIndex == -1 || stockValueIndex == -1)
            {
                throw new InvalidOperationException("I couldn't process the information you need, please try again later.");
            }

            // If we can't parse the value, there's an exception
            if (!float.TryParse(stringsArray[1].Split(',')[stockValueIndex].Replace('.', ','), out stockValue))
            {
                throw new InvalidCastException("There's a problem with the value of that particular stock, please try again later.");
            }
            stockInfo.Code = stringsArray[1].Split(',')[stockCodeIndex];
            stockInfo.Value = stockValue;

            return stockInfo;
        }
    }
}
