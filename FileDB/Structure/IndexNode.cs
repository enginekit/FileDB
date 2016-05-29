using System;
using System.IO;

namespace Numeria.IO
{
    internal class IndexNode
    {
        public const int FILENAME_SIZE = 36;       // Size of file name string
       
        public Guid ID { get; set; }               // 16 bytes

        public bool IsDeleted { get; set; }        //  1 byte

        public IndexLink Right { get; set; }       //  5 bytes 
        public IndexLink Left { get; set; }        //  5 bytes

        public uint DataPageID { get; set; }       //  4 bytes

        // Info

        public DateTime FileDateTime { get; set; } //8 bytes
        public uint FileLength { get; set; }       //  4 bytes
        public string FileUrl { get; set; } // 2 bytes + 36 bytes
 

        public IndexPage IndexPage { get; set; }

        public IndexNode(IndexPage indexPage)
        {
            ID = Guid.Empty;
            IsDeleted = true; // Start with index node mark as deleted. Update this after save all stream on disk
            Right = new IndexLink();
            Left = new IndexLink();
            DataPageID = uint.MaxValue;
            IndexPage = indexPage;
        }

        public void UpdateFromEntry(EntryInfo entity)
        {
            ID = entity.ID;
            FileUrl = entity.FileUrl;
            FileLength = entity.FileLength;
        }
    }
}
