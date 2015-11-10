using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RmsAuto.Common.Misc;
using RmsAuto.Store.Entities;
using System.Collections.Generic;

namespace RmsAuto.UnitTesting
{
    [TestClass]
    public class MiscTests
    {
        [TestClass]
        public class ExtensionsTests
        {
            [TestMethod]
            public void TestSlicing()
            {
                int[] enu = { 1, 2, 3, 4, 5, 6, 7, 8 };

                var slice1 = enu.Slice(2);
                Assert.AreEqual(slice1.Count(), 2);
                Assert.AreEqual(slice1.First().Count(), 4);

                var slice2 = enu.Slice(3);
                Assert.AreEqual(slice2.Count(), 3);
                Assert.AreEqual(slice2.First().Count(), 3);
                Assert.AreEqual(slice2.Skip(2).First().Count(), 2);

                var slice3 = enu.Slice(enu.Count());
                Assert.AreEqual(slice3.Count(), enu.Count());
            }

            [ExpectedException(typeof(ArgumentException))]
            [TestMethod]
            public void TestWrongSlicing1()
            {
                int[] enu = { 1, 2, 3, 4, 5, 6, 7, 8 };
                enu.Slice(enu.Count() + 1);
            }

            [ExpectedException(typeof(ArgumentException))]
            [TestMethod]
            public void TestWrongSlicing2()
            {
                int[] enu = { };
                enu.Slice(0);
            }

            [TestMethod]
            public void TestOverSlicing()
            {
                int[] enu = { 1 };
                var slice1 = enu.Slice(2, true);
                var slice2 = enu.Slice(3, true);
                Assert.AreEqual(slice1.Count(), 2);
                Assert.AreEqual(slice2.Count(), 3);
                Assert.AreEqual(slice1.First().First(), 1);
                Assert.AreEqual(slice1.Skip(1).First().Count(), 0);
                Assert.AreEqual(slice2.Skip(2).First().Count(), 0);
            }

            [TestMethod]
            public void SubtractTest()
            {
                int[] first =  { 1, 2, 3, 8, 118 };
                int[] second = { 2, 3, 5, 8, 10, 1001, 2089 };

                var result = new List<int>(first).Subtract(new List<int>(second));
                Assert.IsTrue(result.Count == 2 && result[0] == 1 && result[1] == 118);

                result = new List<int>().Subtract(new List<int>(second));
                Assert.IsTrue(result.Count == 0);
            }

           /* [TestMethod]
            public void IntersectSortedTest()
            {
                int[] first = { 1, 2, 3, 10, 18 };
                int[] second = { -1, 0, 2, 18, 50, 1001 };

                var result = new List<int>(first).IntersectSorted(new List<int>(second), false);
                Assert.IsTrue(result.Count == 2 && result[0] == 2 && result[1] == 18);

                var aList = new List<CrossInfo>();
                aList.Add(new CrossInfo("vw", "110", "bmw", "520"));
                aList.Add(new CrossInfo("vw", "120", "bmw", "520"));
                aList.Add(new CrossInfo("vw", "121", "bmw", "520"));

                var bList = new List<CrossInfo>();
                bList.Add(new CrossInfo("vw", "120", "bmw", "520"));

                var resList = aList.IntersectSorted(bList, new PartKeyCrossComparer(), false);
                Assert.IsTrue(resList.Count == 1 && resList[0].PartKey.PartNumber == "120");
            }*/

           /* [TestMethod]
            public void CheckAllInSecondaryTest()
            {
                int[] first = { 0, 1, 1, 28 };
                int[] second = { -10, 1, 10, 20, 20, 25, 28 };

                var res = new List<int>(first).CheckItemsInSecondList(new List<int>(second));
                //Assert.IsTrue(res.Count == 4 && res[0] == 1 && res[1] == 1 && res[2] == 1 && res[3] == 10);

                //  Потестим с двумя одинаковыми кросс-айтемами в исходном списке
                var aList = new List<CrossInfo>();
                var bList = new List<CrossInfo>();

                aList.Add(new CrossInfo("ACDelco", "AB31019", "Dayco", "1001"));
                aList.Add(new CrossInfo("ACDelco", "AB31019", "Pirelli", "1001"));
                bList.Add(new CrossInfo("ACDelco", "AB31019", "ACDelco", "AB31019"));

                var resList = aList.CheckItemsInSecondList(bList, new PartKeyCrossComparer());
            }*/

            [TestMethod]
            public void UnionTest()
            {
                int[] first = { 1, 2, 4, 5, 10, 100 };
                int[] second = { -10, 2, 4 };

                var res = new List<int>(first).UnionAndDistinct(new List<int>(second));
                Assert.IsTrue(res.Count == 7 && res.Distinct().Count() == res.Count);

                first = new int[] { 1, 2, 3 };
                second = new int[] { -10, 2, 4, 5, 180 };

                res = new List<int>(first).UnionAndDistinct(new List<int>(second));
                Assert.IsTrue(res.Count == 7 && res.Distinct().Count() == res.Count);
            }
        }
    }
}
