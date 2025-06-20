namespace Claims
{
    class Program {
        public static void Main(string[] args) {
            DataReader reader = new();
            string inputFile = "files/problem.csv";
            reader.ReadClaims(inputFile);
            reader.Stats();

            var accumulatedClaims = ClaimCalculator.AccumulateClaims(reader.Records, reader.MinOriginYear, reader.MaxDevYear);

            DataWriter.WriteToCsv(
                accumulatedClaims,
                reader.MinOriginYear,
                reader.MaxDevYear,
                "files/output.csv"
            );
        } 
    }
}