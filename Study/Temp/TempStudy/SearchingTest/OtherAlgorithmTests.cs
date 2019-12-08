using System.Collections.Generic;
using Xunit;

namespace AntKott.Study.Searching.Tests
{
    public class OtherAlgorithmTests
    {
        private readonly OtherAlgorithm _otherAlgorithm = new OtherAlgorithm();

        public static IEnumerable<object[]> Filtering_Data()
        {
            yield return new object[] { new List<string>(){"apple", "pear", "orange", "banana", "apple",
         "orange", "apple", "pear", "banana", "orange",
         "apple", "kiwi", "pear", "apple", "orange" },
                new HashSet<string>(){ "apple", "pear", "orange", "banana", "kiwi" }
        };
            
        }

        [Theory]
        [MemberData(nameof(Filtering_Data))]
        public void Filtering_Test(List<string> input, HashSet<string> expected)
        {
            var result = _otherAlgorithm.Filtering(input);
            Assert.Equal(expected, result);
        }

    }

}
