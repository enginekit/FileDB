using System;
using System.Collections.Generic;
using System.Text;
using Numeria.IO;
using System.IO;

using SharpConnect.Data;
using SharpConnect.Data.Meltable;

#if !NET20
using System.Threading.Tasks;
using System.Linq;
#endif
namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test1();
            //Test2();
            Test3();
        }


        static byte[] GenerateTestDataBuffer(string datastring)
        {
            return Encoding.UTF8.GetBytes(datastring.ToCharArray());
        }

        class SampleStoreRequest
        {
            public readonly string fileName;
            public readonly byte[] buffer;
            public SampleStoreRequest(string fileName, byte[] buffer)
            {
                this.fileName = fileName;
                this.buffer = buffer;
            }
        }

        static void Test2()
        {

            //------------------------------------------------------------------------------
            string dbFileName = @"d:\\WImageTest\\testdb2.dat";
            //EntryInfo[] fileList2 = FileDB.ListFiles(dbFileName);
            //var filename1 = fileList2[0].FileUrl;

            //1. open
            byte[] inputBuffer = GenerateTestDataBuffer("hello!...1");
            List<SampleStoreRequest> storeReqs = new List<SampleStoreRequest>()
            {
                new SampleStoreRequest("a01", GenerateTestDataBuffer("hello_a01")),
                new SampleStoreRequest("a02", GenerateTestDataBuffer("hello_a02")),
                new SampleStoreRequest("a03", GenerateTestDataBuffer("hello_a03")),
                new SampleStoreRequest("a04", GenerateTestDataBuffer("hello_a04")),
                new SampleStoreRequest("a05", GenerateTestDataBuffer("hello_a05"))
            };


            using (var db = new FileDB(dbFileName, FileAccess.ReadWrite))
            {
                EntryInfo[] prevFiles = db.ListFiles();
                int j = storeReqs.Count;
                for (int i = 0; i < j; ++i)
                {
                    SampleStoreRequest req = storeReqs[i];
                    using (var dataStream = new MemoryStream(req.buffer))
                    {
                        EntryInfo en = db.Store(req.fileName, dataStream);
                        db.Flush();
                    }
                }
            }

            //------------------------------------------------------------------------------
            //test read back
            EntryInfo[] fileList = FileDB.ListFiles(dbFileName);

        }
        static void Test1()
        {
#if NET20
            //---------------------------------------------------------------------------
            string testfile = @"d:\\WImageTest\\testdb.dat";
            //test store in the same file name
            EntryInfo en1 = FileDB.Store(testfile, "/usr/test/d1/aaaaaaaaa/bbbbbbbbb/ccccccccddddddddddddddd.a.txt", GenerateTestDataBuffer("hello!...1"));
            EntryInfo en2 = FileDB.Store(testfile, "/usr/test/d1/aaaaaaaaa/bbbbbbbbb/ccccccccddddddddddddddd.a.txt", GenerateTestDataBuffer("hello!...2"));
            EntryInfo en3 = FileDB.Store(testfile, "/usr/test/d1/aaaaaaaaa/bbbbbbbbb/ccccccccddddddddddddddd.a.txt", GenerateTestDataBuffer("hello!...3"));
            EntryInfo en4 = FileDB.Store(testfile, "/usr/test/d1/aaaaaaaaa/bbbbbbbbb/ccccccccddddddddddddddd.a.txt", GenerateTestDataBuffer("hello!...4"));
            EntryInfo en5 = FileDB.Store(testfile, "/usr/test/d1/aaaaaaaaa/bbbbbbbbb/ccccccccddddddddddddddd.a.txt", GenerateTestDataBuffer("hello!...5"));
            //---------------------------------------------------------------------------
            EntryInfo[] fileList = FileDB.ListFiles(testfile);

            using (MemoryStream ms = new MemoryStream())
            {
                //test read file and metadata
                //EntryInfo enInfo = FileDB.Read(testfile, en5.ID, ms); 

                FileDB.ReadFileContent(testfile, fileList[0], ms);
                string content = System.Text.Encoding.UTF8.GetString(ms.ToArray());

                //read only file metadata
                //EntryInfo enInfo = FileDB.ReadMetadata(testfile, en5.ID);


                ms.Close();
            }

            //foreach (var f in fileList)
            //{   
            //    FileDB.Delete(testfile, en1.ID);
            //}
            //---------------------------------------------------------------------------
            fileList = FileDB.ListFiles(testfile);
#else
            //
            // Parallel Insert
            //
            string dbFile = @"C:\Temp\MvcDemo.dat"; 
            string[] files = new string[] {
                @"C:\Temp\DSC04901.jpg", @"C:\Temp\DSC04902.jpg", @"C:\Temp\ZipFile.zip" }; 
            Parallel.For(0, 3, (i) =>
            {
                Console.WriteLine("Starting " + Path.GetFileName(files[i]));
                FileDB.Store(dbFile, files[i]);
                Console.WriteLine("Ended " + Path.GetFileName(files[i]));
            }); 
            Console.ReadLine();
#endif
        }


        static void Test3()
        {

            string dbFileName = @"d:\\WImageTest\\testdb3.dat";
#if DEBUG
            if (File.Exists(dbFileName))
            {
                File.Delete(dbFileName);
            }
#endif

            using (var db = new FileDB(dbFileName, FileAccess.ReadWrite))
            {
                byte[] docStream = TestGenLqDocStream();
                //save
                using (var dataStream = new MemoryStream(docStream))
                {
                    db.Store(dbFileName, dataStream);
                    db.Flush();
                }
            }

            //test read file
            //and generate liquid document
            using (var db = new FileDB(dbFileName, FileAccess.ReadWrite))
            {
                EntryInfo[] prevFiles = db.ListFiles();
                //test read file
                using (MemoryStream ms = new MemoryStream())
                {
                    db.Read(prevFiles[0].ID, ms);
                    ms.Position = 0;

                    //convert data to document
                    var docDeser = new LiquidDocumentDeserializer();
                    var reader = new BinaryReader(ms);
                    docDeser.SetBinaryReader(reader);
                    docDeser.ReadDocument();
                    LiquidDoc result = docDeser.Result;
                    ms.Close();
                }
            }
        }
        static byte[] TestGenLqDocStream()
        {
            LiquidDoc doc = new LiquidDoc();
            var elem = doc.CreateElement("user_info");
            doc.DocumentElement = elem;
            elem.AppendAttribute("first_name", "A");
            elem.AppendAttribute("last_name", "B");
            elem.AppendAttribute("age", 20);

            //test native array object            
            elem.AppendAttribute("memberlist1", new string[] { "x", "y", "z" });
            elem.AppendAttribute("memberlist2", new object[] { 1, "y", "z" });

            Dictionary<string, int> memberlist3 = new Dictionary<string, int>();
            memberlist3.Add("score1", 10);
            memberlist3.Add("score2", 20);
            memberlist3.Add("score3", 30);
            memberlist3.Add("score4", 40);
            elem.AppendAttribute("memberlist3", memberlist3);

            List<int> memberlist4 = new List<int>() { 1, 2, 3, 4, 5 };
            elem.AppendAttribute("memberlist4", memberlist4);


            byte[] output = null;
            using (var ms = new MemoryStream())
            {
                var ser = new LiquidSerializer();
                var binWriter = new BinaryWriter(ms);
                ser.SetBinaryWriter(binWriter);

                ser.WriteDocument(doc);

                output = ms.ToArray();
                ms.Close();
            }
            return output;

            //using (var ms = new MemoryStream(output))
            //{
            //    var docDeser = new LiquidDocumentDeserializer();

            //    var reader = new BinaryReader(ms);
            //    docDeser.SetBinaryReader(reader);
            //    docDeser.ReadDocument();
            //    LiquidDoc result = docDeser.Result;
            //}
        }
    }
}
