using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;
using SPAMI.Util.Logger;
using System.Data.SQLite;

namespace ExactaEasyCore.TrendingTool
{
    public class TrendTool
    {
        /// <summary>
        /// 1=CSV 2=SQLITE
        /// <para>
        /// NOTE 2023-06-19:
        /// <br>The SQL solution has been found to be slow in writing compared to CSV,</br>
        /// <br>for the same amount of data it takes 30 times as long (10000 values CSV=1.8s SQL=65.7s),</br>
        /// <br>the code regarding SQL will be deprecated, if this solution is to be used it must be optimized.</br>
        /// </para>
        /// </summary>
        public const int FileTypeSaving = 1;
        public const string NameConfigFile = FileTypeSaving == 1 ? "config.xml" : "config.db";
        public const string FormatDateTime = "yyyy-MM-dd HH:mm:ss.fff";

        public string FolderStorage { get; private set; }
        public TrendConfig Config { get; private set; }
        public bool IsReady { get; private set; }

        List<(TrendConfigParSaved, int, string)> _valuesToWrite;
        Timer _timer;
        bool _flagTimer = false;
        int _countErrorsCatch = 0;
        readonly static object _objLock = new object();

        //private constructor
        private TrendTool() { }


        public static TrendTool Load(bool enable, string folderStorage)
        {
            TrendTool data = new TrendTool();
            data.IsReady = enable;

            if (enable == false)
            {
                return data;
            }
            else
            {
                try
                {
                    //check path
                    if (Directory.Exists(folderStorage) == false)
                        //throw new ArgumentException($"path of folder storage '{folderStorage}' is not valid");
                        Directory.CreateDirectory(folderStorage);
                    data.FolderStorage = folderStorage;
                    if (data.FolderStorage.EndsWith("\\") == false)
                        data.FolderStorage += "\\";

                    //create new config file if it not exists
                    if(TrendTool.FileTypeSaving == 1)
                    {
                        if(File.Exists(data.FolderStorage + NameConfigFile) == false)
                            TrendConfig.CreateNew(data.FolderStorage + NameConfigFile);
                    }
                    if(TrendTool.FileTypeSaving == 2)
                    {
                        string pathFull = data.FolderStorage + NameConfigFile;
                        if (File.Exists(pathFull) == false)
                            TrendConfig.CreateNew(pathFull);
                        else
                        {
                            FileInfo info = new FileInfo(pathFull); //check if size is 0
                            if(info.Length == 0)
                            {
                                File.Delete(pathFull);
                                TrendConfig.CreateNew(pathFull);
                            }
                        }
                    }

                    //load config
                    data.Config = TrendConfig.Load(data.FolderStorage + NameConfigFile);

                    data._valuesToWrite = new List<(TrendConfigParSaved, int, string)>();
                    if(data._timer == null)
                    {
                        data._flagTimer = false;
                        data._timer = new Timer();
                        data._timer.Interval = 5000;
                        data._timer.Elapsed += data._timer_Elapsed;
                        data._timer.AutoReset = true;
                        data._timer.Start();
                    }

                    data._countErrorsCatch = 0;
                }
                catch(Exception ex)
                {
                    data.IsReady = false;
                    data.Config = null;
                    Log.Line(LogLevels.Error, "TrendTool.Load", "An error occurred while load the TrendTool: " + ex.Message);
                }

                return data;
            }
        }


