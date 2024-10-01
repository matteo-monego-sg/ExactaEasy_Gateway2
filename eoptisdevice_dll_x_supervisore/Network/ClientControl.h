#ifndef CLIENTCONTROL_H
#define CLIENTCONTROL_H

// ===================================
// INCLUDE
// Vengono inclusi header esterni
// ===================================


#include "NetworkControl.h"

/*! \file NetworkControl.h
    \brief Header con funzioni base della comunicazione ethernet

    Contiene le diverse funzioni che permettono lo scambio di dati tramite protocollo ethernet.
*/

/*!
    \class ClientControl
  \brief Classe relativa alla parte Client della comunicazione ethernet

  Si tratta della classe che descrive il lato client in una comunicazione di tipo ethernet.

*/

class ClientControl : public NetworkControl
{
  public:

    // ===================== //
    // COSTRUTTORE
    // ===================== //
    // ! Costruttore
    /*!
      Crea un oggetto ClientControl.
    */

    ClientControl();

    // ===================== //
    // FUNZIONI OPERATIVE
    // ===================== //
    // ! Aprire la comunicazione lato client
    /*!
      Permette di aprire la comunicazione lato client inserendo un valore relativo al protocollo di comunicazione (TCP/UDP), la porte di comunicazione (in invio e ricezione, solitamente uguali) e l'indirizzo ip del server a cui connettersi
      \param communicationProtocol protocollo di comunicazione (TPC o UDP)
      \param sendPort numero della porta in invio
      \param receivePort numero della porta in ricezione
      \param serverAddress indirizzo ip relativo al server a cui ci si connetterà
    */

    void openClient(CommunicationProtocol communicationProtocol, unsigned int sendPort, unsigned int receivePort, const char* serverAddress);

    // ! Chiudere la comunicazione lato client
    /*!
      Permette di chiudere la comunicazione client/server imbastita in precedenza
    */

    void closeClient();
  private:
    char serverAddress_[16];
    unsigned int receivePort_;
  private:
    void setServerAddressInfo();
    void connectToServer();
    void clientProcessing();
};

#endif // CLIENTCONTROL_H
