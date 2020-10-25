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
            var solns = dlx.Solve().ToList();
            Assert.AreEqual(1, solns.Count());
            foreach (var sln in solns)
            {
                foreach (var mo in sln)
                {
                    var row = mo.CoveredHeaders;
                    var rowCur = row.Cast<int>().OrderBy(i => i).ToArray();
                    Assert.IsNull(mo.RowInfo);
                    Assert.IsTrue(
                        soln.
                            Any(r => r.SequenceEqual(rowCur)));
                }
            }

            var input2 = new List<(object, IEnumerable<object>)>
            {
                ((object)1, new List<object> {2, 4, 5}),
                ((object)2, new List<object> {0, 3, 6}),
                ((object)3, new List<object> {1, 2, 5}),
                ((object)4, new List<object> {0, 3}),
                ((object)5, new List<object> {1, 6}),
                ((object)6, new List<object> {3, 4, 6}),
            };

            var descs = new List<int> {0, 1, 2, 3, 4, 5, 6};
            var soln2 = new[] {1, 4, 5};
            var mtx2 = new Matrix(input2, descs);

            dlx = new DLX(mtx2);
            solns = dlx.Solve().ToList();
            Assert.AreEqual(1, solns.Count());
            foreach (var sln in solns)
            {
                var rows = sln.Select(mo => mo.RowInfo).Cast<int>().OrderBy(i => i).ToList();
                Assert.IsTrue(rows.Zip(soln2, (i1, i2) => i1 == i2).All(f => f));
            }

        }
    }
}