        public void InsertValue(TrendConfigParSaved par, int batchId, string value)
        {
            if (IsReady == false)
                return;

            if (batchId < 0)
                return;

            lock (_objLock)
            {
                if (false) //true if testing
                {
                    Random random = new Random();
                    for (int i = 0; i < 1000; i++)
                    {
                        double val = (double)(1 / (double)random.Next(0, 2000));
                        val += random.Next(0, 2000); //add integer
                        _valuesToWrite.Add((par, batchId, val.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                    }
                }
                else
                {
                    _valuesToWrite.Add((par, batchId, value));
                }
            }
        }


        public List<string> GetListUsedRecipes()
        {
            if (IsReady == false)
                return null;

            List<string> list = new List<string>();
            foreach(TrendConfigFileSaved fileSav in Config.FilesSaved)
            {
                if (list.Contains(fileSav.RecipeName) == false)
                    list.Add(fileSav.RecipeName);
            }
            return list;
        }


        public List<int> GetListUsedBatchId(string recipeName)
        {
            if (IsReady == false)
                return null;

            List<int> list = new List<int>();
            foreach(TrendConfigFileSaved fileSav in Config.FilesSaved)
            {
                if(fileSav.RecipeName == recipeName)
                {
                    if (list.Contains(fileSav.BatchId) == false)
                        list.Add(fileSav.BatchId);
                }
            }

            return list;
        }


        public Dictionary<TrendConfigFileSaved, string> GetListUsedStationTool(string recipeName, int batchId)
        {
            if (IsReady == false)
                return null;

            Dictionary<TrendConfigFileSaved, string> dic = new Dictionary<TrendConfigFileSaved, string>();
            foreach(TrendConfigFileSaved fileSav in Config.FilesSaved)
            {
                if(fileSav.RecipeName == recipeName && fileSav.BatchId == batchId)
                {
                    string nameDesc = $"{fileSav.StationName} - {fileSav.ToolName} - {fileSav.ParameterName}";
                    dic.Add(fileSav, nameDesc);
                }
            }

            return dic;
        }


        public List<KeyValuePair<DateTime, double>> ReadValues(TrendConfigFileSaved file)
        {
            if (IsReady == false)
                return null;

            List<KeyValuePair<DateTime, double>> listRes = new List<KeyValuePair<DateTime, double>>();

            string path = FolderStorage + file.FilePath;
            if (File.Exists(path) == false)
                return null;

            if(TrendTool.FileTypeSaving == 1)
            {
                int count = 0;
                string line;
                using(StreamReader sr = new StreamReader(path))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if(count != 0) //first line is columns description
                        {
                            try
                            {
                                string[] values = line.Split(';');
                                DateTime dt = DateTime.Parse(values[0]);
                                double val = double.Parse(values[1], System.Globalization.CultureInfo.InvariantCulture);

                                listRes.Add(new KeyValuePair<DateTime, double>(dt, val));
                            }
                            catch(Exception ex)
                            {
                                Log.Line(LogLevels.Error, "TrendTool.GetValues", "An error occurred while reading values: " + ex.Message);
                            }
                        }
                        count++;
                    }
                }
            }
            if(TrendTool.FileTypeSaving == 2)
            {
                int idFile = TrendTool.SQLiteConfigGetIdFileSaved(Config.PathConfig, file);
                if (idFile >= 0)
                {
                    List<KeyValuePair<DateTime, double>> listBuff = new List<KeyValuePair<DateTime, double>>();
                    TrendTool.SQLiteValuesReadValue(path, idFile, (stDatetime, stValue) =>
                    {
                        try
                        {
                            DateTime dt = DateTime.Parse(stDatetime);
                            double val = double.Parse(stValue, System.Globalization.CultureInfo.InvariantCulture);

                            listBuff.Add(new KeyValuePair<DateTime, double>(dt, val));
                        }
                        catch (Exception ex)
                        {
                            Log.Line(LogLevels.Error, "TrendTool.GetValues", "An error occurred while reading values: " + ex.Message);
                        }
                    });

                    listRes = listBuff;
                }
            }

            return listRes;
        }


