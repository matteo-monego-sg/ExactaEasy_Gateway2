#ifndef SERVERCONTROL_H
#define SERVERCONTROL_H

// ===================================
// INCLUDE
// Vengono inclusi header esterni
// ===================================

#include <pthread.h>
#include "NetworkControl.h"

/*! \file pthread.h
    \brief Header esterno per le funzionalità dei thread

    Contiene le diverse funzioni che permettono d'instaurare un thread che possa essere eseguito in parallelo al sowftware principale.\n
    Utilizzato per rimanere in attesa di eventuali connessioni da parte di un client.
*/

/*! \file NetworkControl.h
    \brief Header con funzioni base della comunicazione ethernet

    Contiene le diverse funzioni che permettono lo scambio di dati tramite protocollo ethernet.
*/

/*!
    \class ServerControl
  \brief Classe relativa alla parte Server della comunicazione ethernet

  Si tratta della classe che descrive il lato server in una comunicazione di tipo ethernet.

*/


class ServerControl : public NetworkControl
{
  public:

    // ===================== //
    // COSTRUTTORE
    // ===================== //
    // ! Costruttore
    /*!
      Crea un oggetto ServerControl.
    */

    ServerControl();

    // ===================== //
    // FUNZIONI OPERATIVE
    // ===================== //
    // ! Aprire la comunicazione lato server
    /*!
      Permette di aprire una comunicazione lato server inserendo un valore relativo al protocollo di comunicazione (TCP/UDP) e la porta di comunicazione
      \param communicationProtocol protocollo da utilizzare per la comunicazione (TCP o UDP)
      \param port numero della porta per la comunicazione
    */

    void openServer(CommunicationProtocol communicationProtocol, unsigned int port);

    /*!
      Controlla se qualcuno è connesso al server
    */
    bool isClientConnected();

    bool closeServer();
  private:
    bool clientConnected_;
    pthread_t waitClientThread_;
  private:
    void setServerAddressInfo();
    void createWaitForIncomingClientThread();
    //void *waitForIncomingClient();
    void serverProcessing();
    void serverReceiveStream();
};

#endif // SERVERCONTROL_H
