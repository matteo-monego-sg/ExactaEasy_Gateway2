#ifndef OPACIMETRO_H
#define OPACIMETRO_H

// ===================================
// INCLUDE
// Vengono inclusi header esterni
// ===================================

#include <stdio.h>
#include <fstream>
#include "opacimetro_global.h"
#include <DataChunk.h>
#include <ClientControl.h>

/*! \file DataChunk.h
    \brief Header della libreria DataChunk

    Contiene le classi e le funzioni descritte in tale libreria per permettere il passaggio dei dati tramite DataChunk e VariantDatum

    \file ClientControl.h
    \brief Header relativo al lato Client della libreria Network

    Contiene la classi e le funzioni necessarie per il funzionamento del lato Client
*/

//typedef void (_stdcall * CALLBACK_FUNC)(char* ip, CommEvent evt, CommDataType dataType, const DataChunk* incData);

/*!
    \class Opacimetro
  \brief Interfaccia per l'utilizzo del sistema lato Client

  Si tratta della classe che descrive l'interfaccia del lato Client del programma.\n
  Mette a disposizione delle funzioni per settare/ottenere dei parametri per il corretto funzionamento del sistema
*/


class OPACIMETROSHARED_EXPORT Opacimetro
{

public:

    // ===================== //
    // COSTRUTTORE
    // ===================== //
    // ! Costruttore
    /*!
      Crea un oggetto Opacimetro con valori di default.
    */

    Opacimetro();

    // ===================== //
    // DISTRUTTORE
    // ===================== //
    // ! Distruttore
    /*!
      Elimina l'oggetto una volta esaurito il suo utilizzo.
    */

    ~Opacimetro();

    // ===================== //
    // FUNZIONI PER STABILIRE LA COMUNICAZIONE
    // ===================== //
    // ! Ottenere dispositivi disponibili in rete
    /*!
      Permette di ottenere, tramite messaggio broadcast, gli ip dei dipositivi attualmente connessi alla rete (non funzionante)
      \param ip Indirizzo ip a cui fare il broadcast per ottenere lista dei dispositivi presenti
      \return Numero (o indirizzi) dei dispositivi presenti nella rete
    */

    int GetDevices(const char *ip);

    // ! Connettere dispositivo alla rete
    /*!
      Permette di connettere un dispositivio, discriminato dall'ip, ad una porta di comunicazione; permette inoltre di attivare o meno la chiamata alla funzione Callback
      \param ip Indirizzo ip del dispositivo a cui connettersi
      \param port Porta sulla quale stabilire la connessione
      \param activeCallback Impostare se è necessario avere dati tramite Callback o no
      \return Eventuali errori
    */

    ERROR_CODE connect(const char* ip, unsigned int port, bool activeCallback);

    // ! Disconnettere dispositivo dalla rete
    /*!
      Permette di disconnettere un dispositivo, discriminato dall'ip, dalla rete
      \return Eventuali errori
    */

    ERROR_CODE disconnect();

    // ===================== //
    // FUNZIONI OPERATIVE
    // ===================== //
    // ! Ottenere singola informazione
    /*!
      Permette di ottenere una singola informazione relativa al sistema, prendendo l'ID prensente nel VariantDatum passato alla funzione
      \param info VariantDatum con ID relativo all'informazione desiderata ed in cui scrivere il valore letto
      \return Eventuali errori
    */

    ERROR_CODE getInfo(VariantDatum& info);

    // ! Ottenere tutte le informazione
    /*!
      Permette di ottenere tutte le informazioni relativa al sistema
      \param dataChunk DataChunk con ID relativo a tutte le informazione desiderate ed in cui scrivere i valori letti
      \return Eventuali errori
    */

    ERROR_CODE getAllInfo(DataChunk& dataChunk);

    // ! Ottenere singolo parametro
    /*!
      Permette di ottenere un singolo parametro, prendendo l'ID prensente nel VariantDatum e mettendo il valore letto all'interno di tale oggetto
      \param parameter VariantDatum con ID relativo al parametro desiderato ed in cui scrivere il valore letto
      \return Eventuali errori
    */

    ERROR_CODE getParameter(VariantDatum& parameter);

    // ! Settare singolo parametro
    /*!
      Permette di settare un singolo parametro, prendendo l'ID prensente nel VariantDatum passato alla funzione ed il dato in esso contenuto
      \param parameter VariantDatum con ID relativo al parametro desiderato e da cui estrarre il valore da scrivere
      \return Eventuali errori
    */

    ERROR_CODE setParameter(VariantDatum& parameter);

    // ! Ottenere più parametri
    /*!
      Permette di ottenere più parametri in una sola chiamata.\n
      Il valore di totData del DataChunk comunica quanti parametri si vogliono ottenere, mentre gli ID dei VariantDatum presenti nell'oggetto DataChunk discriminano i parametri voluti.\n
      I valori letti verranno poi inseriti nei relativi VariantDatum
      \param dataChunk DataChunk con ID relativi ai parametri desiderati ed in cui scrivere i valori letti
      \return Eventuali errori
    */

    ERROR_CODE getParameters(DataChunk& dataChunk);

