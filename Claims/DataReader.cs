using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Claims;

public class DataReader
{
    public List<ClaimRecord>? Records { get; private set; }
    public int MinOriginYear { get; set; }
    public int MaxDevYear { get; set; }

    public void ReadClaims(string filePath) {
        CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture) {
            TrimOptions = TrimOptions.Trim,
            HeaderValidated = null,
            MissingFieldFound = null
        };

        using StreamReader reader = new(filePath);
        using CsvReader csv = new(reader, config);

        Records = csv.GetRecords<ClaimRecord>().ToList();

        // print records
        foreach (var record in Records)
        {
            Console.WriteLine($"{record.Product}, {record.OriginYear}, {record.DevelopmentYear}, {record.IncrementalValue}");
        }
    }

    public void Stats() {
        if (Records == null || Records.Count == 0)
        {
            throw new ArgumentException("Records cannot be null or empty.");
        }

        MinOriginYear = Records.Min(r => r.OriginYear);
        MaxDevYear = Records.Max(r => r.DevelopmentYear);
    }
}
