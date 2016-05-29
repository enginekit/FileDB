﻿using System;
using System.IO;

namespace Numeria.IO
{
    public class EntryInfo
    {
        public Guid ID { get; private set; }
        public string FileUrl { get; private set; }  //file url 
        public uint FileLength { get; internal set; }
        public DateTime FileDateTime { get; internal set; }

        internal EntryInfo(string fileName)
            : this(fileName, Guid.NewGuid())
        {
        }
        internal EntryInfo(string fileName, Guid guid)
        {
            //this version filename must not longer than 36 bytes
            if (fileName.Length > IndexNode.FILENAME_SIZE)
            {
                throw new FileDBException("file length >" + IndexNode.FILENAME_SIZE);
            }

            ID = Guid.NewGuid();
            FileUrl = fileName;
            FileLength = 0;
        }
        internal EntryInfo(IndexNode node)
        {
            ID = node.ID;
            FileUrl = node.FileUrl;
            FileLength = node.FileLength;
            FileDateTime = node.FileDateTime;
        }
        public override string ToString()
        {
            return this.FileUrl;
        }
    }
}
