using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;

namespace ExactaEasyCore
{
    public class DumpImagesConfiguration2
    {
        //const
        public const int c_version = 1;

        public int CurrentUserSettingsId { get; set; }
        public List<DumpImagesUserSettings2> AllSettings { get; set; }

        [XmlIgnore]
        public DumpImagesUserSettings2 BatchSettings { get; set; }
        [XmlIgnore]
        public List<DumpImagesUserSettings2> UserSettings { get; set; }

        //STATIC
        public static DumpImagesConfiguration2 LoadFromXML(string filePath, MachineConfiguration machineConf)
        {
            DumpImagesConfiguration2 dConf;
            if (File.Exists(filePath) == false)
            {
                dConf = new DumpImagesConfiguration2();
                dConf.CurrentUserSettingsId = 1;
                dConf.AllSettings = new List<DumpImagesUserSettings2>();
                dConf.AllSettings.Add(DumpImagesUserSettings2.GetNew(machineConf, 0, "Batch"));
                dConf.AllSettings.Add(DumpImagesUserSettings2.GetNew(machineConf, 1, "Production"));
                //save on xml
                dConf.SaveXml(filePath);
            }
            else
            {
                using (var reader = new StreamReader(filePath))
                {
                    var xmlSer = new XmlSerializer(typeof(DumpImagesConfiguration2));
                    dConf = (DumpImagesConfiguration2)xmlSer.Deserialize(reader);
                }
            }

            //check if list is not null
            if (dConf.AllSettings == null)
                dConf.AllSettings = new List<DumpImagesUserSettings2>();
            
            //check if exists batch type (save if necessary)
            int bfound = 0; //0=never 1=onlyID 2=OK
            foreach (DumpImagesUserSettings2 sett in dConf.AllSettings)
            {
                if (sett.Id == 0 && sett.Label == "Batch")
                    bfound = 2;
                if (sett.Id == 0 && sett.Label != "Batch")
                    bfound = 1;
            }
            if (bfound == 0)
                dConf.AllSettings.Insert(0, DumpImagesUserSettings2.GetNew(machineConf, 0, "Batch"));
            if (bfound == 1)
                foreach (DumpImagesUserSettings2 sett in dConf.AllSettings)
                    if (sett.Id == 0)
                        sett.Label = "Batch";
            if (bfound != 2)
                dConf.SaveXml(filePath);

            //check to add and set descriptions
            foreach (CameraSetting cam in machineConf.CameraSettings)
            {
                foreach (DumpImagesUserSettings2 usr in dConf.AllSettings)
                {
                    StationDumpSettings2 sf = null;
                    foreach (StationDumpSettings2 sett in usr.StationsDumpSettings)
                        if (sett.Node == cam.Node && sett.Station == cam.Station)
                            sf = sett;
                    if (sf == null)
                    {
                        sf = StationDumpSettings2.GetBase(cam.Node, cam.Station);
                        usr.StationsDumpSettings.Add(sf);
                    }
                    //set desc
                    sf.Description = null;
                    foreach (NodeSetting node in machineConf.NodeSettings)
                        foreach (StationSetting st in machineConf.StationSettings)
                            if (cam.Node == node.Id && cam.Station == st.Id)
                                sf.Description = $"{node.NodeDescription} - {st.StationDescription} - {cam.CameraDescription}";
                }
            }

            //check to remove and sort
            foreach (DumpImagesUserSettings2 usr in dConf.AllSettings)
            {
                List<StationDumpSettings2> toRemove = new List<StationDumpSettings2>();
                foreach (StationDumpSettings2 sett in usr.StationsDumpSettings)
                {
                    bool found = false;
                    foreach (CameraSetting cam in machineConf.CameraSettings)
                        if (sett.Node == cam.Node && sett.Station == cam.Station)
                            found = true;
                    if (found == false)
                        toRemove.Add(sett);
                }
                foreach (StationDumpSettings2 rem in toRemove)
                    usr.StationsDumpSettings.Remove(rem);

                //sort
                List<StationDumpSettings2> newSorted = new List<StationDumpSettings2>();
                foreach (CameraSetting cam in machineConf.CameraSettings)
                    foreach (StationDumpSettings2 sett in usr.StationsDumpSettings)
                        if (cam.Node == sett.Node && cam.Station == sett.Station)
                            newSorted.Add(sett);
                usr.StationsDumpSettings = newSorted;
            }

            //check n tools
            foreach(DumpImagesUserSettings2 usr in dConf.AllSettings)
            {
                foreach (StationDumpSettings2 sf in usr.StationsDumpSettings)
                    if (sf.SaveOnTool.Length != StationDumpSettings2.N_TOOLS)
                        Array.Resize<bool>(ref sf.SaveOnTool, StationDumpSettings2.N_TOOLS); //default bool=false
            }

            //lists
            dConf.BatchSettings = null;
            dConf.UserSettings = null;
            foreach(DumpImagesUserSettings2 usr in dConf.AllSettings)
            {
                if (usr.Id == 0)
                    dConf.BatchSettings = usr;
                else
                {
                    if (dConf.UserSettings == null)
                        dConf.UserSettings = new List<DumpImagesUserSettings2>();
                    dConf.UserSettings.Add(usr);
                }
            }

            return dConf;
        }

