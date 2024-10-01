using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;

namespace EoptisClient
{

    public class EoptisParameters
    {

        //public static EoptisParameters LoadFromFile(string filePath) {

        //    EoptisParameters eoptisParams = null;
        //    using (StreamReader reader = new StreamReader(filePath)) {
        //        string xmlString = reader.ReadToEnd();
        //        eoptisParams = LoadFromXml(xmlString);
        //    }
        //    return eoptisParams;
        //}

        //public static EoptisParameters LoadFromXml(string xmlString) {

        //    EoptisParameters eoptisParams = null;
        //    using (StringReader reader = new StringReader(xmlString)) {

        //        XmlSerializer xmlSer = new XmlSerializer(typeof(EoptisParameters));
        //        eoptisParams = (EoptisParameters)xmlSer.Deserialize(reader);
        //    }
        //    return eoptisParams;
        //}

        public EoptisParameterCollection Parameters { get; set; }

        public EoptisParameters() {

            Parameters = new EoptisParameterCollection();
        }
    }

    public class EoptisParameterCollection : List<EoptisParameter>
    {

        [DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        internal static unsafe extern int getParameterType(int paramId, ref int paramType, ref int paramSize);

        public EoptisParameter this[SYSTEM_ID id] {
            get {
                EoptisParameter resEp = this.Find(ep => ep.Id == id);
                if (resEp == null) {
                    int byteLength = 0, type = 0;
                    getParameterType((int)id, ref type, ref byteLength);
                    resEp = new EoptisParameter(id, (SYSTEM_TYPE)type, byteLength);
                    switch (id) {
                    case SYSTEM_ID.LIGHT_CONTROL:
                        resEp.Threshold.Add(SYSTEM_ID.LIGHT_CONTROL_TH);
                        break;
                    case SYSTEM_ID.PAR_RANGE_1_MEAN:
                        resEp.Threshold.Add(SYSTEM_ID.PAR_RANGE_1_PEAK_REJECT_TH);
                        break;
                    case SYSTEM_ID.PAR_RANGE_2_MEAN:
                        resEp.Threshold.Add(SYSTEM_ID.PAR_RANGE_2_PEAK_REJECT_TH);
                        break;
                    case SYSTEM_ID.PAR_RANGE_3_MEAN:
                        resEp.Threshold.Add(SYSTEM_ID.PAR_RANGE_3_PEAK_REJECT_TH);
                        break;
                    case SYSTEM_ID.PAR_RANGE_4_MEAN:
                        resEp.Threshold.Add(SYSTEM_ID.PAR_RANGE_4_PEAK_REJECT_TH);
                        break;
                    case SYSTEM_ID.PAR_ALL_RANGE_MEAN:
                        resEp.Threshold.Add(SYSTEM_ID.PAR_ALL_RANGE_SUM_REJECT);
                        break;
                    default:
                        break;
                    }
                    this.Add(resEp);
                }
                return resEp;
            }
        }
    }

    public class EoptisParameter
    {

        public SYSTEM_ID Id { get; set; }
        public SYSTEM_TYPE Type { get; set; }
        public int Size { get; set; }
        public int Length { get; set; }
        public int ByteLength { get; set; }
        [XmlIgnore]
        public string Value { get; set; }
        [XmlIgnore]
        public List<SYSTEM_ID> Threshold { get; set; }

        public EoptisParameter() {

            Threshold = new List<SYSTEM_ID>();
            Type = SYSTEM_TYPE.TYPEUNKNOWN;
            ByteLength = 0;
            Size = 0;
            Length = 0;
        }

        public EoptisParameter(SYSTEM_ID id)
            : this() {

            Id = id;
        }

        public EoptisParameter(SYSTEM_ID id, SYSTEM_TYPE type, int byteLength)
            : this() {

            Id = id;
            Type = type;
            ByteLength = byteLength;
            switch (type) {
            case SYSTEM_TYPE.TYPEBOOL:
            case SYSTEM_TYPE.TYPECHAR:
            case SYSTEM_TYPE.TYPEUNSIGNEDCHAR:
            case SYSTEM_TYPE.TYPESTRING:
                Size = 1;
                break;
            case SYSTEM_TYPE.TYPESHORT:
            case SYSTEM_TYPE.TYPESHORTBUFFER:
            case SYSTEM_TYPE.TYPEUNSIGNEDSHORTBUFFER:
            case SYSTEM_TYPE.TYPEMEASURE:
                Size = 2;
                break;
            case SYSTEM_TYPE.TYPEINT:
            case SYSTEM_TYPE.TYPEUNSIGNEDINT:
            case SYSTEM_TYPE.TYPEFLOAT:
                Size = 4;
                break;
            case SYSTEM_TYPE.TYPELONG:
            case SYSTEM_TYPE.TYPEUNSIGNEDLONG:
            case SYSTEM_TYPE.TYPEDOUBLE:
                Size = 8;
                break;
            default:
                Size = 0;
                break;
            }
            Length = (Size > 0) ? ByteLength / Size : 0;
        }

