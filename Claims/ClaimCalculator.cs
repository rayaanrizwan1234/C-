using System;

namespace Claims;

public class ClaimCalculator
{
    public static Dictionary<string, Dictionary<int, List<double>>> AccumulateClaims(List<ClaimRecord> records, int minOriginYear, int maxDevYear) {
            // Converting records to a dictionary grouped by Product and then by (OriginYear, DevelopmentYear)
            Dictionary<string, Dictionary<(int OriginYear, int DevelopmentYear), double>> dict = records
            .GroupBy(r => r.Product)
            .ToDictionary(
                g => g.Key,
                g => g.GroupBy(r => (r.OriginYear, r.DevelopmentYear))
                      .ToDictionary(
                          x => x.Key,
                          x => x.Sum(r => r.IncrementalValue)
                      )
            );

            Dictionary<string, Dictionary<int, List<double>>> accumulatedClaims = new();

            // loop over products
            foreach (var product in dict)
            {
                if (!accumulatedClaims.ContainsKey(product.Key))
                {
                    accumulatedClaims[product.Key] = new Dictionary<int, List<double>>();
                }

                for (int oy = minOriginYear; oy <= maxDevYear; oy++) {
                    if (!accumulatedClaims[product.Key].ContainsKey(oy))
                    {
                        accumulatedClaims[product.Key][oy] = new List<double>();
                    }
                    double val = 0;
                    for (int dy = oy; dy <= maxDevYear; dy++) {
                        if (product.Value.TryGetValue((oy, dy), out double incrementalValue))
                        {
                            val += incrementalValue;
                        }
                        accumulatedClaims[product.Key][oy].Add(val);
                    }
                }
            }

            return accumulatedClaims;
    }
}
