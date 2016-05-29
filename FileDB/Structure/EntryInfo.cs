using System;
using System.IO;

namespace Numeria.IO
{
    public class EntryInfo
    {
        public Guid ID { get; private set; }
        public string FileUrl { get; private set; }  //file url 
        public uint FileLength { get; internal set; }
        public ushort FileMetadataLength { get; internal set; }

        public DateTime FileDateTime { get; private set; }

        internal EntryInfo(string fileName)
            : this(fileName, Guid.NewGuid(), DateTime.Now)
        {

        }
        internal EntryInfo(string fileName, Guid guid, DateTime datetime)
        {
            //this version filename must not longer than 36 bytes
            if (fileName.Length > IndexNode.FILENAME_SIZE)
            {
                throw new FileDBException("file length >" + IndexNode.FILENAME_SIZE);
            }

            ID = Guid.NewGuid();
            FileUrl = fileName;
            FileLength = 0;
            FileMetadataLength = 0;
            FileDateTime = datetime;
        }
        internal EntryInfo(IndexNode node)
        {
            ID = node.ID;
            FileUrl = node.FileUrl;
            FileLength = node.FileLength;
            FileMetadataLength = node.FileMetaDataLength;
            //---------------------
            //no datetime or other metadata here             
            //---------------------
        }
        public override string ToString()
        {
            return this.FileUrl;
        }
    }
}