        internal string GetValue(byte[] value) {

            string res = "";
            for (int iL = 0; iL < Length; iL++) {
                switch (Type) {
                case SYSTEM_TYPE.TYPEBOOL:
                    res += (Length == 1) ?
                        BitConverter.ToBoolean(value, iL * Size).ToString(CultureInfo.InvariantCulture) :
                        iL.ToString() + ";" + BitConverter.ToBoolean(value, iL * Size).ToString(CultureInfo.InvariantCulture) + ";";
                    break;
                case SYSTEM_TYPE.TYPECHAR:
                case SYSTEM_TYPE.TYPEUNSIGNEDCHAR:
                    res += (Length == 1) ?
                        value[iL * Size].ToString() :
                        iL.ToString() + ";" + value[iL * Size].ToString() + ";";
                    break;
                case SYSTEM_TYPE.TYPESHORT:
                case SYSTEM_TYPE.TYPESHORTBUFFER:
                    res += (Length == 1) ?
                        BitConverter.ToInt16(value, iL * Size).ToString(CultureInfo.InvariantCulture).Replace(",", "") :
                        iL.ToString() + ";" + BitConverter.ToInt16(value, iL * Size).ToString(CultureInfo.InvariantCulture).Replace(",", "") + ";";
                    break;
                case SYSTEM_TYPE.TYPEUNSIGNEDSHORTBUFFER:
                    res += (Length == 1) ?
                        BitConverter.ToUInt16(value, iL * Size).ToString(CultureInfo.InvariantCulture).Replace(",", "") :
                        iL.ToString() + ";" + BitConverter.ToUInt16(value, iL * Size).ToString(CultureInfo.InvariantCulture).Replace(",", "") + ";";
                    break;
                case SYSTEM_TYPE.TYPEINT:
                    res += (Length == 1) ?
                        BitConverter.ToInt32(value, iL * Size).ToString(CultureInfo.InvariantCulture).Replace(",", "") :
                        iL.ToString() + ";" + BitConverter.ToInt32(value, iL * Size).ToString(CultureInfo.InvariantCulture).Replace(",", "") + ";";
                    break;
                case SYSTEM_TYPE.TYPEUNSIGNEDINT:
                    res += (Length == 1) ?
                        BitConverter.ToUInt32(value, iL * Size).ToString(CultureInfo.InvariantCulture).Replace(",", "") :
                        iL.ToString() + ";" + BitConverter.ToUInt32(value, iL * Size).ToString(CultureInfo.InvariantCulture).Replace(",", "") + ";";
                    break;
                case SYSTEM_TYPE.TYPELONG:
                    res += (Length == 1) ?
                        BitConverter.ToInt64(value, iL * Size).ToString(CultureInfo.InvariantCulture).Replace(",", "") :
                        iL.ToString() + ";" + BitConverter.ToInt64(value, iL * Size).ToString(CultureInfo.InvariantCulture).Replace(",", "") + ";";
                    break;
                case SYSTEM_TYPE.TYPEUNSIGNEDLONG:
                    res += (Length == 1) ?
                        BitConverter.ToUInt64(value, iL * Size).ToString(CultureInfo.InvariantCulture).Replace(",", "") :
                        iL.ToString() + ";" + BitConverter.ToUInt64(value, iL * Size).ToString(CultureInfo.InvariantCulture).Replace(",", "") + ";";
                    break;
                case SYSTEM_TYPE.TYPEFLOAT:
                    res += (Length == 1) ?
                        BitConverter.ToSingle(value, iL * Size).ToString(CultureInfo.InvariantCulture).Replace(",", "") :
                        iL.ToString() + ";" + BitConverter.ToSingle(value, iL * Size).ToString(CultureInfo.InvariantCulture).Replace(",", "") + ";";
                    break;
                case SYSTEM_TYPE.TYPEDOUBLE:
                    res += (Length == 1) ?
                        BitConverter.ToDouble(value, iL * Size).ToString(CultureInfo.InvariantCulture).Replace(",", "") :
                        iL.ToString() + ";" + BitConverter.ToDouble(value, iL * Size).ToString(CultureInfo.InvariantCulture).Replace(",", "") + ";";
                    break;
                case SYSTEM_TYPE.TYPEMEASURE:
                    //for (int i = 0; i < ByteLength / sizeof(short); i++) {
                    short val = BitConverter.ToInt16(value, iL * Size);
                    res += iL.ToString() + ";" + "0x" + val.ToString("X2", CultureInfo.InvariantCulture).Replace(",", "") + ";" + val.ToString(CultureInfo.InvariantCulture).Replace(",", "") + ";";
                    //}
                    break;
                case SYSTEM_TYPE.TYPESTRING:
                    string[] values = Encoding.Default.GetString(value).Split(new char[] { '\n', '\0' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string curr_string in values) {
                        res += curr_string;
                    }
                    return res;
                case SYSTEM_TYPE.TYPEUNKNOWN:
                    throw new Exception("Invalid parameter type");
                default:
                    throw new Exception("Invalid parameter type");
                }
            }
            return res;
        }

        internal byte[] SetValue(string value) {

            byte[] res = new byte[ByteLength];
            byte[] buffer;
            string[] values = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int iL = 0; iL < values.Length; iL++) {
                if (iL % 2 == 0 && values.Length > 1)
                    continue;
                int iBuffer = Math.Max(0, (iL - 1) * Size / 2);
                switch (Type) {
                case SYSTEM_TYPE.TYPEBOOL:
                    buffer = new byte[1];
                    buffer[0] = Convert.ToBoolean(values[iL]) ? (byte)1 : (byte)0;
                    Array.Copy(buffer, 0, res, iBuffer, buffer.Length);
                    break;
                case SYSTEM_TYPE.TYPECHAR:
                case SYSTEM_TYPE.TYPEUNSIGNEDCHAR:
                    byte b1 = Convert.ToByte(values[iL]);
                    buffer = BitConverter.GetBytes(b1);
                    Array.Copy(buffer, 0, res, iBuffer, buffer.Length);
                    break;
                case SYSTEM_TYPE.TYPESHORT:
                case SYSTEM_TYPE.TYPESHORTBUFFER:
                    short w0 = Convert.ToInt16(values[iL]);
                    buffer = BitConverter.GetBytes(w0);
                    Array.Copy(buffer, 0, res, iBuffer, buffer.Length);
                    break;
                case SYSTEM_TYPE.TYPEUNSIGNEDSHORTBUFFER:
                    ushort w1 = Convert.ToUInt16(values[iL]);
                    buffer = BitConverter.GetBytes(w1);
                    Array.Copy(buffer, 0, res, iBuffer, buffer.Length);
                    break;
                case SYSTEM_TYPE.TYPEINT:
                    int dw0 = Convert.ToInt32(values[iL]);
                    buffer = BitConverter.GetBytes(dw0);
                    Array.Copy(buffer, 0, res, iBuffer, buffer.Length);
                    break;
                case SYSTEM_TYPE.TYPEUNSIGNEDINT:
                    uint dw1 = Convert.ToUInt32(values[iL]);
                    buffer = BitConverter.GetBytes(dw1);
                    Array.Copy(buffer, 0, res, iBuffer, buffer.Length);
                    break;
                case SYSTEM_TYPE.TYPELONG:
                case SYSTEM_TYPE.TYPEUNSIGNEDLONG:
                    throw new NotImplementedException();
                //break;
                case SYSTEM_TYPE.TYPEFLOAT:
                    float fVal = Convert.ToSingle(values[iL], CultureInfo.InvariantCulture);
                    buffer = BitConverter.GetBytes(fVal);
                    Array.Copy(buffer, 0, res, iBuffer, buffer.Length);
                    break;
                case SYSTEM_TYPE.TYPEDOUBLE:
                    double dVal = Convert.ToDouble(values[iL], CultureInfo.InvariantCulture);
                    buffer = BitConverter.GetBytes(dVal);
                    Array.Copy(buffer, 0, res, iBuffer, buffer.Length);
                    break;
                case SYSTEM_TYPE.TYPEMEASURE:
                    //for (int i = 0; i < values.Length; i++) {
                    short w2 = Convert.ToInt16(values[iL]);
                    buffer = BitConverter.GetBytes(w2);
                    Array.Copy(buffer, 0, res, iBuffer, buffer.Length);
                    //}
                    break;
                case SYSTEM_TYPE.TYPESTRING:
                    buffer = Encoding.Default.GetBytes(values[iL]);
                    Array.Copy(buffer, 0, res, iBuffer, Math.Min(buffer.Length, ByteLength));
                    break;
                case SYSTEM_TYPE.TYPEUNKNOWN:
                    throw new Exception("Invalid parameter type");
                default:
                    throw new Exception("Invalid parameter type");
                }
            }
            return res;
        }
    }

