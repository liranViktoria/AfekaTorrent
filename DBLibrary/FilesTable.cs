﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DetailsLibrary;

namespace DBLibrary
{
    public class FilesTable
    {
        private static readonly FilesTable instance = new FilesTable();
        public static FilesTable Instance { get { return instance; } }
        private FilesTable() { }

        public List<File> GetFilesByName(String fileName)
        {
            List<File> files = new List<File>();
            using (AfekaMiniTorrent_DataBaseServerDataContext dataBase = new AfekaMiniTorrent_DataBaseServerDataContext())
            {
                foreach (var file in dataBase.Files)
                {
                    if(file.FileName==fileName)
                        files.Add(file);
                }  
            }
            return files;
        }

        public List<File> GetAllFiles()
        {
            List<File> files = new List<File>();
            using (AfekaMiniTorrent_DataBaseServerDataContext dataBase = new AfekaMiniTorrent_DataBaseServerDataContext())
            {
                foreach (var file in dataBase.Files)
                    files.Add(file);
                
            }
            return files;
        }

        public int GetNumOfAllFiles()
        {
            return GetAllFiles().Count();
        }

        public void AddFiles(String ip, String userName, List<FileDetails> files)
        {
            using (AfekaMiniTorrent_DataBaseServerDataContext dataBase = new AfekaMiniTorrent_DataBaseServerDataContext())
            {
                foreach (FileDetails details in files)
                {
                    File newFile = new File();
                    newFile.UserName = userName;
                    newFile.Ip = ip;
                    newFile.FileName = details.NameFile;
                    newFile.Size = details.NumberOfBytesOfFiles;
                    dataBase.Files.InsertOnSubmit(newFile);
                    dataBase.SubmitChanges();
                }
            }
        }

        public void DeleteFiles(string userName)
        {
            using (AfekaMiniTorrent_DataBaseServerDataContext dataBase = new AfekaMiniTorrent_DataBaseServerDataContext())
            {
                var filesToRemove = from f in dataBase.Files where f.UserName == userName select f;
                foreach (var f in filesToRemove)
                {
                    dataBase.Files.DeleteOnSubmit(f);
                    dataBase.SubmitChanges();
                }
            }
        }

        public List<MySearchResult> FindFile(string fileName)
        {
            using (AfekaMiniTorrent_DataBaseServerDataContext dataBase = new AfekaMiniTorrent_DataBaseServerDataContext())
            {
                var files = from f in dataBase.Files where f.FileName == fileName select f;
                var sizes = (from f in dataBase.Files where f.FileName == fileName select f.Size).Distinct();
                List<MySearchResult> searches = new List<MySearchResult>();
                foreach (var size in sizes)
                {
                    List<String> ips = new List<string>();
                    foreach (var file in files)
                    {
                        if (file.Size == size) { ips.Add(file.Ip); }
                    }
                    searches.Add(new MySearchResult(fileName, size, ips));
                }
                if (searches.Count() != 0) return searches;
            }
            return null;
        }

    }//end of class FilesTable

}//end of namespace
