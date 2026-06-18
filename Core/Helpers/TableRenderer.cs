using System.Reflection;

namespace SkyFlow.Core.Helpers
{
    public static class TableRenderer
    {
        public static void Render<T>(List<T> items)
        {
            if (items == null || items.Count == 0)
            {
                Console.WriteLine("No records found.");
                return;
            }

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var headers = new List<string>();
            var columnWidths = new List<int>();

            // Get headers and initial widths
            foreach (var prop in properties)
            {
                string header = prop.Name;
                headers.Add(header);
                columnWidths.Add(header.Length);
            }

            // Calculate max width for each column
            foreach (var item in items)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var value = properties[i].GetValue(item)?.ToString() ?? "";
                    if (value.Length > columnWidths[i])
                        columnWidths[i] = value.Length;
                }
            }

            // Print top border
            PrintBorder(columnWidths);

            // Print Header
            for (int i = 0; i < headers.Count; i++)
            {
                Console.Write($"| {headers[i].PadRight(columnWidths[i])} ");
            }
            Console.WriteLine("|");

            PrintBorder(columnWidths);

            // Print Data Rows
            foreach (var item in items)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var value = properties[i].GetValue(item)?.ToString() ?? "";
                    Console.Write($"| {value.PadRight(columnWidths[i])} ");
                }
                Console.WriteLine("|");
            }

            PrintBorder(columnWidths);
        }

        private static void PrintBorder(List<int> widths)
        {
            Console.Write("+");
            foreach (var width in widths)
            {
                Console.Write(new string('-', width + 2) + "+");
            }
            Console.WriteLine();
        }
    }
}