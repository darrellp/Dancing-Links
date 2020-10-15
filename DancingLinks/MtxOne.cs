namespace DancingLinks
{
	class MtxOne
    {
        internal Link<MtxOne> RowLink;
        internal Link<MtxOne> ColLink;
        internal ColHeader Header;

        internal MtxOne() : this(null, null, null){}

        internal MtxOne(Link<MtxOne> prvRow, Link<MtxOne> prvCol, ColHeader header)
        {
            RowLink = new Link<MtxOne>(this, prvRow);
            ColLink = new Link<MtxOne>(this, prvCol);
            Header = header;
        }
    }
}
