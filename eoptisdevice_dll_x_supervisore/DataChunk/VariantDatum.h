#ifndef VARIANTDATUM_H
#define VARIANTDATUM_H

#include <stdio.h>
#include <memory.h>
#include <stdexcept>
#include <string.h>
#include "DataChunkDefs.h"

/*!
    \class VariantDatum
  \brief Classe alla base della comunicazione e dello scambio dati

  Si tratta della classe che contiene il valore, più ulteriori informazioni quali la lunchezza in bute ed
  il tipo di valore, necessario per le operazioni del sistema.\n
*/

class VariantDatum
{
public:
    static DataDescriptor getDataDescriptorGivenID(SYSTEM_ID id);
public:

    // ===================== //
    // COSTRUTTORE
    // ===================== //
    // ! Costruttore
    /*!
      Crea un oggetto VariantDatum con valori di default.
    */

    VariantDatum();

    // ===================== //
    // DISTRUTTORE
    // ===================== //
    // ! Distruttore
    /*!
      Elimina l'oggetto una volta esaurito il suo utilizzo.
    */

    ~VariantDatum();

    // ===================== //
    // RIDEFINIZIONE OPERATORE
    // ===================== //
    // ! Ridefizione dell'operatore uguale ( = )
    /*!
      Con questa ridefinizione è possibile rendere uguali due oggetti VariantDatum senza la necessità di "copiare" elemento per elemento.
      \param data VariantDatum da copiare
      \return VariantDatum con valore uguale a quello passato come parametro
    */

    VariantDatum& operator=(const VariantDatum& data);

    // ===================== //
    // FUNZIONI OPERATIVE
    // ===================== //
    // ! Inserire un valore ed un ID all'interno di un VariantDatum
    /*!
      Permette di inserire un ID, a cui automaticamente verranno assegnati tipo e dimensione tramite DataDescriptor, ed un valore.
      \param id ID relativo al parametro desiderato
      \param data Valore da impostare
      \return Eventuali errori
    */

    ERROR_CODE setData(SYSTEM_ID id, DATA_BYTE* data);

    // ! Ottenere ID
    /*!
      Permette di ottenere l'ID (espresso in un valore unsigned int relativo all'enum presente in DataChunkDefs.h).
      \return ID sotto forma di numero intero (relativo a enum corrispondente)
    */

    SYSTEM_ID getID();

    // ! Ottenere la lunghezza del dato in byte
    /*!
      Permette di ottenere la lunghezza in byte del valore presente all'interno dell'oggetto.
      \return Lunghezza in termini di byte
    */

    unsigned int getLength();

    // ! Ottenere il tipo del dato
    /*!
      Permette di ottenere il tipo del valore presente all'interno dell'oggetto.
      \return Tipo sotto forma di numero intero (relativo a enum corrispondente)
    */

    SYSTEM_TYPE getType();

    // ! Ottenere il valore all'interno dell'oggetto
    /*!
      Permette di ottenere il valore contenuto in un oggetto VariantDatum e di salvarlo all'interno della variabile passata alla funzione.
      \param data DATA_BYTE in cui inserire il valore presente all'interno del VariantDatum selezionato
      \return Eventuali errori
    */

    ERROR_CODE getData(DATA_BYTE* data);

    // ! Stampare a video il contenuto di un oggetto
    /*!
      Permette di ottenere a video tutte le informazioni contenuto all'interno di un VariantDatum (ID, tipo, lunghezza in byte e valore).
    */

    void print();


    // ! Scrivere su file txt il contenuto di un VariantDatum
    /*!
      Permette di scrivere in un file txt tutte le informazioni contenuto all'interno di un VariantDatum (ID, tipo, lunghezza in byte e valore).
      \param file File txt in cui scrivere i dati contenuti in VariantDatum (passato dalla funzione analoga di DataChunk)
    */

    void writeValuesOnFile(FILE *file);

//private:
    SYSTEM_ID id_;
    unsigned int length_;
    SYSTEM_TYPE type_;
    DATA_BYTE* data_;
    //InputParameters inputParameters_;
private:
    void clear();
    void printType(SYSTEM_TYPE type);
    void printTypeMeasure();
    void printTypeShortBuffer();
    void printTypeUnsignedShortBuffer();
    void printTypeOnFile(SYSTEM_TYPE type, FILE *file);
    void printTypeMeasureOnFile(FILE *file);
    void printTypeShortBufferOnFile(FILE *file);
    void printTypeUnsignedShortBufferOnFile(FILE* file);
};

#endif // VARIANTDATUM_H
