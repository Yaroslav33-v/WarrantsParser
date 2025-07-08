using System.Linq;

namespace WarrantyParser
{
    internal class CsvConverter
    {
        public static string ConvertToCsv(string line)
        {
            string[] cells = line.Split('|')
                .Select(cell =>
                {
                    string trimmedCell = cell.Trim();

                    string escapedCell = trimmedCell.Replace("\"", "\"\"");

                    return $"\"{escapedCell}\""; // Добавление кавычек для корректного отображения данных
                })
                .ToArray();

            // Соединение ячеек через запятую
            return string.Join(",", cells);
        }
    }
}