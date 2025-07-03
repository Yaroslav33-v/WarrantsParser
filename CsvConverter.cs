using System.Linq;

namespace WarrantyParser
{
    internal class CsvConverter
    {
        public static string ConvertToCsv(string line)
        {
            string[] cells = line.Split('|').Select(cell => cell.Trim()).ToArray();

            var escapedCells = cells.Select(cell =>
            {
                if (cell.Contains(",") || cell.Contains("\""))
                    return $"\"{cell.Replace("\"", "\"\"")}\"";
                else
                    return cell;
            });

            return string.Join(",", escapedCells);
        }
    }
}