using System.Collections.Generic;
using System.Linq;

namespace DancingLinks
{
    public class MtxOne
    {
        internal Link<MtxOne> RowLink;
        internal Link<MtxOne> ColLink;
        internal ColHeader Header;
        public object RowInfo { get; }
        public IEnumerable<object> CoveredHeaders => RowLink.Elements().Select(mo => mo.Header.Desc);

        internal MtxOne() : this(null, null, null){}

        internal MtxOne(Link<MtxOne> prvRow, Link<MtxOne> prvCol, ColHeader header, object rowInfo = null)
        {
            RowLink = new Link<MtxOne>(this, prvRow);
            ColLink = new Link<MtxOne>(this, prvCol);
            Header = header;
            RowInfo = rowInfo;
        }
    }
}
