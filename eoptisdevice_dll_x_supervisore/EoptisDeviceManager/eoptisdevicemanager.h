#ifndef EOPTISDEVICEMANAGER_H
#define EOPTISDEVICEMANAGER_H

#include <stdio.h>
#include <DataChunk.h>

#include <QtCore/qglobal.h>
#include "eoptisdevicemanager_global.h"

#define MAX_DEVICES 16

namespace eoptis_device_manager
{

// ===================== //
// FUNZIONI PER STABILIRE LA COMUNICAZIONE
// ===================== //
// ! Ottenere dispositivi disponibili in rete
/*!
  Permette di ottenere, tramite messaggio broadcast, gli ip dei dipositivi attualmente connessi alla rete (non funzionante)
  \param ip Indirizzo ip a cui fare il broadcast per ottenere lista dei dispositivi presenti
  \return Numero (o indirizzi) dei dispositivi presenti nella rete
*/

//int EOPTISDEVICEMANAGERSHARED_EXPORT getDevices(const char *ip);

// ! Connettere dispositivo alla rete
/*!
  Permette di connettere un dispositivio, discriminato dall'ip, ad una porta di comunicazione; permette inoltre di attivare o meno la chiamata alla funzione Callback
  \param ip Indirizzo ip del dispositivo a cui connettersi
  \param port Porta sulla quale stabilire la connessione
  \param activeCallback Impostare se è necessario avere dati tramite Callback o no
  \return Eventuali errori
*/

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV connectDevice(const char* ip, unsigned int port, bool activeCallback);

// ! Disconnettere dispositivo dalla rete
/*!
  Permette di disconnettere un dispositivio, discriminato dall'ip, dalla rete
  \param ip Indirizzo ip da cui ci si vuole disconnettere
  \return Eventuali errori
*/

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV disconnect(const char* ip);

// ===================== //
// FUNZIONI OPERATIVE
// ===================== //
// ! Ottenere singola informazione
/*!
  Permette di ottenere una singola informazione relativa al sistema, prendendo l'ID prensente nel VariantDatum passato alla funzione
  \param info VariantDatum con ID relativo all'informazione desiderata ed in cui scrivere il valore letto
  \return Eventuali errori
*/

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getInfo(const char *ip, VariantDatumStruct* info);

// ! Ottenere tutte le informazione
/*!
  Permette di ottenere tutte le informazioni relativa al sistema
  \param dataChunk DataChunk con ID relativo a tutte le informazione desiderate ed in cui scrivere i valori letti
  \return Eventuali errori
*/

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getAllInfo(const char *ip, DataChunkStruct* dataChunk);

// ! Ottenere singolo parametro
/*!
  Permette di ottenere un singolo parametro, prendendo l'ID prensente nel VariantDatum e mettendo il valore letto all'interno di tale oggetto
  \param parameter VariantDatum con ID relativo al parametro desiderato ed in cui scrivere il valore letto
  \return Eventuali errori
*/

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getParameter(const char *ip, VariantDatumStruct* parameter);

// ! Ottenere type e size parametro
/*!
  Permette di ottenere il type ed il size di un parametro avente id pari a paramId
  \param parameter paramId: ID relativo al parametro desiderato e di cui riportare le informazioni
  \return Eventuali errori
*/

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getParameterType(int paramId, int* paramType, int* paramSize);

// ! Settare singolo parametro
/*!
  Permette di settare un singolo parametro, prendendo l'ID prensente nel VariantDatum passato alla funzione ed il dato in esso contenuto
  \param parameter VariantDatum con ID relativo al parametro desiderato e da cui estrarre il valore da scrivere
  \return Eventuali errori
*/

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV setParameter(const char *ip, const VariantDatumStruct* parameter);

// ! Ottenere più parametri
/*!
  Permette di ottenere più parametri in una sola chiamata.\n
  Il valore di totData del DataChunk comunica quanti parametri si vogliono ottenere, mentre gli ID dei VariantDatum presenti nell'oggetto DataChunk discriminano i parametri voluti.\n
  I valori letti verranno poi inseriti nei relativi VariantDatum
  \param dataChunk DataChunk con ID relativi ai parametri desiderati ed in cui scrivere i valori letti
  \return Eventuali errori
*/

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getParameters(const char *ip, DataChunkStruct* dataChunk);

// ! Settare più parametri
/*!
  Permette di settare più parametri in una sola chiamata.\n
  Il valore di totData del DataChunk comunica quanti parametri si vogliono settare, mentre gli ID dei VariantDatum presenti nell'oggetto DataChunk discriminano i parametri voluti.\n
  I dati contenuti nei VariantDatum verranno poi impostati
  \param dataChunk DataChunk con ID relativi ai parametri desiderati e da cui estrarre i valori da impostare
  \return Eventuali errori
*/

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV setParameters(const char *ip, const DataChunkStruct* dataChunk);

// ! Ottenere status
/*!
  Interroga il sistema riguardo il suo stato di funzionamento
  \param status DataChunk in cui scrivere il valore dello status del sistema
  \param continuousUpdate Valore che, se settato true, permette di ricevere un aggiornamento ogni qual volta cambi lo status
  \return Eventuali errori
*/

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getStatus(const char *ip, DataChunkStruct* status, bool continuousUpdate);

// ! Ottenere input
/*!
  Permette di ottenere lo stato dei GPINput del sistema
  \param inputs DataChunk in cui scrivere i valori degli input
  \param continuousUpdate Valore che, se settato true, permette di ricevere un aggiornamento ogni qual volta cambino gli input
  \return Eventuali errori
*/

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getInputs(const char *ip, DataChunkStruct* inputs, bool continuousUpdate);

// ! Ottenere output
/*!
  Permette di ottenere lo stato dei GPOUTput del sistema
  \param outputs DataChunk in cui scrivere i valori degli output
  \param continuousUpdate Valore che, se settato true, permette di ricevere un aggiornamento ogni qual volta cambino gli output
  \return Eventuali errori
*/

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getOutputs(const char *ip, DataChunkStruct* outputs, bool continuousUpdate);

// ! Settare output
/*!
  Permette di settare, o meglio forzare, lo stato dei GPOUTput del sistema
  \param outputs DataChunk in cui sono presenti i valori, con relativi ID, da settare
  \return Eventuali errori
*/

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV setOutputs(const char *ip, const DataChunkStruct* outputs);

// ! Settare pattern di scarto
/*!
  Permette di impostare il due numeri di boccette da scartare, con il relativo codice di scarto
  \param patternArray DataChunk contenente i valori di codice di scarto e di numero di boccette da scartare (DataChunk con quattro elementi e relativi ID)
  \return Eventuali errori
*/

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV setOutputPattern(const char *ip, const DataChunkStruct* patternArray);

// ! Settare modalità di funzionamento
/*!
  Permette di settare la modalità di funzionamento del sistema (control, free run o mechanical test)
  \param mode Modalità di funzionamento (conversione in VariantDatum fatta all'interno della funzione in automatico)
  \return Eventuali errori
*/

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV setWorkingMode(const char *ip, int mode);    //da completare

// ! Ottenere modalità di funzionamento
/*!
  Permette di ottenre la modalità di funzionamento attualmente settata per il sistema
  \param mode Valore in cui verrà salvato il valore letto (conversione in VariantDatum fatta all'interno della funzione in automatico)
*/

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getWorkingMode(const char *ip, int *mode);   //da completare

// ! Impostare le condizioni per la stop on condition
/*!
  Permette di settare i valori, in termini di modalità e numero di mandrino, per i quali il sistema deve mettersi in modalità stop on (foto totale del sistema)
  \param stopCondition Condizione per la quale si deve verificare la stop on condition (good, reject, any)
  \param spindleID Numero del mandrino a cui deve verificarsi la condizione (none, any, #spindleCount)
  \return Eventuali errori
*/

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV stopOnCondition(const char *ip, int stopCondition, int spindleID);   //da completare

/* funzioni ancora da implementare

 ERROR_CODE getParameterList(const char* ip, DataChunk& codeList);
 ERROR_CODE softReset(const char* ip);
 ERROR_CODE hardReset(const char* ip);

*/

// ! Ricevere dati dal server
/*!
  Permette di ricevere i dati in arrivo dal server e di salvarli in un oggetto DataChunk (nella modalità free run)
  \param dataChunk DataChunk in cui vengono scritti i valori in arrivo dal server
*/

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT int CALL_CONV receiveData(const char *ip, DataChunkStruct* dataChunk);

extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT void CALL_CONV freeData(DataChunkStruct* dataChunk);

// DA QUI IN POI SONO TUTTE FUNZIONI DA BUTTARE

//extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT void receiveDataOld(const char *ip, DataChunk &dataChunk);


// FUNZIONI DUMMY PER TESTARE DLL SENZA DEVICE

//extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getParamDummy(const char *ip, VariantDatumStruct* parameter);

//extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV setParamDummy(const char *ip, const VariantDatumStruct* parameter);

//extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getParamsDummy(const char *ip, DataChunkStruct* dataChunk);

//extern "C" EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV setParamsDummy(const char *ip, const DataChunkStruct* dataChunk);
};


#endif // EOPTISDEVICEMANAGER_H
