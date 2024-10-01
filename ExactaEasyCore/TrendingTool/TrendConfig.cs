using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Data.SQLite;
using System.Data;

namespace ExactaEasyCore.TrendingTool
{
    public class TrendConfig
    {
        //private fields
        string _path;
        readonly static object _objLock = new object();

        //serialize
        public List<TrendConfigParSaved> ParsSaved { get; set; }
        public List<TrendConfigFileSaved> FilesSaved { get; set; }

        //PathConfig -- ignore xml serializer
        [XmlIgnore]
        public string PathConfig => _path;

        //private constructor
        private TrendConfig() { }


        public static TrendConfig Load(string path)
        {
            TrendConfig conf = null;

            if(TrendTool.FileTypeSaving == 1)
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(TrendConfig));
                    conf = (TrendConfig)serializer.Deserialize(sr);
                }
            }
            if(TrendTool.FileTypeSaving == 2)
            {
                conf = new TrendConfig();
                conf.ParsSaved = TrendTool.SQLiteConfigGetParsSaved(path);
                conf.FilesSaved = TrendTool.SQLiteConfigGetFilesSaved(path);
            }

            //path
            conf._path = path;
            //check
            if (conf.ParsSaved == null)
                conf.ParsSaved = new List<TrendConfigParSaved>();
            if (conf.FilesSaved == null)
                conf.FilesSaved = new List<TrendConfigFileSaved>();

            return conf;
        }

        public static TrendConfig CreateNew(string path)
        {
            TrendConfig data = new TrendConfig();
            data._path = path;
            if(TrendTool.FileTypeSaving == 1)
                data.Save();
            if (TrendTool.FileTypeSaving == 2)
                TrendTool.SQLiteCreateNewDB(path, true);
            return data;
        }

        public void Save()
        {
            //use lock because it can be called from a different thread
            lock (_objLock)
            {
                //check folder
                string directory = Path.GetDirectoryName(_path);
                if (Directory.Exists(directory) == false)
                    Directory.CreateDirectory(directory);

                //backup file
                string fileName = Path.GetFileNameWithoutExtension(_path);
                string fileExtension = Path.GetExtension(_path);
                string newFilePath = $"{directory}\\{fileName}.{DateTime.Now.ToString("yyyyMMdd_HHmmss_fff")}{fileExtension}";
                if (File.Exists(_path))
                {
                    File.Copy(_path, newFilePath);
                }

                //delete backup files
                string[] files = Directory.GetFiles(directory, $"{fileName}.*").OrderByDescending(s => s).ToArray();
                List<string> listToDelete = new List<string>();
                int countToDel = 0;
                foreach (string file in files)
                {
                    try
                    {
                        string[] splits = Path.GetFileName(file).Split('.');
                        if (DateTime.TryParseExact(splits[1], "yyyyMMdd_HHmmss_fff", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _))
                        {
                            countToDel++;
                            if (countToDel >= 16)
                                listToDelete.Add(file);
                        }
                    }
                    catch { }
                }
                foreach (string file in listToDelete)
                    File.Delete(file);

                //save
                if(TrendTool.FileTypeSaving == 1)
                {
                    //serialize
                    using (StreamWriter sw = new StreamWriter(_path))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(TrendConfig));
                        serializer.Serialize(sw, this);
                    }
                }
                if(TrendTool.FileTypeSaving == 2)
                {
                    //parSaved
                    TrendTool.SQLiteConfigDeleteParsSaved(_path);
                    foreach (TrendConfigParSaved par in ParsSaved)
                    {
                        TrendTool.SQLiteConfigAddParSaved(_path, par);
                    }

                    //fileSaved
                    foreach(TrendConfigFileSaved file in FilesSaved)
                    {
                        if (TrendTool.SQLiteConfigGetIdFileSaved(_path, file) < 0)
                            TrendTool.SQLiteConfigAddFileSaved(_path, file);
                    }
                }
            }
        }
    }
}
