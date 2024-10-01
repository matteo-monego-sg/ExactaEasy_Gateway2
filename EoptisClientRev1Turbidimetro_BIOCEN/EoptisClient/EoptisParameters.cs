using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EoptisClient {

    public class EoptisParameters {

        public static EoptisParameters LoadFromFile(string filePath) {

            EoptisParameters eoptisParams = null;
            using (StreamReader reader = new StreamReader(filePath)) {
                string xmlString = reader.ReadToEnd();
                eoptisParams = LoadFromXml(xmlString);
            }
            return eoptisParams;
        }

        public static EoptisParameters LoadFromXml(string xmlString) {

            EoptisParameters eoptisParams = null;
            using (StringReader reader = new StringReader(xmlString)) {

                XmlSerializer xmlSer = new XmlSerializer(typeof(EoptisParameters));
                eoptisParams = (EoptisParameters)xmlSer.Deserialize(reader);
            }
            return eoptisParams;
        }

        public EoptisParameterCollection Parameters { get; set; }

        public EoptisParameters() {

            Parameters = new EoptisParameterCollection();
        }
    }

    public class EoptisParameterCollection : List<EoptisParameter> {

        public EoptisParameter this[SYSTEM_ID id] {
            get {
                return this.Find(ep => ep.Id == id);
            }
        }
    }

    public class EoptisParameter {

        public SYSTEM_ID Id { get; set; }
        public SYSTEM_TYPE Type { get; set; }
        public int Size { get; set; }
        [XmlIgnore]
        public string Value { get; set; }

        public EoptisParameter() {
        }

        public EoptisParameter(SYSTEM_ID id) {
            Id = id;
        }
    }

    public enum SYSTEM_TYPE {
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
    }

    /**
     *  ID utilizzati all'interno del sistema
    **/

    public enum SYSTEM_ID {
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
        /*9999 */
        BROADCAST_MESSAGE = 9999,                     /*!< 9999: Parametro relativo alla fase di ricerca dispositivi via broadcast */
        /*0x7FFFFFFF */
        FORCE_SYSTEM_ID_MAX_INT = 0x7FFFFFFF,   /*!< 0x7FFFFFFF: Parametro inutilizzato per forzare gli altri ID ad un valore di tipo int */
    }
}