        /// <summary>
        /// read from all batchs
        /// </summary>
        /// <param name="recipeName"></param>
        public List<KeyValuePair<DateTime, double>> ReadValues(string recipeName, string nameDesc)
        {
            if (IsReady == false)
                return null;

            List<KeyValuePair<DateTime, double>> listRes = new List<KeyValuePair<DateTime, double>>();
            List<int> batches = GetListUsedBatchId(recipeName);

            foreach(int batchId in batches)
            {
                Dictionary<TrendConfigFileSaved, string> dicFiles = GetListUsedStationTool(recipeName, batchId);
                foreach(KeyValuePair<TrendConfigFileSaved, string> kvp in dicFiles)
                {
                    if(kvp.Value == nameDesc)
                    {
                        List<KeyValuePair<DateTime, double>> resBuff = ReadValues(kvp.Key);
                        foreach(KeyValuePair<DateTime, double> kvpReaded in resBuff)
                        {
                            listRes.Add(kvpReaded);
                        }
                    }
                }
            }

            return listRes;
        }




        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (IsReady == false)
                return;
            if (_flagTimer == true)
                return;
            _flagTimer = true;

            //clone the list to avoid changing it during the iteration
            List<(TrendConfigParSaved, int, string)> itemsTmp = new List<(TrendConfigParSaved, int, string)>();
            lock (_objLock)
            {
                foreach ((TrendConfigParSaved, int, string) item in _valuesToWrite)
                {
                    TrendConfigParSaved parTmp = (TrendConfigParSaved)item.Item1.Clone();
                    string valueTmp = (string)item.Item3.Clone();
                    itemsTmp.Add((parTmp, item.Item2, valueTmp));
                }
                _valuesToWrite.Clear();
            }

            foreach ((TrendConfigParSaved, int, string) item in itemsTmp)
            {
                TrendConfigParSaved par = item.Item1;
                int batchId = item.Item2;
                string value = item.Item3;

                try
                {
                    //check folder and file
                    string pathDirRecipe = null;
                    string pathFile = null;
                    if(TrendTool.FileTypeSaving == 1)
                    {
                        pathDirRecipe = $"{FolderStorage}{FilterValidCharactersPath(par.RecipeName)}";
                        if (Directory.Exists(pathDirRecipe) == false)
                            Directory.CreateDirectory(pathDirRecipe);

                        pathFile = $"{pathDirRecipe}\\Batch_{batchId}_{par.StationName}_TOOL_{par.ToolIndex.ToString("d2")} - {par.ToolName}_{par.ParameterName}.csv";
                        if (File.Exists(pathFile) == false)
                        {
                            using (FileStream fs = File.Create(pathFile)) { }
                            using (StreamWriter sw = File.AppendText(pathFile))
                            {
                                string cols = "Date;Value";
                                sw.WriteLine(cols);
                            }
                        }
                    }
                    if(TrendTool.FileTypeSaving == 2)
                    {
                        pathDirRecipe = FolderStorage;
                        pathFile = $"{pathDirRecipe}{FilterValidCharactersPath(par.RecipeName)}.db";
                        if (File.Exists(pathFile) == false)
                            TrendTool.SQLiteCreateNewDB(pathFile, false);
                    }

                    //check exists file saved in config
                    TrendConfigFileSaved fileSaved = new TrendConfigFileSaved(par.RecipeName, par.StationName, par.ToolName, par.ParameterName, batchId, pathFile.Replace(FolderStorage, ""));
                    bool found = false;
                    foreach (TrendConfigFileSaved file in Config.FilesSaved)
                        if (file.IsEqualTo(fileSaved))
                            found = true;
                    if(found == false)
                    {
                        Config.FilesSaved.Add(fileSaved);
                        Config.Save();
                    }

                    //insert values
                    if (TrendTool.FileTypeSaving == 1)
                    {
                        using (StreamWriter sw = File.AppendText(pathFile))
                        {
                            string line = $"{DateTime.Now.ToString(FormatDateTime)};{value}";
                            sw.WriteLine(line);
                        }
                    }
                    if(TrendTool.FileTypeSaving == 2)
                    {
                        int idFile = TrendTool.SQLiteConfigGetIdFileSaved(Config.PathConfig, fileSaved);
                        if(idFile >= 0)
                        {
                            TrendTool.SQLiteValuesInsertValue(pathFile, idFile, DateTime.Now.ToString(FormatDateTime), value);
                        }
                    }

                    _countErrorsCatch = 0;
                }
                catch (Exception ex)
                {
                    if(_countErrorsCatch < 10)
                    {
                        Log.Line(LogLevels.Error, "TrendTool.SaveValues", "An error occurred while saving values: " + ex.Message);
                        _countErrorsCatch++;
                    }
                }
            }

