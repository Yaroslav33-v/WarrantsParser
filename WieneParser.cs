using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WienerParserAttempt
{
    internal class WieneParser
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public string[] Parse(IHtmlDocument document)
        {
            List<string> list = new List<string>();
            var tables = document.QuerySelectorAll("tbody");

            foreach (var table in tables)
            {
                var rows = table.QuerySelectorAll("tr");

                foreach (var row in rows)
                {
                    // Получаем все ячейки (тег <td> или <th>)
                    var cells = row.QuerySelectorAll("td");

                    // Собираем текст из ячеек, разделяя пробелами
                    string rowText = string.Join(" | ", cells.Select(cell => cell.TextContent.Trim()));

                    list.Add(rowText);
                }
            }
            return list.ToArray();
        }
        public string[] ParseNames(IHtmlDocument document)
        {
            List<string> list = new List<string>();
            var tables = document.QuerySelectorAll("thead");
            foreach (var table in tables)
            {
                var headers = table.QuerySelectorAll("th");
                string rowText = string.Join(" | ", headers.Select(header => header.TextContent.Trim()));
                list.Add(rowText);
            }
            return list.ToArray();
        }
        public int FindLastPageNumber(IHtmlDocument document)
        {
            try
            {
                var pageLinks = document.QuerySelectorAll("li")
                    .Select(x => x.TextContent.Trim())
                    .Where(text => int.TryParse(text, out _))
                    .Select(int.Parse)
                    .ToList();

                int lastPage = pageLinks.Any() ? pageLinks.Max() : 1;
                logger.Debug($"Найдено страниц: {lastPage}");
                return lastPage;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка при определении количества страниц");
                return 1;
            }
        }
    }
}
