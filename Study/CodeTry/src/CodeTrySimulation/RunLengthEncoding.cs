using System.Text;

namespace CodeTrySimulation
{
    public static class RunLengthEncoding
    {
        public static string Encode(string input) {
            var n = input.Length;
            var result = new StringBuilder();
            for (int i = 0; i < n; i++)
            {
                // Count occurrences of current character
                int count = 1;
                while (i < n - 1 && input[i] == input[i + 1])
                {
                    count++;
                    i++;
                }
                if (count > 1) {
                    result.Append(count);
                }
                result.Append(input[i]);
            }
            return result.ToString();
        }

        public static string EncodeList(IEnumerable<string> inputs)
        {
            var result = new StringBuilder();
            foreach (var input in inputs) {
                result.Append(Encode(input));
            }
            return result.ToString();
        }
    }
}
