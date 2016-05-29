namespace Numeria.IO
{
    internal class IndexPage : BasePage
    {
        public const long INDEX_HEADER_SIZE = 46;
        public const int NODES_PER_PAGE = 50;
        //each index use data = 81 
        //so (81*50) = 4050 
        //4050+ 46 => 4096 

        public override PageType Type { get { return PageType.Index; } }  //  1 byte
        public byte UsedNodeCount { get; private set; }                           //  1 byte

        public IndexNode[] Nodes { get; set; }

        public bool IsDirty { get; set; }

        public IndexPage(uint pageID)
        {
            PageID = pageID;
            NextPageID = uint.MaxValue;
            //UsedNodeCount = 0;
            Nodes = new IndexNode[IndexPage.NODES_PER_PAGE];
            IsDirty = false;

            for (int i = 0; i < IndexPage.NODES_PER_PAGE; i++)
            {
                Nodes[i] = new IndexNode(this);
            }
        }

        public void SetUsedNodeCount(byte nodeUsedCount)
        {
            UsedNodeCount = nodeUsedCount;
        }
        public void IncNodeUsed()
        {
            UsedNodeCount++;
        }
    }
}
