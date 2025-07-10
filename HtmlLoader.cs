using NLog;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WarrantyParser
{
    internal class HtmlLoader
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        readonly HttpClient сlient;
        readonly string url;

        public HtmlLoader()
        {
            сlient = new HttpClient();
            сlient.DefaultRequestHeaders.Add("User-Agent", "C# App");
            url = "https://www.wienerborse.at/en/warrants/?c10952-page={CurrentPage}&per-page=50";
        }

        public async Task<string> GetSourceByPage(int currentPage) // Получение данных со страницы
        {
            _logger.Trace($"Загрузка страницы {currentPage}");
            string currentUrl = url.Replace("{CurrentPage}", currentPage.ToString());

            try
            {
                HttpResponseMessage response = await сlient.GetAsync(currentUrl);
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _logger.Debug($"Успешно загружена страница {currentPage}");
                    return await response.Content.ReadAsStringAsync();
                }
                _logger.Warn($"Ошибка загрузки страницы {currentPage}. StatusCode: {response?.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ошибка при загрузке страницы {currentPage}");
            }
            return default;
        }
    }
}