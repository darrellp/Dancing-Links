using System.Collections.Generic;
using System.Linq;

namespace DancingLinks
{
	internal class ColHeader : MtxOne
	{
        public object Desc { get; }
        public int Size { get; set; }

        public virtual bool IsMaster()
        {
            return false;
        }

        protected ColHeader() : this(null) {}

        public ColHeader(object desc, MtxOne colheaderPrev = null)
        {
            Desc = desc;
            Size = 0;
            RowLink = new Link<MtxOne>(this, colheaderPrev?.RowLink);
            ColLink = new Link<MtxOne>(this);
            Header = this;
        }

        public IEnumerable<MtxOne> Ones =>
            this.
                ColLink.Elements().
                Where(ch => ch != this);
        public IEnumerable<MtxOne> OnesRev =>
            this.
                ColLink.Elements(false).
                Where(ch => ch != this);

        public void Cover()
        {
            RowLink.Unlink();
            foreach (var one in Ones)
            {
                foreach (var r in one.RowLink.Elements().Skip(1))
                {
                    r.ColLink.Unlink();
                    r.Header.Size--;
                }
            }
        }

        public void Uncover()
        {
            foreach (var one in OnesRev)
            {
                foreach (var r in one.RowLink.Elements(false).Skip(1))
                {
                    r.Header.Size++;
                    r.ColLink.Relink();
                }
            }
            RowLink.Relink();
        }
    }
}