    // ! Settare più parametri
    /*!
      Permette di settare più parametri in una sola chiamata.\n
      Il valore di totData del DataChunk comunica quanti parametri si vogliono settare, mentre gli ID dei VariantDatum presenti nell'oggetto DataChunk discriminano i parametri voluti.\n
      I dati contenuti nei VariantDatum verranno poi impostati
      \param dataChunk DataChunk con ID relativi ai parametri desiderati e da cui estrarre i valori da impostare
      \return Eventuali errori
    */

    ERROR_CODE setParameters(DataChunk& dataChunk);

    // ! Ottenere status
    /*!
      Interroga il sistema riguardo il suo stato di funzionamento
      \param status DataChunk in cui scrivere il valore dello status del sistema
      \param continuousUpdate Valore che, se settato true, permette di ricevere un aggiornamento ogni qual volta cambi lo status
      \return Eventuali errori
    */

    ERROR_CODE getStatus(DataChunk& status, bool continuousUpdate);

    // ! Ottenere input
    /*!
      Permette di ottenere lo stato dei GPINput del sistema
      \param inputs DataChunk in cui scrivere i valori degli input
      \param continuousUpdate Valore che, se settato true, permette di ricevere un aggiornamento ogni qual volta cambino gli input
      \return Eventuali errori
    */

    ERROR_CODE getInputs(DataChunk& inputs, bool continuousUpdate);

    // ! Ottenere output
    /*!
      Permette di ottenere lo stato dei GPOUTput del sistema
      \param outputs DataChunk in cui scrivere i valori degli output
      \param continuousUpdate Valore che, se settato true, permette di ricevere un aggiornamento ogni qual volta cambino gli output
      \return Eventuali errori
    */

    ERROR_CODE getOutputs(DataChunk &outputs, bool continuousUpdate);

    // ! Settare output
    /*!
      Permette di settare, o meglio forzare, lo stato dei GPOUTput del sistema
      \param outputs DataChunk in cui sono presenti i valori, con relativi ID, da settare
      \return Eventuali errori
    */

    ERROR_CODE setOutputs(DataChunk &outputs);

    // ! Settare pattern di scarto
    /*!
      Permette di impostare il due numeri di boccette da scartare, con il relativo codice di scarto
      \param patternArray DataChunk contenente i valori di codice di scarto e di numero di boccette da scartare (DataChunk con quattro elementi e relativi ID)
      \return Eventuali errori
    */

    ERROR_CODE setOutputPattern(DataChunk &patternArray);

    // ! Settare modalità di funzionamento
    /*!
      Permette di settare la modalità di funzionamento del sistema (control, free run o mechanical test)
      \param mode Modalità di funzionamento (conversione in VariantDatum fatta all'interno della funzione in automatico)
      \return Eventuali errori
    */

    ERROR_CODE setWorkingMode(int mode);    //da completare

    // ! Ottenere modalità di funzionamento
    /*!
      Permette di ottenre la modalità di funzionamento attualmente settata per il sistema
      \param mode Valore in cui verrà salvato il valore letto (conversione in VariantDatum fatta all'interno della funzione in automatico)
    */

    ERROR_CODE getWorkingMode(int *mode);   //da completare

    // ! Impostare le condizioni per la stop on condition
    /*!
      Permette di settare i valori, in termini di modalità e numero di mandrino, per i quali il sistema deve mettersi in modalità stop on (foto totale del sistema)
      \param stopCondition Condizione per la quale si deve verificare la stop on condition (good, reject, any)
      \param spindleID Numero del mandrino a cui deve verificarsi la condizione (none, any, #spindleCount)
      \return Eventuali errori
    */

    ERROR_CODE stopOnCondition(int stopCondition, int spindleID);   //da completare

    /* funzioni ancora da implementare

     ERROR_CODE getParameterList(const char* ip, DataChunk& codeList);
     ERROR_CODE softReset(const char* ip);
     ERROR_CODE hardReset(const char* ip);

    */

public: //funzioni per test su pc singolo

    // ! Ricevere dati dal server
    /*!
      Permette di ricevere i dati in arrivo dal server e di salvarli in un oggetto DataChunk (nella modalità free run)
      \param dataChunk DataChunk in cui vengono scritti i valori in arrivo dal server
    */

    int receiveData(DataChunk &dataChunk);

    // ! Ottenere l'indirizzo IP
    /*!
      Permette di ottenere l'indirizzo IP di un'istanza opacimetro
    */

    const char* getIP();

private:
    ClientControl communicationInterface_;
    ClientControl communicationInterfaceUdp_;
    char* receiveDataBuffer_;
	char* sendDataBuffer_;
    unsigned int dataSize_;
    char ip_[32];
    //DataChunk dataChunkDefaultValues_;

private:
    int dataChunkToBuffer(DataChunk &dataChunk, const bool isWrite);
    void bufferToDataChunk(DataChunk &dataChunk);
    void exchangeDataChunk(DataChunk &dataChunk, bool isWrite);
    void clear();
    void sendData(DataChunk &dataChunk, bool isWrite);

    ERROR_CODE checkIdInfo(SYSTEM_ID id);
    ERROR_CODE checkIdParameter(SYSTEM_ID id);
    ERROR_CODE checkIdStatus(SYSTEM_ID id);
    ERROR_CODE checkIdOutputs(SYSTEM_ID id);
    ERROR_CODE checkOutputPattern(DataChunk& patternArray);

};

#endif // OPACIMETRO_H
