using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WienerParserAttempt
{
    internal class CsvConverter
    {
        public static string ConvertToCsv(string line)
        {
            string[] cells = line.Split('|').Select(cell => cell.Trim()).ToArray();

            // Экранируем кавычки и запятые (если они есть внутри данных)
            var escapedCells = cells.Select(cell =>
            {
                if (cell.Contains(",") || cell.Contains("\""))
                    return $"\"{cell.Replace("\"", "\"\"")}\""; // Экранирование кавычек
                else
                    return cell;
            });

            // Объединяем в CSV-строку и записываем
            return string.Join(",", escapedCells);
        }
    }
}