        //INSTANCE
        public void SaveXml(string filePath)
        {
            using(StreamWriter sw = new StreamWriter(filePath))
            {
                XmlSerializer serilize = new XmlSerializer(typeof(DumpImagesConfiguration2));
                serilize.Serialize(sw, this);
            }
        }
    }

    public class DumpImagesUserSettings2 : ICloneable
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public List<StationDumpSettings2> StationsDumpSettings { get; set; }

        public object Clone()
        {
            DumpImagesUserSettings2 sett = new DumpImagesUserSettings2();
            sett.Id = this.Id;
            sett.Label = this.Label;
            sett.StationsDumpSettings = this.StationsDumpSettings == null ? null : new List<StationDumpSettings2>();
            if (sett.StationsDumpSettings != null)
                foreach (StationDumpSettings2 sd in this.StationsDumpSettings)
                    sett.StationsDumpSettings.Add((StationDumpSettings2)sd.Clone());

            return sett;
        }

        public List<StationDumpSettings2> GetSettingsByNode(int node)
        {
            List<StationDumpSettings2> list = new List<StationDumpSettings2>();
            if (StationsDumpSettings != null)
                foreach (StationDumpSettings2 sett in StationsDumpSettings)
                    if (sett.Node == node)
                        list.Add(sett);
            return list;
        }

        public static DumpImagesUserSettings2 GetNew(MachineConfiguration conf, int id, string label)
        {
            DumpImagesUserSettings2 bas = new DumpImagesUserSettings2();
            bas.Id = id;
            bas.Label = label;
            bas.StationsDumpSettings = new List<StationDumpSettings2>();
            foreach (StationSetting st in conf.StationSettings)
            {
                StationDumpSettings2 nst = StationDumpSettings2.GetBase(st.Node, st.Id);
                bas.StationsDumpSettings.Add(nst);
            }
            return bas;
        }

        public static string GetJSONStr(List<StationDumpSettings2> setts)
        {
            if (setts == null)
                return null;
            if (setts.Count == 0)
                return "";

            JObject jobj = new JObject();
            jobj.Add("Version", DumpImagesConfiguration2.c_version);

            JArray jarr = new JArray();
            foreach (StationDumpSettings2 sett in setts)
                jarr.Add(sett.GetJSON());

            jobj.Add("Stations", jarr);
            return jobj.ToString();
        }

