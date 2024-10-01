#ifndef NETWORKCONTROL_H
#define NETWORKCONTROL_H

// ===================================
// INCLUDE
// Vengono inclusi header esterni
// ===================================

/*! \file WIN32
    \brief Header e librerie esterne per Windows

    Il software può essere utilizzato sia su sistemi Windows-based che Linux-based.\n
    In questa parte vengono distinte gli header e le librerie necessarie per il funzionamento in ambiente Windows. \n
    Vengono inoltre definite delle variabili che sono presenti in Linux, ma non in Windows.
*/

#ifdef WIN32
#pragma comment (lib, "Ws2_32.lib")
#endif

#ifdef WIN32
  #include <winsock2.h>        // For socket(), connect(), send(), and recv()
#else
  #include <sys/types.h>       // For data types
  #include <sys/socket.h>      // For socket(), connect(), send(), and recv()
  #include <netdb.h>           // For gethostbyname()
  #include <arpa/inet.h>       // For inet_addr()
  #include <unistd.h>          // For close()
  #include <netinet/in.h>      // For sockaddr_in
  #include <errno.h>           // For errno
  #include <unistd.h>
  #define INVALID_SOCKET -1
  #define SOCKET_ERROR   -1
#endif

/*! \brief Header e librerie esterne per Linux

    In questa parte vengono distinte gli header e le librerie necessarie per il funzionamento in ambiente Linux. \n
    Viene inoltre definito un tipo (SOCKET) non presente in ambiente Linux.
*/

#include <stdlib.h>
#include <stdio.h>
#include <stdexcept>
#include <string.h>

#ifndef WIN32
    typedef int SOCKET;
#endif


// ===================================
// DEFINE
// Vengono definite delle variabili globali
// ===================================

//#define DEFAULT_BUFLEN 1500
#define DEFAULT_PORT 27015
#define MAX_STREAM_SIZE 10*1024*1024 // 10MB

using namespace std;

//====================================================================
// ENUM
// Utilizzati per identificare univocamente protocolli di comunicazioni e se viene utilizzato come client o server
//====================================================================

/**
 *  Protocolli di comunicazione
**/

enum CommunicationProtocol
{
  /*  0*/ TCP,              /*!< 0: Protocollo di comunicazione: TCP */
  /*  1*/ UDP               /*!< 1: Protocollo di comunicazione: UDP */
};

/**
 *  Modalità di funzionamento
**/

enum ComputerFunction
{
  /*  0*/ COMPUTER_FUNCTION_SERVER,             /*!< 0: Modalità di funzionamento: Server */
  /*  1*/ COMPUTER_FUNCTION_CLIENT              /*!< 0: Modalità di funzionamento: Client */
};

/*!
    \class NetworkControl
  \brief Classe relativa al funzionamento base della comunicazione ethernet

  Si tratta della classe che contiene le funzioni base per lo scambio di dati tramite ethernet.

*/


class NetworkControl
{
  public:

    // ===================== //
    // COSTRUTTORE
    // ===================== //
    // ! Costruttore
    /*!
      Crea un oggetto NetworkControl con valori di default.
    */

    NetworkControl();

    // ===================== //
    // FUNZIONI OPERATIVE
    // ===================== //
    // ! Inviare dati
    /*!
      Permette di inviare dati inserendo un buffer di caratteri contententi i dati e la lunghezza di tale buffer.
      \param buffer buffer di caratteri che verrà inviato
      \param bufferLength lunghezza del buffer da inviare
    */

    int sendData(char* buffer, unsigned int bufferLength);

    // ! Ricevere dati
    /*!
      Permette di ricevere dati inserendo un buffer di caratteri contententi i dati e la lunghezza di tale buffer.
      \param buffer buffer di char in cui verranno messi i dati appena ricevuti
      \param bufferLength lunghezza del buffer a disposizione per la ricezione
    */

    int receiveData(char* buffer, unsigned int bufferLength);

    // ! Ottenere l'ultimo errore in fase di connessione/send-receive di dati
    /*!
      Permette di ottenere il codice di errore quando un'operazione a livello di connessione e di invio/ricezione dati non va a buon fine.
    */

    int getLastError();

    // ! Impostare un tempo di timeout per una socket
    /*!
      Permette di impostare un tempo (timeout) durante il quale viene tenuta aperta una socket per la comunicazione.
      \param timeoutMilliseconds lasso di tempo, espresso in millisecondi, nel quale viene tenuta aperta la socket
    */

    void setSocketTimeout(unsigned int timeoutMilliseconds);

  protected:
    //char dataBuffer_[DEFAULT_BUFLEN];
    SOCKET communicationSocket_;
	//fd_set readfds;	//pier
    struct sockaddr_in serverAddressInfo_;
    struct sockaddr_in clientAddressInfo_;
    struct sockaddr_in localAddressInfo_;
    int communicationProtocol_;
    unsigned int port_;
    ComputerFunction computerFunction_;
  protected:
    void initializeWinsock();
    SOCKET createSocket();
    void bindSocket();
    void initAddressInfo(struct sockaddr_in& addressInfo);
    void closeInterface();
    void shutdownConnection();
    void terminateSession();
    int closeSocket(SOCKET socket);
private:
    int lastError_;
};

#endif // NETWORKCONTROL_H
