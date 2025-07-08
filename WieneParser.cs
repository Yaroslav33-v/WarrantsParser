using AngleSharp.Html.Dom;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WarrantyParser
{
    internal class WieneParser
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public string[] Parse(IHtmlDocument document) // Получение данных из таблицы
        {
            List<string> list = new List<string>();
            var tables = document.QuerySelectorAll("tbody");

            foreach (var table in tables)
            {
                var rows = table.QuerySelectorAll("tr");

                foreach (var row in rows)
                {
                    var cells = row.QuerySelectorAll("td");
                    string rowText = string.Join(" | ", cells.Select(cell => cell.TextContent.Trim()));
                    list.Add(rowText);
                }
            }
            return list.ToArray();
        }

        public string[] ParseNames(IHtmlDocument document) // Получение имён из таблицы
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

        public int FindLastPageNumber(IHtmlDocument document) // Нахождение номера последней страницы
        {
            try
            {
                var pageLinks = document.QuerySelectorAll("li")
                    .Select(x => x.TextContent.Trim())
                    .Where(text => int.TryParse(text, out _))
                    .Select(int.Parse)
                    .ToList();

                int lastPage = pageLinks.Any() ? pageLinks.Max() : 1;
                Logger.Debug($"Найдено страниц: {lastPage}");
                return lastPage;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при определении количества страниц");
                return 1;
            }
        }
    }
}