        public override string ToString()
        {
            return this.Label;
        }
    }

    public class StationDumpSettings2 : ICloneable
    {
        //const
        public const int N_TOOLS = 20;

        public bool Enable;
        public int Node;
        public int Station;
        public string Description;
        public int ID; //-1=ANY
        public StationDumpTypes2 Type;
        public StationDumpConditions2 Condition;
        public StationDumpLogics2 Logic;
        public StationDumpSamplings2 Sampling;
        public StationDumpPattern2 ConditionOnReject;
        public StationDumpPattern2 ConditionOnGood;
        public int ToSave;
        public bool[] SaveOnTool; //true(1) - false(0)

        public static StationDumpSettings2 GetBase(int node, int station)
        {
            StationDumpSettings2 nst = new StationDumpSettings2();
            nst.Enable = false;
            nst.Node = node;
            nst.Station = station;
            nst.ID = -1;
            nst.Type = StationDumpTypes2.Frames;
            nst.Condition = StationDumpConditions2.AnyCase;
            nst.Logic = StationDumpLogics2.Spindle;
            nst.Sampling = StationDumpSamplings2.None;
            nst.ConditionOnReject = StationDumpPattern2.GetBase();
            nst.ConditionOnGood = StationDumpPattern2.GetBase();
            nst.ToSave = 1000;
            nst.SaveOnTool = new bool[N_TOOLS];
            return nst;
        }

        public object Clone()
        {
            StationDumpSettings2 sett = new StationDumpSettings2();
            sett.Enable = this.Enable;
            sett.Node = this.Node;
            sett.Station = this.Station;
            sett.Description = (string)this.Description.Clone();
            sett.ID = this.ID;
            sett.Type = this.Type;
            sett.Condition = this.Condition;
            sett.Logic = this.Logic;
            sett.Sampling = this.Sampling;
            sett.ConditionOnReject = this.ConditionOnReject == null ? null : (StationDumpPattern2)this.ConditionOnReject.Clone();
            sett.ConditionOnGood = this.ConditionOnGood == null ? null : (StationDumpPattern2)this.ConditionOnGood.Clone();
            sett.ToSave = this.ToSave;
            sett.SaveOnTool = this.SaveOnTool == null ? null : new bool[this.SaveOnTool.Length];
            if (sett.SaveOnTool != null)
                Array.Copy(this.SaveOnTool, sett.SaveOnTool, sett.SaveOnTool.Length);
            return sett;
        }

        public JObject GetJSON()
        {
            JObject jobj = new JObject();
            jobj.Add("Enable", Enable ? 1 : 0);
            if (Enable)
            {
                jobj.Add("StationIndex", Station);
                jobj.Add("ID", ID);
                jobj.Add("Type", (int)Type);
                jobj.Add("Condition", (int)Condition);
                jobj.Add("Logic", (int)Logic);
                jobj.Add("Sampling", (int)Sampling);
                if (Condition == StationDumpConditions2.OnPattern)
                {
                    if (ConditionOnReject != null)
                        jobj.Add("ConditionOnReject", ConditionOnReject.GetJSON());
                    if (ConditionOnGood != null)
                        jobj.Add("ConditionOnGood", ConditionOnGood.GetJSON());
                }
                jobj.Add("ToSave", ToSave);
                if (SaveOnTool != null)
                {
                    JArray jarrSaveOnTool = new JArray();
                    foreach (bool sav in SaveOnTool)
                        jarrSaveOnTool.Add(sav ? 1 : 0);
                    jobj.Add("SaveOnTool", jarrSaveOnTool);
                }
            }

            return jobj;
        }
    }

    public enum StationDumpTypes2
    {
        Frames = 1,
        Thumbnail = 2,
        Result = 3,
        Hvld = 5,
    }

    public enum StationDumpConditions2
    {
        Never = 0,
        AnyCase = 1,
        OnGood = 2,
        OnReject = 3,
        OnTool = 4,
        OnPattern = 5,
    }

    public enum StationDumpLogics2
    {
        None = 0,
        Spindle = 1,
        Label = 2,
    }

    public enum StationDumpSamplings2
    {
        None = 0,
        Sub2 = 2,
        Sub4 = 4,
        Sub8 = 8,
        Sub16 = 16,
        Sub32 = 32,
    }

    public class StationDumpPattern2 : ICloneable
    {
        public StationDumpPatternTypes2 Type;
        public int ToSave;
        public int Every;

        public JObject GetJSON()
        {
            JObject jobj = new JObject();
            jobj.Add("Type", (int)Type);
            jobj.Add("ToSave", ToSave);
            jobj.Add("Every", Every);
            return jobj;
        }

        public static StationDumpPattern2 GetBase()
        {
            StationDumpPattern2 pat = new StationDumpPattern2();
            pat.Type = StationDumpPatternTypes2.Never;
            pat.ToSave = 1;
            pat.Every = 5;
            return pat;
        }

        public object Clone()
        {
            StationDumpPattern2 patt = new StationDumpPattern2();
            patt.Type = this.Type;
            patt.ToSave = this.ToSave;
            patt.Every = this.Every;
            return patt;
        }
    }

    public enum StationDumpPatternTypes2
    {
        Never = 0,
        EveryOnceIn = 1,
        Always = 2,
    }
}
