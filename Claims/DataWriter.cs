using System;

namespace Claims;

public class DataWriter
{
     public static void WriteToCsv(
        Dictionary<string, Dictionary<int, List<double>>> cumulativeData,
        int minOriginYear, int maxDevYear,
        string outputPath)
    {
        using var writer = new StreamWriter(outputPath);

        writer.WriteLine($"{minOriginYear}, {maxDevYear - minOriginYear + 1}");

        foreach (var (product, triangles) in cumulativeData)
        {
            var row = new List<string> { product };

            foreach (var yearly in triangles.Values)
                row.AddRange(yearly.Select(v => v.ToString("F2")));

            writer.WriteLine(string.Join(", ", row));
        }

        Console.WriteLine($"✅ Output saved to: {outputPath}");
    }
}
