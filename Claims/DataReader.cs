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
        Records = new List<ClaimRecord>();

        CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture) {
            TrimOptions = TrimOptions.Trim,
            HeaderValidated = null,
            MissingFieldFound = null
        };

        HashSet<(string, int, int)> seenRecords = new();

        using StreamReader reader = new(filePath);
        using (CsvReader csv = new(reader, config)) {
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var incrementalValue = csv.GetField<double>("IncrementalValue");
                var originYear = csv.GetField<int>("OriginYear");
                var developmentYear = csv.GetField<int>("DevelopmentYear");
                var product = csv.GetField<string>("Product");

                if (incrementalValue < 0 || double.IsNaN(incrementalValue)) {
                    Console.WriteLine(
                        $"Warning: Invalid incremental value '{incrementalValue}' for product '{product}' in year {developmentYear}. Setting to 0.");
                    incrementalValue = 0;
                }

                if (developmentYear < originYear) {
                    Console.WriteLine(
                        $"Warning: Development year {developmentYear} is less than origin year {originYear} for product '{product}'. Skipping row");
                    continue;
                }

                if (seenRecords.Contains((product, originYear, developmentYear)))
                {
                    throw new ArgumentException(
                        $"Duplicate record found for product '{product}' in origin year {originYear} and development year {developmentYear}.");
                }

                seenRecords.Add((product, originYear, developmentYear));

                var record = new ClaimRecord {
                    Product = product,
                    OriginYear = originYear,
                    DevelopmentYear = developmentYear,
                    IncrementalValue = incrementalValue
                };
                
                Records.Add(record);
            }
        };

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