    public enum SYSTEM_TYPE
    {
        /*  -1*/
        TYPEUNKNOWN = -1,          /*!< -1: Tipo sconosciuto (usato sia per errori che come default) */
        /*  0*/
        TYPEINT,                    /*!< 0: Tipo int */
        /*  1*/
        TYPEUNSIGNEDINT,            /*!< 1: Tipo unsigned int */
        /*  2*/
        TYPECHAR,                   /*!< 2: Tipo char */
        /*  3*/
        TYPEUNSIGNEDCHAR,           /*!< 3: Tipo unsigned char */
        /*  4*/
        TYPEFLOAT,                  /*!< 4: Tipo float */
        /*  5*/
        TYPEDOUBLE,                 /*!< 5: Tipo double */
        /*  6*/
        TYPEBOOL,                   /*!< 6: Tipo bool */
        /*  7*/
        TYPELONG,                   /*!< 7: Tipo long */
        /*  8*/
        TYPEUNSIGNEDLONG,           /*!< 8: Tipo unsigned long */
        /*  9*/
        TYPESTRING,                 /*!< 9: Tipo stringa (array di char) */
        /*  10*/
        TYPEMEASURE,               /*!< 10: Tipo misura (array di short, relativo ai ai valori letti dall'ADC del sistema embedded */
        /*  11*/
        TYPESHORT,                 /*!< 11: Tipo short */
        /*  12*/
        TYPESHORTBUFFER,           /*!< 12: Tipo buffer di short, relativo alla stop on condition (tutti i valori grezzi di un mandrino)*/
        /*  13*/
        TYPEUNSIGNEDSHORTBUFFER,   /*!< 13: Tipo buffer di unsigned short, relativo al progetto colorimetro (valori dello spettro)*/
    }

    /**
     *  ID utilizzati all'interno del sistema
    **/

