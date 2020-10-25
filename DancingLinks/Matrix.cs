using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
// Outer enumeration is on rows, inner enumeration is on columns
using EnumMtx = System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<bool>>;

namespace DancingLinks
{
	public class Matrix
    {
        #region Private variables
        private readonly Master _master = new Master();
        #endregion

        #region Public variables
        public int Width { get; }

        internal IEnumerable<ColHeader> Columns => 
            _master.
                RowLink.Elements().
                Where(ch => ch != _master).
                Select(l => l).
                Cast<ColHeader>();

        internal bool Solved => _master.RowLink.Unlinked;
        #endregion

        #region Constructors
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>	Constructor with just rows which consist of enumerable options. </summary>
        ///
        /// <remarks>
        /// These options must be column descriptions.  If two rows both cover the same option that is
        /// only indicated by them have the identical object in this enumeration. Darrell Plank,
        /// 10/23/2020.
        /// </remarks>
        ///
        /// <param name="rows"> 	The rows. </param>
        /// <param name="width">	The width. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Matrix(IEnumerable<IEnumerable<object>> rows, int width) 
            : this(rows, Enumerable.Range(0, width).ToList()) {}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>	Constructor with just rows which consist of enumerable options </summary>
        ///
        /// <remarks>	These options must be column descriptions.  If two rows both cover the same option
        /// 			that is only indicated by them have the identical object in this enumeration.
        /// 			Darrell Plank, 10/23/2020. </remarks>
        ///
        /// <param name="rows"> 	The rows. </param>
        /// <param name="width">	The width. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Matrix(IEnumerable<IEnumerable<object>> rows, IEnumerable descriptions) :
            this(rows.Select(r => ((object)null, r)), descriptions) {}

        public Matrix(IEnumerable<(object, IEnumerable<object>)> rows, IEnumerable descriptions)
        {
            var descDict = new Dictionary<object, ColHeader>();
            foreach (var desc in descriptions)
            {
                if (descDict.ContainsKey(desc))
                {
                    throw new InvalidDataException("Description fields must be unique in Matrix constructor");
                }

                var lastColumn = _master.RowLink.Prev.Container as ColHeader;
                var thisColumn = new ColHeader(desc, lastColumn);
                descDict[desc] = thisColumn;
                Width++;
            }

            foreach (var (rowDesc, row) in rows)
            {
                MtxOne prevOne = null;
                foreach (var desc in row)
                {
                    if (!descDict.ContainsKey(desc))
                    {
                        throw new InvalidDataException("Row description with no matching column description");
                    }
                    var col = descDict[desc];
                    prevOne = new MtxOne(prevOne?.RowLink, col.ColLink.Prev, col, rowDesc);
                    col.Size++;
                }
            }
        }

        public Matrix(EnumMtx input, IEnumerable<object> descriptions = null)
        {
            var setupColumns = true;
            var iCol = 0;
            using (var descEnum = descriptions?.GetEnumerator())
            {
                foreach (var row in input)
                {
                    ColHeader thisColumn = _master;
                    MtxOne prevOne = null;

                    foreach (var element in row)
                    {
                        if (setupColumns)
                        {
                            object desc;
                            if (descriptions == null)
                            {
                                // Defaults to column index
                                desc = iCol++;
                            }
                            else
                            {
                                desc = descEnum.Current;
                                descEnum.MoveNext();
                            }

                            var lastColumn = _master.RowLink.Prev.Container as ColHeader;
                            thisColumn = new ColHeader(desc, lastColumn);
                            Width++;
                        }
                        else
                        {
                            Debug.Assert(thisColumn != null, nameof(thisColumn) + " != null");
                            thisColumn = thisColumn.RowLink.Next.Container as ColHeader;
                        }

                        if (element)
                        {
                            Debug.Assert(thisColumn != null, nameof(thisColumn) + " != null");
                            prevOne = new MtxOne(prevOne?.RowLink, thisColumn.ColLink.Prev, thisColumn);
                            thisColumn.Size++;
                        }
                    }

                    setupColumns = false;
                }
            }
        }
        #endregion
    }
}
