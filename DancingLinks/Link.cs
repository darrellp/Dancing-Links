using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DancingLinksTests")]

namespace DancingLinks
{
	internal class Link<T>
    {
        internal Link<T> Next { get; set; }
        internal Link<T> Prev { get; set; }
        internal T Container { get; set; }

		public bool Unlinked => Next == this;

        public Link(T container, Link<T> prev = null)
        {
            Container = container;
            if (prev == null)
            {
                Next = Prev = this;
                return;
            }
            Next = prev.Next;
            Prev = prev;
            Next.Prev = this;
            Prev.Next = this;
        }

        public void Relink()
        {
            Next.Prev = this;
            Prev.Next = this;
        }

        public void Unlink()
        {
            Prev.Next = Next;
            Next.Prev = Prev;
        }

        internal IEnumerable<T> Elements(bool fwd = true)
        {
            // If someone is using this enumeration to unlink all the elements then
            // after unlinking "this" the prev element would not point to "this" any
            // more so looping until we get back to this element will fail.  Thus, we
            // loop until we reach the previous element so we keep track of it in
            // this variable. 
            var last = fwd ? Prev : Next;
            yield return this.Container;
            if (Next == this)
            {
                yield break;
            }
            var next = this;
            do
            {
                Debug.Assert(next != null, nameof(next) + " != null");
                next = fwd ? next.Next : next.Prev;
                yield return next.Container;
            } while (next != last);
        }
    }
}
