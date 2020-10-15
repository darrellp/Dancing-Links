using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DancingLinks;

namespace DancingLinksTests
{
	[TestClass]
	public class DLXTests
    {
        [TestMethod]
        public void TestSolve()
        {
            var input = new List<List<object>>
            {
                new List<object> {2, 4, 5},
                new List<object> {0, 3, 6},
                new List<object> {1, 2, 5},
                new List<object> {0, 3},
                new List<object> {1, 6},
                new List<object> {3, 4, 6},
            };
            var soln = new[]
            {
                new[] {0, 3},
                new[] {2, 4, 5},
                new[] {1, 6},
            };

            var mtx = new Matrix(input, 7);

            var dlx = new DLX(mtx);
            var solns = dlx.Solve().ToArray();
            Assert.AreEqual(1, solns.Length);
            foreach (var sln in solns)
            {
                foreach (var row in sln)
                {
                    var rowCur = row.Cast<int>().OrderBy(i => i).ToArray();
                    Assert.IsTrue(
                        soln.
                            Any(r => r.SequenceEqual(rowCur)));
                }
            }
        }
	}
}
