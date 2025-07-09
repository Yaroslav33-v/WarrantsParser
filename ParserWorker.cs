using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using NLog;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyParser
{
    internal class ParserWorker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly HtmlLoader _loader = new HtmlLoader();
        private WieneParser _parser;

        public WieneParser Parser
        {
            get => _parser;
            set => _parser = value;
        }

        public ParserWorker(WieneParser parser)
        {
            _parser = parser;
        }

        public async Task Worker() // Метод, записывающий полученные данные в файл 
        {
            Logger.Info("Начало работы парсера");
            string csvFilePath = $"output_{DateTime.Now:yyyyMMdd}.csv";

            try
            {
                using (var sw = new StreamWriter(csvFilePath, false, Encoding.UTF8))
                {
                    Logger.Debug("Загрузка первой страницы для заголовков");
                    string source = await _loader.GetSourceByPage(1);
                    IHtmlDocument document = await new HtmlParser().ParseDocumentAsync(source);

                    var headers = _parser.ParseNames(document);
                    Logger.Debug($"Найдено {headers.Length} заголовков");

                    foreach (string line in headers)
                    {
                        sw.WriteLine(line);
                    }

                    int pageCount = _parser.FindLastPageNumber(document);
                    Logger.Info($"Всего страниц для обработки: {pageCount}");

                    for (int i = 1; i <= pageCount; i++)
                    {
                        Logger.Trace($"Обработка страницы {i}/{pageCount}");
                        source = await _loader.GetSourceByPage(i);
                        document = await new HtmlParser().ParseDocumentAsync(source);
                        var rows = _parser.Parse(document);

                        Logger.Debug($"На странице {i} найдено {rows.Length} строк");

                        foreach (string line in rows)
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                Logger.Info($"Данные успешно сохранены в {Path.GetFullPath(csvFilePath)}");
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "Критическая ошибка в работе парсера");
            }
            Console.WriteLine($"Данные успешно сохранены в {Path.GetFullPath(csvFilePath)}");
        }

        public async Task Start()
        {
            await Worker();
        }
    }
}