            _flagTimer = false;
        }




        string FilterValidCharactersPath(string input)
        {
            string res = input;
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            foreach (char c in invalid)
            {
                res = res.Replace(c.ToString(), "");
            }
            return res;
        }





        //SQLITE MANAGEMENT
        public static void SQLiteCreateNewDB(string path, bool isConfig)
        {
            SQLiteConnection.CreateFile(path);
            using (SQLiteConnection con = new SQLiteConnection($"Data Source = {path}"))
            {
                con.Open();
                string query = null;
                if (isConfig)
                {
                    query += "BEGIN TRANSACTION;";
                    query += "DROP TABLE IF EXISTS \"ParSaved\";\n";
                    query += "CREATE TABLE IF NOT EXISTS \"ParSaved\" (\"Id\" INTEGER NOT NULL UNIQUE, \"Xml\" TEXT, PRIMARY KEY(\"Id\" AUTOINCREMENT));\n";
                    query += "DROP TABLE IF EXISTS \"FileSaved\";\n";
                    query += "CREATE TABLE IF NOT EXISTS \"FileSaved\" (\"Id\" INTEGER NOT NULL UNIQUE, \"Xml\" TEXT, PRIMARY KEY(\"Id\" AUTOINCREMENT));\n";
                    query += "COMMIT;\n";
                }
                else
                {
                    query += "BEGIN TRANSACTION;";
                    query += "DROP TABLE IF EXISTS \"Values\";\n";
                    query += "CREATE TABLE IF NOT EXISTS \"Values\" (\"IdFileSaved\" INTEGER, \"DateTime\" TEXT, \"Value\" TEXT);\n";
                    query += "COMMIT;\n";
                }
                using (SQLiteCommand comm = new SQLiteCommand(query, con))
                {
                    comm.ExecuteNonQuery();
                }
            }
        }

        public static List<TrendConfigParSaved> SQLiteConfigGetParsSaved(string path)
        {
            List<TrendConfigParSaved> list = new List<TrendConfigParSaved>();
            using (SQLiteConnection con = new SQLiteConnection($"Data Source = {path}"))
            {
                con.Open();
                using (SQLiteCommand comm = new SQLiteCommand("SELECT * FROM ParSaved", con))
                {
                    using (SQLiteDataReader reader = comm.ExecuteReader())
                    {
                        int ordXml = reader.GetOrdinal("Xml");
                        while (reader.Read())
                        {
                            string xml = reader.GetString(ordXml);
                            list.Add(TrendConfigParSaved.FromXML(xml));
                        }
                    }
                }
            }
            return list;
        }

        public static List<TrendConfigFileSaved> SQLiteConfigGetFilesSaved(string path)
        {
            List<TrendConfigFileSaved> list = new List<TrendConfigFileSaved>();
            using (SQLiteConnection con = new SQLiteConnection($"Data Source = {path}"))
            {
                con.Open();
                using (SQLiteCommand comm = new SQLiteCommand("SELECT * FROM FileSaved", con))
                {
                    using (SQLiteDataReader reader = comm.ExecuteReader())
                    {
                        int ordXml = reader.GetOrdinal("Xml");
                        while (reader.Read())
                        {
                            string xml = reader.GetString(ordXml);
                            list.Add(TrendConfigFileSaved.FromXML(xml));
                        }
                    }
                }
            }
            return list;
        }

        public static void SQLiteConfigAddParSaved(string path, TrendConfigParSaved par)
        {
            using (SQLiteConnection con = new SQLiteConnection($"Data Source = {path}"))
            {
                con.Open();
                string xml = par.ToXML();
                using (SQLiteCommand comm = new SQLiteCommand($"INSERT INTO ParSaved (Xml) VALUES ('{xml}')", con))
                {
                    comm.ExecuteNonQuery();
                }
            }
        }

