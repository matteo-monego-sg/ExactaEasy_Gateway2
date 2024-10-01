    #ifndef DATACHUNK_H
#define DATACHUNK_H

// ===================================
// INCLUDE
// Vengono inclusi header esterni
// ===================================
#include <stdio.h>
#include "DataChunkDefs.h"
#include "VariantDatum.h"

/*! \file DataChunkDefs.h
    \brief Header con enum relativi al funzionamento del sistema

    Contiene le diverse liste di enum comprendenti gli ID atti al funzionamento del sistema.\n
    Viene descritta anche una struttura, DataDescriptor, che ad ogni ID associa le relative informazioni
    (tipo, lunghezza in byte).\n
    Contiene inoltre la definizione di DATA_TYPE, tipo dei dati contenuti in VariantDatum e quindi DataChunk.

    \file VariantDatum.h
    \brief Header relativo al dato contenuto in DataChunk

    Ogni DataChunk possiede un valore relativo ai VariantDatum in esso contenuti ed i VariantDatum stessi.
*/

/*!
    \class DataChunk
  \brief Classe alla base della comunicazione e dello scambio dati

  Si tratta della classe che descrive il dato che client e server si scambiano per comunicare tra loro.\n
  Al suo interno c'è una classe che contiene il valore effettivo del dato.
*/


class DataChunk
{
public:
    static unsigned int getMaxNumberOfData();
public:

    // ===================== //
    // COSTRUTTORE
    // ===================== //
    // ! Costruttore
    /*!
      Crea un oggetto DataChunk con valori di default.
    */

    DataChunk();

    // ===================== //
    // DISTRUTTORE
    // ===================== //
    // ! Distruttore
    /*!
      Elimina l'oggetto una volta esaurito il suo utilizzo.
    */

    ~DataChunk();

    // ===================== //
    // FUNZIONI OPERATIVE
    // ===================== //
    // ! Inserire il numero di VariantDatum dell'oggetto DataChunk
    /*!
      Permette di inserire un valore (intero) di dati contenuti in DataChunk. Crea un array di VariantDatum della dimensione appena inserita
      \param totData Numero di VariantDatum da predisporre all'interno dell'oggetto DataChunk
      \return Eventuali errori
    */

    ERROR_CODE setTotData(unsigned int totData);

    // ! Ottenere il valore di dati presenti nell'oggetto DataChunk
    /*!
      Restituisce un valore (intero) di elementi VariantDatum presenti in DataChunk
      \return Numero di VarianDatum prensenti all'interno dell'oggetto DataChunk in questione
    */

    unsigned int getTotData();

    // ! Settare un VariantDatum all'interno di un DataChunk
    /*!
      Permette d'inserire un oggetto VariantDatum all'interno di un DataChunk, indicandone (tramite index) la posizione dell'array di VariantDatum interno
      \param data VariantDatum da impostare
      \param index Posizione all'interno dell'array di VariantDatum presente in DataChunk (dipende da totData)
      \return Eventuali errori
    */

    ERROR_CODE setData(const VariantDatum& data, unsigned int index);

    // ! Ottenere un VariantDatum all'interno di un DataChunk
    /*!
      Permette di ottenere un oggetto VariantDatum all'interno di un DataChunk, indicandone (tramite index) la posizione nell'array di VariantDatum interno
      \param data VariantDatum in cui mettere i valori letti
      \param index Posizione all'interno dell'array di VariantDatum presente in DataChunk (dipende da totData) in cui andare a prendere il dato
      \return Eventuali errori
    */

    ERROR_CODE getData(VariantDatum& data, unsigned int index);

    // ! Ottenere il valore effettivo contenuto in un VariantDatum dell'oggetto DataChunk
    /*!
      Permette di ottenere direttamente il valore contenuto nel VarianDatum che occupa la posizione index all'interno dell'array
      \param data DATA_BYTE in cui mettere il dato
      \param index Posizione all'interno dell'array di VariantDatum presente in DataChunk (dipende da totData) in cui andare a prendere il dato
      \return Eventuali errori
    */

    ERROR_CODE getDataValue(DATA_BYTE *data, unsigned int index);

    // ! Leggere valori da un file
    /*!
      Permette di ottenere dei valori, tipicamente di default all'avvio, leggendoli da un file di tipo txt
      \param fileName File txt da cui estrarre i valori da settare all'interno del DataChunk (totData presente all'interno del file)
    */

    void readValuesFromFile(const char* fileName);

    // ! Stampare a video il contenuto di un DataChunk
    /*!
      Permette di ottenere a video il contenuto di un DataChunk (elementi dell'array e contenuto di ogni elemento del suddetto array)
    */

    void print();

    // ! Scrivere su file txt il contenuto di un DataChunk
    /*!
      Permette di scrivere in un file txt il contenuto di un DataChunk (elementi dell'array e contenuto di ogni elemento del suddetto array)
      \param file File txt in cui scrivere i dati contenuti in DataChunk
    */

    void writeValuesOnFile(const char *fileName);

//private:
    unsigned int totData_;
    VariantDatum* data_;
private:
    void clear();
};

#endif // DATACHUNK_H
