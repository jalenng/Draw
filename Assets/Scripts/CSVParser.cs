using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

public class CSVParser
{
    public static List<string[]> ParseCsv(string filePath)
    {
        List<string[]> records = new List<string[]>();

        using (var reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] fields = ParseCsvLine(line);
                records.Add(fields);
            }
        }

        return records;
    }

    public static string[] ParseCsvLine(string line)
    {
        List<string> fields = new List<string>();

        // Use regular expression to match fields
        var regex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        string[] matches = regex.Split(line);

        foreach (var match in matches)
        {
            string field = match.Trim();

            // Remove surrounding double quotes if present
            if (field.StartsWith("\"") && field.EndsWith("\""))
            {
                field = field.Substring(1, field.Length - 2);
                // Handle double quotes within double-quoted fields
                field = field.Replace("\"\"", "\"");
            }

            fields.Add(field);
        }

        return fields.ToArray();
    }
}
