
using MidiShapeShifter.CSharpUtil;
using NUnit.Framework;


namespace MidiShapeShifterTest.CSharpUtil
{
    [TestFixture]
    internal class LruCacheTest
    {
        [Test]
        public void GetKeyValuePairRemover_CacheSizeOf1_CreateValueGetsCalledForEveryNewElement()
        {
            var cache = new LruCache<int, int>(1);

            AddKeyAndAssertCreateValueIsCalled(cache, 1, true);
            AddKeyAndAssertCreateValueIsCalled(cache, 2, true);
            AddKeyAndAssertCreateValueIsCalled(cache, 1, true);
            AddKeyAndAssertCreateValueIsCalled(cache, 1, false);
        }

        [Test]
        public void GetKeyValuePairRemover_CacheSizeOf2()
        {
            var cache = new LruCache<int, int>(2);

            AddKeyAndAssertCreateValueIsCalled(cache, 1, true);
            AddKeyAndAssertCreateValueIsCalled(cache, 1, false);
            AddKeyAndAssertCreateValueIsCalled(cache, 2, true);
            AddKeyAndAssertCreateValueIsCalled(cache, 1, false);
            AddKeyAndAssertCreateValueIsCalled(cache, 2, false);
            AddKeyAndAssertCreateValueIsCalled(cache, 3, true);
        }

        [Test]
        public void GetKeyValuePairRemover_MultipleValuesAdded_CorrectValueIsReturned()
        {
            var cache = new LruCache<int, int>(2);

            int value1 = cache.GetAndAddValue(1, () => 10);
            int value2 = cache.GetAndAddValue(2, () => 20);
            int value3 = cache.GetAndAddValue(1, () => 10);
            int value4 = cache.GetAndAddValue(10, () => 100);

            Assert.AreEqual(10, value1);
            Assert.AreEqual(20, value2);
            Assert.AreEqual(10, value3);
            Assert.AreEqual(100, value4);
        }

        [Test]
        public void GetKeyValuePairRemover_MoreValuesAddedThenThereAreRoomFor_ValuesThatHaventBeenAccessedAreRemoved()
        {
            var cache = new LruCache<int, int>(3);

            cache.GetAndAddValue(1, () => 1);
            cache.GetAndAddValue(2, () => 2);
            cache.GetAndAddValue(3, () => 3);

            //Should cause 1, 2, or 3 to be removed and the recentlyUsed flag should be set to false 
            //for the remaining two.
            cache.GetAndAddValue(4, () => 4);

            //Adds 2 to the cache or updates its recentlyUsed flag to true.
            cache.GetAndAddValue(2, () => 2);
            //Deletes something other than 2
            cache.GetAndAddValue(5, () => 5);

            //2 is not added again because it already exists
            AddKeyAndAssertCreateValueIsCalled(cache, 2, false);

            //Deletes something other than 2
            cache.GetAndAddValue(6, () => 6);

            //2 is not added again because it already exists
            AddKeyAndAssertCreateValueIsCalled(cache, 2, false);

            //1 and 3 should have been removed by now
            AddKeyAndAssertCreateValueIsCalled(cache, 1, true);
            AddKeyAndAssertCreateValueIsCalled(cache, 3, true);

        }


        protected void AddKeyAndAssertCreateValueIsCalled<Key, Value>(LruCache<Key, Value> cache, Key key, bool createValueShouldBeCalled)
        {
            bool createValueIsCalled = false;
            cache.GetAndAddValue(key, () =>
            {
                createValueIsCalled = true;
                return default(Value);
            });

            Assert.AreEqual(createValueShouldBeCalled, createValueIsCalled);
        }

    }
}
