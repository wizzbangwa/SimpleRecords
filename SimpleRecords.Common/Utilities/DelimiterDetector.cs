namespace SimpleRecords.Common.Utilities
{
    public class DelimiterDetector
    {
        private static readonly string[] _delimiters = { " | ", ", ", " " };

        public static string DetectSeparator(string delimitedLine)
        {
            foreach (string delimiter in _delimiters)
            {
                if (delimitedLine.Contains($"\"{delimiter}\"") ||
                    delimitedLine.Contains($"'{delimiter}'") ||
                    delimitedLine.Contains(delimiter))
                    return delimiter;
            }

            return null;
        }
    }
}
