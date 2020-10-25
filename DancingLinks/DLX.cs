using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DancingLinks
{
	public class DLX
    {
        private readonly Matrix _mtx;
        private readonly Stack<MtxOne> _slnRows;

        public DLX(Matrix mtx)
        {
            _mtx = mtx;
            _slnRows = new Stack<MtxOne>();
        }

        public IEnumerable<IEnumerable<MtxOne>> Solve()
        {
            // If R[h] == h
            if (_mtx.Solved)
            {
                // print the current solution and return
                yield return _slnRows;
                yield break;
            }

            // Otherwise choose a column object c
            var sizeMin = int.MaxValue;
            ColHeader colSelect = null;

            foreach (var col in _mtx.Columns)
            {
				if (col.Size < sizeMin)
                {
				    if (col.Size == 0)
				    {
					    // Slight shortcut
					    yield break;
				    }

                    colSelect = col;
                    sizeMin = col.Size;
                }
            }

            // Cover column c
            Debug.Assert(colSelect != null, nameof(colSelect) + " != null");
            colSelect.Cover();

            // For each r <- D[c], D[D[c]],..., while r != c
            foreach (var one in colSelect.Ones)
            {
                // Set O[k] <- r
                _slnRows.Push(one);

                // For each j <- R[r], R[R[r]],..., while j != r
                foreach (var colCovered in one.RowLink.Elements().Skip(1).Select(o => o.Header))
                {
                    // cover column j
                    colCovered.Cover();
                }

                // search(k + 1)
                // NOTE: We don't use k because we put solutions into a stack whose
                // length is kept internal to the stack - effectively, k == _solns.Count. 
                foreach (var soln in Solve())
                {
                    yield return soln;
                }

                // r <- O[k], c <- C[r]
                // NOTE: We haven't altered r (one) or c (colSelect) so no need to reset them here but since we
                // keep our solutions in a stack, we have to pop the current one off
                _slnRows.Pop();

                // For each j <- L[r], L[L[r]],..., while j != r
                foreach (var colUncover in one.RowLink.Elements(false).Skip(1).Select(o => o.Header))
                {
                    colUncover.Uncover();
                }
            }
            // Uncover column c and return
            colSelect.Uncover();
        }
    }
}
