using AngleSharp.Browser;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Text;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WienerParserAttempt
{
    internal class ParserWorker
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        WieneParser parser;
        readonly HtmlLoader loader = new HtmlLoader();

        public WieneParser Parser
        {
            get { return parser; }
            set { parser = value; }
        }
        public ParserWorker(WieneParser parser)
        {
            this.parser = parser;
        }
        public async Task Worker()
        {
            logger.Info("Начало работы парсера");
            string csvFilePath = $"output_{DateTime.Now:yyyyMMdd}.csv";

            try
            {
                using (var sw = new StreamWriter(csvFilePath, false, Encoding.UTF8))
                {
                    logger.Debug("Загрузка первой страницы для заголовков");
                    string source = await loader.GetSourceByPage(1);
                    IHtmlDocument document = await new HtmlParser().ParseDocumentAsync(source);

                    // Заголовки
                    var headers = parser.ParseNames(document);
                    logger.Debug($"Найдено {headers.Length} заголовков");

                    foreach (string line in headers)
                    {
                        sw.WriteLine(CsvConverter.ConvertToCsv(line));
                    }

                    // Данные
                    int pageCount = parser.FindLastPageNumber(document);
                    logger.Info($"Всего страниц для обработки: {pageCount}");

                    for (int i = 1; i <= pageCount; i++)
                    {
                        logger.Trace($"Обработка страницы {i}/{pageCount}");
                        source = await loader.GetSourceByPage(i);
                        document = await new HtmlParser().ParseDocumentAsync(source);
                        var rows = parser.Parse(document);

                        logger.Debug($"На странице {i} найдено {rows.Length} строк");

                        foreach (string line in rows)
                        {
                            sw.WriteLine(CsvConverter.ConvertToCsv(line));
                        }
                    }
                }
                logger.Info($"Данные успешно сохранены в {Path.GetFullPath(csvFilePath)}");
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Критическая ошибка в работе парсера");
                throw;
            }
        }
        public async Task Start()
        {
            await Worker();
        }
    }
}
