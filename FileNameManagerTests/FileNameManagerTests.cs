using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileNameManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yutil.YFile;
using System.IO;
namespace Yutil.YFile.Tests
{
    [TestClass()]
    public class FileNameManagerTests
    {
        [TestMethod()]
        public void OpenDirectoryTest()
        {

             string path= @"C:\";
            YFileNameManager fileNameManager = new YFileNameManager();
            Assert.AreEqual(fileNameManager.OpenDirectory(path), true);
            Assert.AreEqual(path, fileNameManager.DirectoryInfo.FullName);
        }
        private void MakeTestFolder()
        {
            string path = @"C:\test0070\";
            Directory.CreateDirectory(path);
            string filea = path + "n1.txt";
            string fileb = path + "n2.srt";
            string filec = path + "n3.txt";
            string filed = path + "n4.srt";
            string filee = path + "n5.txt";
            string filef = path + "n6.srt";

            File.Create(filea).Close();
            File.Create(fileb).Close();
            File.Create(filec).Close();
            File.Create(filed).Close();
            File.Create(filee).Close();
            File.Create(filef).Close();
        }
        private void DeleteTestFolder()
        {
            string path = @"C:\test0070\";
            Directory.Delete(path ,true);
        }
        [TestMethod()]
        public void GetFolderFilesByExtenstionTest()
        {
            //DeleteTestFolder();
            MakeTestFolder();
            string path = @"C:\test0070\";
            YFileNameManager fileNameManager = new YFileNameManager();
            fileNameManager.OpenDirectory(path);
            fileNameManager.SplitByExtention( new string[] { "txt","srt" });
            var filesa = fileNameManager.ExtentionFiles["txt"].Count;
            var filesb = fileNameManager.ExtentionFiles["srt"].Count;
            Assert.AreEqual(filesa, filesb);

            DeleteTestFolder();
        }

    }
}