    public enum SYSTEM_ID
    {
        /* -1*/
        SYSTEM_ID_UNKNOWN = -1,                         /*!< -1: ID sconosciuto (usato sia per errori che come default) */
        /*200 */
        INFO_SERIAL_NUMBER = 200,                      /*!< 200: Info relativa al numero seriale */
        /*201 */
        INFO_FIRMWARE_VERSION,                         /*!< 201: Info relativa alla versione del firmware */
        /*202 */
        INFO_SDK_VERSION,                              /*!< 202: Info relativa alla versione dell'SDK */
        /*203 */
        INFO_IP_ADDRESS,                               /*!< 203: Info relativa all'indirizzo ip del sistema embedded */
        /*204 */
        INFO_PORT,                                     /*!< 204: Info relativa alla porta di comunicazione tra sistema embedded e PC */
        /*205 */
        INFO_FPGA_VERSION,                             /*!< 205: Info relativa alla versione dell'FPGA del sistema embedded */
        /*299 */
        INFO_OFFLINE,                                  /*!< 299: Info relativa alla modalità d'uso online / offline */
        /*300 */
        PAR_SPINDLE_COUNT = 300,                       /*!< 300: Parametro relativo al numero del mandrino della ruota della macchina */
        /*301 */
        PAR_WORKING_MODE,                              /*!< 301: Parametro relativo alla modalità di funzionamento */
        /*302 */
        PAR_CURRENT_INPUT_DWORD,                       /*!< 302: Parametro relativo alla "parola" in input corrente */
        /*303 */
        PAR_CURRENT_OUTPUT_DWORD,                      /*!< 303: Parametro relativo alla "parola" in output corrente */
        /*304 */
        PAR_REJECT_PATTERN_CODE_1,                     /*!< 304: Parametro relativo al primo codice di scarto riferito al pattern fisso di scarto */
        /*305 */
        PAR_REJECT_COUNT_CODE_1,                       /*!< 305: Parametro relativo al primo numero di campioni da scartare con codice di scarto precedente */
        /*306 */
        PAR_REJECT_PATTERN_CODE_2,                     /*!< 306: Parametro relativo al secondo codice di scarto riferito al pattern fisso di scarto */
        /*307 */
        PAR_REJECT_COUNT_CODE_2,                       /*!< 307: Parametro relativo al secondo numero di campioni da scartare con codice di scarto precedente */
        /*308 */
        PAR_STOP_ON_CONDITION_MODE,                    /*!< 308: Parametro relativo alla modalità in cui avviene la stop-on condition (good, any, #reject) */
        /*309 */
        PAR_STOP_ON_CONDITION_SPINDLE,                 /*!< 309: Parametro relativo al numero del mandrino in cui avviene la stop-on condition (none, any, #spindle_id) */
        /*310 */
        PAR_CALIBRATION_TABLE,                         /*!< 310: Parametro NON UTILIZZATO relativo alla calibration table */
        /*311 */
        PAR_CHANNEL_2_ACTIVE,                          /*!< 311: Parametro embedded relativo al canale 2 (on/off) */
        /*312 */
        PAR_CHANNEL_1_ACTIVE,                          /*!< 312: Parametro embedded relativo al canale 1 (on/off) */
        /*313 */
        PAR_SENSOR_2_STATUS,                           /*!< 313: Parametro embedded relativo al sensore 2 (on/off) */
        /*314 */
        PAR_LASER_2_STATUS,                            /*!< 314: Parametro embedded relativo al laser 2 (on/off) */
        /*315 */
        PAR_SENSOR_1_STATUS,                           /*!< 315: Parametro embedded relativo al sensore 1 (on/off) */
        /*316 */
        PAR_LASER_1_STATUS,                            /*!< 316: Parametro embedded relativo al laser 1 (on/off) */
        /*317 */
        PAR_LED_RED_POWER,                             /*!< 317: Parametro embedded relativo al led rosso power (on/off) */
        /*318 */
        PAR_LED_GREEN_POWER,                           /*!< 318: Parametro embedded relativo al led verde power (on/off) */
        /*319 */
        PAR_LED_RED_CHANNEL_2,                         /*!< 319: Parametro embedded relativo al led rosso canale 2 (on/off) */
        /*320 */
        PAR_LED_GREEN_CHANNEL_2,                       /*!< 320: Parametro embedded relativo al led verde canbale 2 (on/off) */
        /*321 */
        PAR_LED_RED_CHANNEL_1,                         /*!< 321: Parametro embedded relativo al led rosso canale 1 (on/off) */
        /*322 */
        PAR_LED_GREEN_CHANNEL_1,                       /*!< 322: Parametro embedded relativo al led verde canale 1 (on/off) */
        /*323 */
        PAR_ENABLE_LASER_2,                            /*!< 323: Parametro embedded relativo all'abilitazione del laser 2 (on/off) */
        /*324 */
        PAR_ENABLE_LASER_1,                            /*!< 324: Parametro embedded relativo all'abilitazione del laser 1 (on/off) */
        /*325 */
        PAR_GAIN_CHANNEL_2_HIGH,                       /*!< 325: Parametro embedded relativo all'abilitazione del guadagno alto del canale 2 (on/off) */
        /*326 */
        PAR_GAIN_CHANNEL_2_LOW,                        /*!< 326: Parametro embedded relativo all'abilitazione del guadagno basso del canale 2 (on/off) */
        /*327 */
        PAR_GAIN_CHANNEL_1_HIGH,                       /*!< 327: Parametro embedded relativo all'abilitazione del guadagno alto del canale 1 (on/off) */
        /*328 */
        PAR_GAIN_CHANNEL_1_LOW,                        /*!< 328: Parametro embedded relativo all'abilitazione del guadagno basso del canale 1 (on/off) */
        /*331 */
        PAR_LASER_EXPOTIME = 331,                      /*!< 331: Parametro relativo al tempo (ms) d'esposizione minimo del laser davanti al sensore (asenza di fialetta)*/
        /*332 */
        PAR_VIAL_EXPOTIME,                             /*!< 332: Parametro relativo al tempo (ms) d'esposizione minimo della fialetta davanti al sensore */
        /*333 */
        PAR_MEASURE_50_VALUES,                         /*!< 333: Parametro relativo ai 50 valori letti dall'ADC (misura di un periodo)*/
        /*350 */
        PAR_VIAL_GAIN = 350,                           /*!< 350: Parametro relativo al guadagno utilizzato per acquisire i dati in presenza di boccetta*/
        /*360 */
        PAR_LASER_GAIN = 360,                          /*!< 360: Parametro relativo al guadagno utilizzato per acquisire i dati in assenza di boccetta*/
        /*370 */
        PAR_0_TORRETTA_OFFSET = 370,					/*!< 370: Parametro relativo al valore del mandrino subito dopo lo 0 torretta*/
        /*371 */
        PAR_INCREMENTO_SPINDLE_ID,						/*!< 371: Parametro relativo all'incremento nel conteggio dei mandrini (1 o 2 unità)*/
        /*372 */
        PAR_VIAL_SPINDLE_NUMBER,                       /*!< 372: Parametro che associa una posizione (relativa al mandrino solo se la ruota è piena) ad una boccetta specifica*/
        /*373 */
        PAR_FREE_RUN_TIMER,                            /*!< 373: Parametro che definisce il tempo in cui rimanere in free run prima di ritornare in Idle*/
        /*374 */
        PAR_RECORDING_MODE,                            /*!< 374: Parametro che definisce la modalità di registrazione*/
        /*375 */
        PAR_RECORDING_TO_SAVE,                         /*!< 375: Parametro che definisce quanti contenitore registrare*/
        /*376 */
        PAR_RECORDING_SAVED,                           /*!< 376: Parametro che definisce quanti contenitori sono stati registrati*/
        /*377 */
        PAR_ACQUISITION_MODE,                          /*!< 377: Parametro che definisce la modalità di acquisizione (dal campo o da file)*/
        /*400 */
        ST_UNAVAILABLE = 400,                          /*!< 400: Parametro relativo allo status del dispositivo: non disponibile */
        /*401 */
        ST_AVAILABLE,                                  /*!< 401: Parametro relativo allo status del dispositivo: disponibile */
        /*402 */
        ST_INITIALIZING,                               /*!< 402: Parametro relativo allo status del dispositivo: initializing */
        /*403 */
        ST_READY,                                      /*!< 403: Parametro relativo allo status del dispositivo: ready */
        /*404 */
        ST_ERROR,                                      /*!< 404: Parametro relativo allo status del dispositivo: errore */
        /*500 */
        WM_NONE = 500,                                 /*!< 500: Parametro relativo alla modalià di lavoro: nessuna */
        /*501 */
        WM_CONTROL,                                    /*!< 501: Parametro relativo alla modalià di lavoro: control (ex WM_EXTERNAL_SOURCE) */
        /*502 */
        WM_FREE_RUN,                                   /*!< 502: Parametro relativo alla modalià di lavoro: free run (ex WM_TIMED)*/
        /*503 */
        WM_MECHANICAL_TEST,                            /*!< 503: Parametro relativo alla modalià di lavoro: test meccanico */
        /*600 */
        PM_NORMAL = 600,                               /*!< 600: Parametro relativo alla modalià di processing: normale */
        /*601 */
        PM_GOING_TO_STOP_ON_CONDITION,                 /*!< 601: Parametro relativo alla modalià di processing: going to stop on condition */
        /*602 */
        PM_STOP_ON_CONDITION,                          /*!< 602: Parametro relativo alla modalià di processing: stop on condition */
        /*701 */
        IN_TRIGGER = 700,                              /*!< 700: Parametro relativo ai GPINput: trigger */
        /*702 */
        IN_ZERO_TORRETTA,                              /*!< 701: Parametro relativo ai GPINput: zero torretta */
        /*703 */
        IN_VIAL_ID_0,                                  /*!< 702: Parametro relativo ai GPINput: vial id 0 */
        /*704 */
        IN_VIAL_ID_1,                                  /*!< 703: Parametro relativo ai GPINput: vial id 1 */
        /*800 */
        OUT_READY = 800,                               /*!< 800: Parametro relativo ai GPOUTput: ready */
        /*801 */
        OUT_ACQ_READY,                                 /*!< 801: Parametro relativo ai GPOUTput: acquisition ready */
        /*802 */
        OUT_VIAL_ID_0,                                 /*!< 802: Parametro relativo ai GPOUTput: vial id 0 */
        /*803 */
        OUT_VIAL_ID_1,                                 /*!< 803: Parametro relativo ai GPOUTput: vial id 1 */
        /*804 */
        OUT_DATA_VALID,                                /*!< 804: Parametro relativo ai GPOUTput: data valid */
        /*805 */
        OUT_REJECT_1,                                  /*!< 805: Parametro relativo ai GPOUTput: reject 1 (primo bit per la composizione delle combinazioni delle cause di scarto)*/
        /*806 */
        OUT_REJECT_2,                                  /*!< 806: Parametro relativo ai GPOUTput: reject 2 (secondo bit per la composizione delle combinazioni delle cause di scarto)*/
        /*807 */
        OUT_REJECT_3,                                  /*!< 807: Parametro relativo ai GPOUTput: reject 3 (terzo bit per la composizione delle combinazioni delle cause di scarto)*/
        /*900 */
        CAL_SPLINE_REGION_USED_COUNT = 900,            /*!< 900: Parametro relativo alla calibration table: regione di lavoro (da scegliere tra 5 dipsonibili, da 0 a 4) */
        /*901 */
        CAL_A0_REGION0,                                /*!< 901: Parametro relativo alla calibration table: coefficiente A della regione 0 */
        /*902 */
        CAL_B0_REGION0,                                /*!< 902: Parametro relativo alla calibration table: coefficiente B della regione 0 */
        /*903 */
        CAL_C0_REGION0,                                /*!< 903: Parametro relativo alla calibration table: coefficiente C della regione 0 */
        /*904 */
        CAL_D0_REGION0,                                /*!< 904: Parametro relativo alla calibration table: coefficiente D della regione 0 */
        /*905 */
        CAL_X0_LOWER,                                  /*!< 905: Parametro relativo alla calibration table: limite inferiore X della regione 0 */
        /*906 */
        CAL_X0_UPPER,                                  /*!< 906: Parametro relativo alla calibration table: limite superiore X della regione 0 */
        /*907 */
        CAL_A1_REGION1,                                /*!< 907: Parametro relativo alla calibration table: coefficiente A della regione 1 */
        /*908 */
        CAL_B1_REGION1,                                /*!< 908: Parametro relativo alla calibration table: coefficiente B della regione 1 */
        /*909 */
        CAL_C1_REGION1,                                /*!< 909: Parametro relativo alla calibration table: coefficiente C della regione 1 */
        /*910 */
        CAL_D1_REGION1,                                /*!< 910: Parametro relativo alla calibration table: coefficiente D della regione 1 */
        /*911 */
        CAL_X1_LOWER,                                  /*!< 911: Parametro relativo alla calibration table: limite inferiore X della regione 1 */
        /*912 */
        CAL_X1_UPPER,                                  /*!< 912: Parametro relativo alla calibration table: limite superiore X della regione 1 */
        /*913 */
        CAL_A2_REGION2,                                /*!< 913: Parametro relativo alla calibration table: coefficiente A della regione 2 */
        /*914 */
        CAL_B2_REGION2,                                /*!< 914: Parametro relativo alla calibration table: coefficiente B della regione 2 */
        /*915 */
        CAL_C2_REGION2,                                /*!< 915: Parametro relativo alla calibration table: coefficiente C della regione 2 */
        /*916 */
        CAL_D2_REGION2,                                /*!< 916: Parametro relativo alla calibration table: coefficiente D della regione 2 */
        /*917 */
        CAL_X2_LOWER,                                  /*!< 917: Parametro relativo alla calibration table: limite inferiore X della regione 2 */
        /*918 */
        CAL_X2_UPPER,                                  /*!< 918: Parametro relativo alla calibration table: limite superiore X della regione 2 */
        /*919 */
        CAL_A3_REGION3,                                /*!< 919: Parametro relativo alla calibration table: coefficiente A della regione 3 */
        /*920 */
        CAL_B3_REGION3,                                /*!< 920: Parametro relativo alla calibration table: coefficiente B della regione 3 */
        /*921 */
        CAL_C3_REGION3,                                /*!< 921: Parametro relativo alla calibration table: coefficiente C della regione 3 */
        /*922 */
        CAL_D3_REGION3,                                /*!< 922: Parametro relativo alla calibration table: coefficiente D della regione 3 */
        /*923 */
        CAL_X3_LOWER,                                  /*!< 923: Parametro relativo alla calibration table: limite inferiore X della regione 3 */
        /*924 */
        CAL_X3_UPPER,                                  /*!< 924: Parametro relativo alla calibration table: limite superiore X della regione 3 */
        /*925 */
        CAL_A4_REGION4,                                /*!< 925: Parametro relativo alla calibration table: coefficiente A della regione 4 */
        /*926 */
        CAL_B4_REGION4,                                /*!< 926: Parametro relativo alla calibration table: coefficiente B della regione 4 */
        /*927 */
        CAL_C4_REGION4,                                /*!< 927: Parametro relativo alla calibration table: coefficiente C della regione 4 */
        /*928 */
        CAL_D4_REGION4,                                /*!< 928: Parametro relativo alla calibration table: coefficiente D della regione 4 */
        /*929 */
        CAL_X4_LOWER,                                  /*!< 929: Parametro relativo alla calibration table: limite inferiore X della regione 4 */
        /*930 */
        CAL_X4_UPPER,                                  /*!< 930: Parametro relativo alla calibration table: limite superiore X della regione 4 */
        /*935 */
        TH_WARNING_UPPER = 935,                        /*!< 935: Parametro relativo all'analisi dati per verificare la causa di scarto: soglia di warning superiore */
        /*936 */
        TH_WARNING_LOWER,                              /*!< 936: Parametro relativo all'analisi dati per verificare la causa di scarto: soglia di warning inferiore */
        /*937 */
        TH_ERROR_UPPER,                                /*!< 937: Parametro relativo all'analisi dati per verificare la causa di scarto: soglia d'errore superiore */
        /*938 */
        TH_ERROR_LOWER,                                /*!< 938: Parametro relativo all'analisi dati per verificare la causa di scarto: soglia d'errore inferiore */
        /*940 */
        LOCKIN_SAMPLES_PER_PERIOD = 940,               /*!< 940: Parametro relativo alla fase di elaborazione ed acquisizione: campioni per periodo */
        /*941 */
        LOCKIN_VIAL_PERIODS,                           /*!< 941: Parametro relativo alla fase di elaborazione ed acquisizione: periodi con boccetta acquisiti */
        /*942 */
        LOCKIN_LASER_PERIODS,                          /*!< 942: Parametro relativo alla fase di elaborazione ed acquisizione: periodi senza boccetta acquisiti */
        /*950 */
        LOCKIN_VIAL_AMPLITUDE_RAW = 950,               /*!< 950: Parametro relativo alla fase di elaborazione della funzione lockin: ampiezza grezza boccetta */
        /*960 */
        LOCKIN_LASER_AMPLITUDE_RAW = 960,              /*!< 960: Parametro relativo alla fase di elaborazione della funzione lockin: ampiezza grezza senza boccetta */
        /*970 */
        LOCKIN_AMPLITUDE_FACTOR_COMPENSATED = 970,     /*!< 970: Parametro relativo alla fase di elaborazione della funzione lockin: ampiezza compensata */
        /*980 */
        LOCKIN_AMPLITUDE_POST_CALIBRATION = 980,       /*!< 980: Parametro relativo alla fase di elaborazione della funzione lockin: ampiezza post calibrazione */
        /*981 */
        STOP_ON_CONDITION_FULL_VALUE_BUFFER,           /*!< 981: Parametro relativo alla stop on condition: buffer completo di valori grezzi (con e senza boccetta) */
        /*982 */
        STOP_ON_CONDITION_REAL_VALUE_BUFFER,           /*!< 982: Parametro relativo alla stop on condition: buffer completo di valori grezzi (con e senza boccetta) con contollo sul tempo d'acquisizione */
        /*983 */
        ANALYSIS_VIAL_ID_COUNTER,                      /*!< 983: Parametro relativo all'analisi: incrementa ogni qual volta si verifica un fronte di salita del trigger (tiene conto di tutte le boccette analizzate)*/
        /*984 */
        ANALYSIS_IS_REJECT,                            /*!< 984: Parametro relativo all'analisi: true se valore ampiezza è ok, false per qualsiasi causa di scarto*/
        /*985 */
        ANALYSIS_REJECTION_CAUSE_NUMBER,               /*!< 985: Parametro relativo all'analisi: valore da 0 a 7 relativo alla causa di scarto*/
        /*990 */
        DIAGNOSTIC_FULL_REGISTER_SL_STATUS = 990,      /*!< 990: Parametro relativo alla diagnostica: intero registro relativo al canale e alla coppia laser-sensore attive */
        /*991 */
        DIAGNOSTIC_FULL_REGISTER_LED_STATUS,           /*!< 991: Parametro relativo alla diagnostica: intero registro dei led */
        /*992 */
        DIAGNOSTIC_FULL_REGISTER_GPOUTPUT,             /*!< 992: Parametro relativo alla diagnostica: intero registro degli output */
        /*993 */
        DIAGNOSTIC_FULL_REGISTER_GPINPUT,              /*!< 993: Parametro relativo alla diagnostica: intero registro degli input */
        /*994 */
        DIAGNOSTIC_FULL_REGISTER_LASER_CTRL,           /*!< 994: Parametro relativo alla diagnostica: intero registro relativo allo stato dei laser (on-off) */
        /*995 */
        DIAGNOSTIC_FULL_REGISTER_SENS_CTRL,            /*!< 995: Parametro relativo alla diagnostica: intero registro dei guadagni */
        /*999 */
        ANALYSIS_RESPONSE = 999,                       /*!< 999: Parametro relativo alle modalità: comunica se l'analisi è andata a buon fine o se ci sono degli errori*/
        /*1000 */
        PAR_SM_STATUS_ACQ = 1000,                     /*!< 1000: Parametro relativo al registro 0x01 dell'FPGA per il progetto Colorimetro: comunica all'FPGA di essere pronto ad acquisire lo spetto*/
        /*1001 */
        PAR_SM_STATUS_READY,                          /*!< 1001: Parametro relativo al registro 0x01 dell'FPGA per il progetto Colorimetro: segnale dell'FPGA per comunicare che i dati sono pronti*/
        /*1002 */
        IN_PRESENCE,                                  /*!< 1002: Parametro relativo ai GPINput: presence*/
        /*1003 */
        INFO_LOOK_UP_TABLE,                           /*!< 1003: Parametro relativo al progetto Colorimetro: look-up table che associa pixel dello spettrometro a lunghezze d'onda*/
        /*1004 */
        PAR_TINT_VIAL,                                /*!< 1004: Parametro relativo al progetto Colorimetro: tempo d'integrazione, in ms, in presenza di boccetta*/
        /*1005 */
        PAR_TINT_BACKGROUND,                          /*!< 1005: Parametro relativo al progetto Colorimetro: tempo d'integrazione, in ms, in assenza di boccetta*/
        /*1006 */
        PAR_FULL_SPECTRUM_BUFFER,                     /*!< 1006: Parametro relativo al progetto Colorimetro: buffer con i valori dello spettro misurato ("raw" values)*/
        /*1007 */
        PAR_REFERENCE_SPECTRUM_VALUES,                /*!< 1007: Parametro relativo al progetto Colorimetro: buffer con i valori dello spettro di riferimento*/
        /*1008 */
        PAR_BACKGROUND_SPECTRUM_VALUES,               /*!< 1008: Parametro relativo al progetto Colorimetro: buffer con i valori dello spettro di background*/
        /*1009 */
        PAR_CALIBRATION_REFERENCE,                    /*!< 1009: Parametro relativo al progetto Colorimetro: nome del file in cui ci sono i dati per il riferimento*/
        /*1010 */
        PAR_CALIBRATION_BACKGROUND,                   /*!< 1010: Parametro relativo al progetto Colorimetro: nome del file in cui ci sono i dati per il background*/
        /*1011 */
        PAR_REFERENCE_ACTUAL_CALIBRATION,             /*!< 1011: Parametro relativo al progetto Colorimetro: nome del file di riferimento attualmente usato*/
        /*1012 */
        PAR_BACKGROUND_ACTUAL_CALIBRATION,            /*!< 1012: Parametro relativo al progetto Colorimetro: nome del file di background attualmente usato*/