        public static void SQLiteConfigAddFileSaved(string path, TrendConfigFileSaved file)
        {
            using (SQLiteConnection con = new SQLiteConnection($"Data Source = {path}"))
            {
                con.Open();
                string xml = file.ToXML();
                using (SQLiteCommand comm = new SQLiteCommand($"INSERT INTO FileSaved (Xml) VALUES ('{xml}')", con))
                {
                    comm.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// returns the id of the row where there is a match, returns -1 if nothing was found
        /// </summary>
        public static int SQLiteConfigGetIdParSaved(string path, TrendConfigParSaved par)
        {
            int res = -1;
            using (SQLiteConnection con = new SQLiteConnection($"Data Source = {path}"))
            {
                con.Open();
                string xml = par.ToXML();
                using (SQLiteCommand comm = new SQLiteCommand($"SELECT Id FROM ParSaved WHERE Xml='{xml}'", con))
                {
                    object obj = comm.ExecuteScalar();
                    if (obj != null)
                        res = (int)obj;
                }
            }
            return res;
        }

        /// <summary>
        /// returns the id of the row where there is a match, returns -1 if nothing was found
        /// </summary>
        public static int SQLiteConfigGetIdFileSaved(string path, TrendConfigFileSaved file)
        {
            int res = -1;
            using (SQLiteConnection con = new SQLiteConnection($"Data Source = {path}"))
            {
                con.Open();
                string xml = file.ToXML();
                using (SQLiteCommand comm = new SQLiteCommand($"SELECT Id FROM FileSaved WHERE Xml='{xml}'", con))
                {
                    object obj = comm.ExecuteScalar();
                    if (obj != null)
                        res = Convert.ToInt32(obj);
                }
            }
            return res;
        }

        public static void SQLiteConfigDeleteParsSaved(string path)
        {
            using (SQLiteConnection con = new SQLiteConnection($"Data Source = {path}"))
            {
                con.Open();
                using (SQLiteCommand comm = new SQLiteCommand($"DELETE FROM ParSaved", con))
                {
                    comm.ExecuteScalar();
                }
            }
        }

        public static void SQLiteConfigDeleteFilesSaved(string path)
        {
            using (SQLiteConnection con = new SQLiteConnection($"Data Source = {path}"))
            {
                con.Open();
                using (SQLiteCommand comm = new SQLiteCommand($"DELETE FROM FileSaved", con))
                {
                    comm.ExecuteScalar();
                }
            }
        }

        public static void SQLiteValuesReadValue(string path, int id, Action<string, string> func)
        {
            using (SQLiteConnection con = new SQLiteConnection($"Data Source = {path}"))
            {
                con.Open();
                using (SQLiteCommand comm = new SQLiteCommand($"SELECT DateTime, Value FROM 'Values' WHERE IdFileSaved={id}", con)) //attention to keywords on the table name (Values)
                {
                    using(SQLiteDataReader reader = comm.ExecuteReader())
                    {
                        int ordDateTime = reader.GetOrdinal("DateTime");
                        int ordValue = reader.GetOrdinal("Value");
                        while (reader.Read())
                        {
                            string dateTime = reader.GetString(ordDateTime);
                            string value = reader.GetString(ordValue);
                            func.Invoke(dateTime, value);
                        }
                    }
                }
            }
        }

        public static void SQLiteValuesInsertValue(string path, int id, string dateTime, string value)
        {
            using (SQLiteConnection con = new SQLiteConnection($"Data Source = {path}"))
            {
                con.Open();
                using (SQLiteCommand comm = new SQLiteCommand($"INSERT INTO 'Values' (IdFileSaved, DateTime, Value) VALUES ({id}, '{dateTime}', '{value}')", con)) //attention to keywords on the table name (Values)
                {
                    comm.ExecuteNonQuery();
                }
            }
        }
    }
}
