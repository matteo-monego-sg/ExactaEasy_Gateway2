README SUPERVISORE

Il supervisore (ExactaEasy.exe) ha come progetto principale ExactaEasyUI. Inoltre ci sono varie dll ausiliarie (DisplayManager, EoptisClient2, ExactaEasyCore, ExactaEasyEng, ExEyGateway, ExEyGtwTest, GretelClient, …)

ExactaEasyUI
References:
Qui vengono gestite le varie form, menu, lingue

ExactaEasyCore
References: Logger, LightController (serve solo per macchine Tattile), Utilities
Sono presenti le classi per la configurazione macchina (cfr. machineConfig.xml e globalConfig.xml) con le relative classi per provider e settings di nodi, stazioni e camere. Inoltre la classe TaskObserver serve a monitorare e aggiornare lo stato di vari task (ad esempio nella splash screen). 

ExEyGateway
References: ExactaEasyEng
È il cavallo di troia usato in iFix per comunicare col supervisore di visione via WCF. Nella classe ExEyGatewayCtrl sono implementati eventi e proprietà pubblici per gestire la comunicazione col supervisore.

ExactaEasyEng
References: ExactaEasyCore, Logger, LightController (serve solo per macchine Tattile), Utilities
Gestisce la comunicazione con HMI (AppEngine) e le proprietà che HMI può modificare (AppContext) tra cui: posizione sullo schermo del supervisore, lingua, livello utente, machine mode, machine info (abilitazione stazioni -> REMINDER: la sequenza dei bit di abilitazione inviati da HMI deve essere la stessa dell’ordine degli station settings su machineConfig del supervisore, velocità), supervisor mode (cfr: colore occhio HMI), batch (non usato, ma utile se si decide ad esempio di far partire il salvataggio immagini all’inizio di un batch, database (usato solo per momentive, commessa spami).

ExEyGtwTest
Interfaccina usata per simulare le chiamate da e per HMI.

DisplayManager
References: MSTSCLib (Desktop Remoto), EmguCV (OpenCV wrapper), VNCSharp, ZedGraph (grafici), ExactaEasyEng, ExactaEasyCore, Logger, LightController (serve solo per macchine Tattile), Utilities
Non è solo il manager delle varie viste (Immagini, grafici, ...) come si evince dalla classe Display e dalle sue derivate: ScreenGridDisplay (homepage), SingleDisplay (tattile), ThumbScreenDisplay (gretel) e dalla classe Graph e dalle sue derivate: LineGraph (turbidimetro), SpectrometerGraphs (colorimetro). In questa dll ci sono anche: la gestione dei risultati delle varie ispezioni (InpsectionResults), le classi che identificano nodi, stazioni e camere le cui derivate sono presenti in EoptisClient2 e GretelClient, la gestione delle connessioni remote con i pc gretel e soprattutto la classe VisionSystemManager che gestisce nodi, stazioni e camere ed eventi ad essi correlati assieme al progetto principale (ExactaEasyUI).

EoptisClient2
References: DisplayManager, EmguCV (OpenCV wrapper), ExactaEasyEng, ExactaEasyCore, Logger
Implementazione di nodi, stazioni e camere (i tre concetti coincidono in questo caso) per i device Eoptis (turbidimetro e colorimetro). Tutte le chiamate alla dll (c++) “EoptisDeviceManager” passano per la classe statica EoptisSvc.

GretelClient
References: DisplayManager, EmguCV (OpenCV wrapper), ExactaEasyEng, ExactaEasyCore, Logger
Implementazione di nodi, stazioni e camere (nodi=minipc, stazioni e camere coincidono) per le varie istanze di Gretel. Tutte le chiamate alla dll (c++) "EtherComm.dll" compilata nel progetto Gretel (deve essere uguale a quella presente nei minipc!) passano per la classe statica GretelSvc.