        //aggiunti dopo documento specifiche 24/08/2015

        /*1013 */
        PAR_RANGE_1_L,                                /*!< 1013: Parametro relativo al progetto Colorimetro: lunghezza d'onda inizio del range 1*/
        /*1014 */
        PAR_RANGE_1_H,                                /*!< 1014: Parametro relativo al progetto Colorimetro: lunghezza d'onda fine range 1*/
        /*1015 */
        PAR_RANGE_1_SIGN,                             /*!< 1015: Parametro relativo al progetto Colorimetro: segno con cui considerare la differenza del range 1*/
        /*1016 */
        PAR_RANGE_1_TH,                               /*!< 1016: Parametro relativo al progetto Colorimetro: soglia entro cui ignorare la somma del range 1*/
        /*1017 */
        PAR_RANGE_1_PEAK_REJECT_TH,                   /*!< 1017: Parametro relativo al progetto Colorimetro: soglia di scarto per picco del range 1*/
        /*1018 */
        PAR_RANGE_2_L,                                /*!< 1018: Parametro relativo al progetto Colorimetro: lunghezza d'onda inizio del range 2*/
        /*1019 */
        PAR_RANGE_2_H,                                /*!< 1019: Parametro relativo al progetto Colorimetro: lunghezza d'onda fine del range 2*/
        /*1020 */
        PAR_RANGE_2_SIGN,                             /*!< 1020: Parametro relativo al progetto Colorimetro: segno con cui co+nsiderare la differenza del range 2*/
        /*1021 */
        PAR_RANGE_2_TH,                               /*!< 1021: Parametro relativo al progetto Colorimetro: soglia entro cui ignorare la somma del range 2*/
        /*1022 */
        PAR_RANGE_2_PEAK_REJECT_TH,                   /*!< 1022: Parametro relativo al progetto Colorimetro: soglia di scarto per picco del range 2*/
        /*1023 */
        PAR_RANGE_3_L,                                /*!< 1023: Parametro relativo al progetto Colorimetro: lunghezza d'onda inizio del range 3*/
        /*1024 */
        PAR_RANGE_3_H,                                /*!< 1024: Parametro relativo al progetto Colorimetro: lunghezza d'onda fine del range 3*/
        /*1025 */
        PAR_RANGE_3_SIGN,                             /*!< 1025: Parametro relativo al progetto Colorimetro: segno con cui considerare la differenza del range 3*/
        /*1026 */
        PAR_RANGE_3_TH,                               /*!< 1026: Parametro relativo al progetto Colorimetro: soglia entro cui ignorare la somma del range 3*/
        /*1027 */
        PAR_RANGE_3_PEAK_REJECT_TH,                   /*!< 1027: Parametro relativo al progetto Colorimetro: soglia di scarto per picco del range 3*/
        /*1028 */
        PAR_RANGE_4_L,                                /*!< 1028: Parametro relativo al progetto Colorimetro: lunghezza d'onda inizio del range 4*/
        /*1029 */
        PAR_RANGE_4_H,                                /*!< 1029: Parametro relativo al progetto Colorimetro: lunghezza d'onda fine del range 4*/
        /*1030 */
        PAR_RANGE_4_SIGN,                             /*!< 1030: Parametro relativo al progetto Colorimetro: segno con cui considerare la differenza del range 4*/
        /*1031 */
        PAR_RANGE_4_TH,                               /*!< 1031: Parametro relativo al progetto Colorimetro: soglia entro cui ignorare la somma del range 4*/
        /*1032 */
        PAR_RANGE_4_PEAK_REJECT_TH,                   /*!< 1032: Parametro relativo al progetto Colorimetro: soglia di scarto per picco del range 4*/
        /*1033 */
        PAR_ALL_RANGE_SUM_REJECT,                     /*!< 1033: Parametro relativo al progetto Colorimetro: soglia di scarto per la somma di tutti i valori trovati nei 4 range*/

