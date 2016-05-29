using System;
using System.Collections.Generic;
using System.Text;
using Numeria.IO;
using System.IO;

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

#if NET20

            //---------------------------------------------------------------------------
            string testfile = @"d:\\WImageTest\\testdb.dat";
            EntryInfo en1 = FileDB.Store(testfile, "/usr/test/d1", GenerateTestDataBuffer("hello!...1"));
            EntryInfo en2 = FileDB.Store(testfile, "/usr/test/d2", GenerateTestDataBuffer("hello!...2"));
            //---------------------------------------------------------------------------
            EntryInfo[] fileList = FileDB.ListFiles(testfile);
            foreach (var f in fileList)
            {   
                FileDB.Delete(testfile, en1.ID);
            }
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
        static byte[] GenerateTestDataBuffer(string datastring)
        {
            return Encoding.UTF8.GetBytes(datastring.ToCharArray());
        }

    }
}
