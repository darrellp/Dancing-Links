using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DancingLinks;

namespace DancingLinksTests
{
	[TestClass]
	public class MatrixTests
	{
		[TestMethod]
		public void TestConstructorFromBoolMatrix()
        {
            var input = new List<List<bool>>
            {
                new List<bool> {true, false},
                new List<bool> {false, true}
            };

            var mtx = new Matrix(input);
            Assert.AreEqual(2, mtx.Columns.Count());
            foreach (var col in mtx.Columns)
            {
                // Should be one element in each column
                Assert.AreEqual(1, col.Ones.Count());
                // ...and one element in each row
                Assert.AreEqual(1, col.Ones.First().RowLink.Elements().Count());
            }
        }

        [TestMethod]
        public void TestConstructorFromDescList()
        {
            var input = new List<List<object>>
            {
                new List<object> {0},
                new List<object> {1}
            };

            var mtx = new Matrix(input, 2);
            Assert.AreEqual(2, mtx.Columns.Count());
            foreach (var col in mtx.Columns)
            {
                // Should be one element in each column
                Assert.AreEqual(1, col.Ones.Count());
                // ...and one element in each row
                Assert.AreEqual(1, col.Ones.First().RowLink.Elements().Count());
            }
        }
	}
}