        /*1034 */
        PAR_FULL_SPECTRUM_MIS,                        /*!< 1034: Parametro relativo al progetto Colorimetro: valori spettro misurato normalizzati*/
        /*1035 */
        PAR_FULL_SPECTRUM_ELAB,                       /*!< 1035: Parametro relativo al progetto Colorimetro: valori spettro ELAB (differenza misurato-reference)*/
        /*1036 */
        PAR_FULL_SPECTRUM_REF_NORM,                   /*!< 1036: Parametro relativo al progetto Colorimetro: valori spettro reference normalizzati*/
        /*1037 */
        PAR_FULL_SPECTRUM_BACK_NORM,                  /*!< 1037: Parametro relativo al progetto Colorimetro: valori spettro background normalizzati*/
        /*1038 */
        PAR_REF_SPECTRUM_FROM_CLIENT,                 /*!< 1038: Parametro relativo al progetto Colorimetro: ID speciale che permette al client di inviare al server lo spettro di riferimento*/
        /*1039 */
        PAR_REF_SPECTRUM_FROM_CLIENT_FILENAME,        /*!< 1039: Parametro relativo al progetto Colorimetro: relativo ad ID speciale 1038, nome del file da salvare */
        /*1040 */
        PAR_REF_SPECTRUM_FROM_CLIENT_OVERWRITE,       /*!< 1039: Parametro relativo al progetto Colorimetro: relativo ad ID speciale 1038, bool che segnala se sovrascrivere o meno anche il buffer background */

