using NLog;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WarrantyParser
{
    internal class HtmlLoader
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        readonly HttpClient Client;
        readonly string Url;

        public HtmlLoader()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Add("User-Agent", "C# App");
            Url = "https://www.wienerborse.at/en/warrants/?c10952-page={CurrentPage}&per-page=50";
        }

        public async Task<string> GetSourceByPage(int currentPage)
        {
            Logger.Trace($"Загрузка страницы {currentPage}");
            string currentUrl = Url.Replace("{CurrentPage}", currentPage.ToString());

            try
            {
                HttpResponseMessage response = await Client.GetAsync(currentUrl);
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Logger.Debug($"Успешно загружена страница {currentPage}");
                    return await response.Content.ReadAsStringAsync();
                }
                Logger.Warn($"Ошибка загрузки страницы {currentPage}. StatusCode: {response?.StatusCode}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Ошибка при загрузке страницы {currentPage}");
            }
            return default;
        }
    }
}