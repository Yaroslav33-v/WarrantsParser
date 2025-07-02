using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WienerParserAttempt
{
    internal class HtmlLoader
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        readonly HttpClient client;
        readonly string url;

        public HtmlLoader()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");
            url = "https://www.wienerborse.at/en/warrants/?c10952-page={CurrentPage}&per-page=50";
        }

        public async Task<string> GetSourceByPage(int CurrentPage)
        {
            logger.Trace($"Загрузка страницы {CurrentPage}");
            string CurrentUrl = url.Replace("{CurrentPage}", CurrentPage.ToString());

            try
            {
                HttpResponseMessage response = await client.GetAsync(CurrentUrl);
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    logger.Debug($"Успешно загружена страница {CurrentPage}");
                    return await response.Content.ReadAsStringAsync();
                }
                logger.Warn($"Ошибка загрузки страницы {CurrentPage}. StatusCode: {response?.StatusCode}");
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Ошибка при загрузке страницы {CurrentPage}");
            }
            return default;
        }
    }
}