        /*1041 */
        PAR_RANGE_1_MEAN,                             /*!< 1041: Parametro relativo al progetto Colorimetro: valori media spettro ELAB per PAR_RANGE_1_SIGN range 1*/
        /*1042 */
        PAR_RANGE_2_MEAN,                             /*!< 1042: Parametro relativo al progetto Colorimetro: valori media spettro ELAB per PAR_RANGE_2_SIGN range 2*/
        /*1043 */
        PAR_RANGE_3_MEAN,                             /*!< 1043: Parametro relativo al progetto Colorimetro: valori media spettro ELAB per PAR_RANGE_3_SIGN range 3*/
        /*1044 */
        PAR_RANGE_4_MEAN,                             /*!< 1044: Parametro relativo al progetto Colorimetro: valori media spettro ELAB per PAR_RANGE_4_SIGN range 4*/
        /*1045 */
        PAR_ALL_RANGE_MEAN,							/*!< 1045: Parametro relativo al progetto Colorimetro: somma delle medie di ciascun range*/

        /*1050 */
        LIGHT_CONTROL = 1050,                         /*!< 1050: Parametro relativo al progetto Colorimetro: valore misurato per il controllo luce*/
        /*1051 */
        LIGHT_CONTROL_TH,                             /*!< 1051: Parametro relativo al progetto Colorimetro: soglia per il controllo luce*/

        /*1060 */
        INITIAL_WAVELENGTH = 1060,                    /*!< 1060: Parametro relativo al progetto Colorimetro: lunghezza d'onda iniziale*/
        /*1061 */
        WAVELENGTH_INCREMENT,                         /*!< 1061: Parametro relativo al progetto Colorimetro: incremento della lunghezza d'onda*/
        /*1062 */
        FINAL_WAVELENGTH,                             /*!< 1062: Parametro relativo al progetto Colorimetro: lunghezza d'onda finale*/

        /*9999 */
        BROADCAST_MESSAGE = 9999,                     /*!< 9999: Parametro relativo alla fase di ricerca dispositivi via broadcast */
        /*0x7FFFFFFF */
        FORCE_SYSTEM_ID_MAX_INT = 0x7FFFFFFF,   /*!< 0x7FFFFFFF: Parametro inutilizzato per forzare gli altri ID ad un valore di tipo int */
    }
}
