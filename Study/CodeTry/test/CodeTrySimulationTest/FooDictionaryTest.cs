namespace CodeTryTests
{
    public class FooDictionaryTest
    {
        [Fact]
        public void FooDictionaryAdd_TimeTest()
        {
            var m = FooDictionary.FooDictionaryIns;
            for (int i = 0; i < 10000; i++)
            {
                var key = new Foo(i);
                m.Add(key, i*2);
            }
        }

        [Fact]
        public void FooDictionaryFind_Should_NotFind()
        {
            var m = FooDictionary.FooDictionaryIns;
            for (int i = 0; i < 2; i++)
            {
                var key = new Foo(i);
                var value = i * 2;
                m.Add(key, i * 2);
            }
            m.TryGetValue(new Foo(1), out var result).Should().BeFalse();
        }

        [Fact]
        public void Foo2DictionaryAdd_TimeTest()
        {
            var m = FooDictionary.Foo2DictionaryIns;
            for (int i = 0; i < 10000; i++)
            {
                var key = new Foo2(i);
                m.Add(key, i * 2);
            }
        }

        [Fact]
        public void FooD2ictionaryFind_Should_Find()
        {
            var m = FooDictionary.Foo2DictionaryIns;
            for (int i = 0; i < 2; i++)
            {
                var key = new Foo2(i);
                var value = i * 2;
                m.Add(key, i * 2);
            }
            m.TryGetValue(new Foo2(1), out var result).Should().BeTrue();
            result.Should().Be(2);
        }
    }
}