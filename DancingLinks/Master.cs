namespace DancingLinks
{
    internal class Master : ColHeader
	{
        public override bool IsMaster()
        {
            return true;
        }

        public Master() : base("Master") {}
    }
}
