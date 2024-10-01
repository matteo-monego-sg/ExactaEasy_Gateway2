#-------------------------------------------------
#
# Project created by QtCreator 2015-03-30T14:31:31
#
#-------------------------------------------------

QT       -= gui

TARGET = EoptisDeviceManager
TEMPLATE = lib

DEFINES += EOPTISDEVICEMANAGER_LIBRARY

SOURCES +=  eoptisdevicemanager.cpp \
    EoptisDeviceManagerPrivate.cpp

HEADERS +=    eoptisdevicemanager.h \
    eoptisdevicemanager_global.h \
    EoptisDeviceManagerPrivate.h

unix {
    target.path = /usr/lib
    INSTALLS += target
}


win32:CONFIG(release, debug|release): LIBS += -L$$OUT_PWD/../DataChunk/release/ -lDataChunk
else:win32:CONFIG(debug, debug|release): LIBS += -L$$OUT_PWD/../DataChunk/debug/ -lDataChunk
else:unix: LIBS += -L$$OUT_PWD/../DataChunk/ -lDataChunk

INCLUDEPATH += $$PWD/../DataChunk
DEPENDPATH += $$PWD/../DataChunk

win32-g++:CONFIG(release, debug|release): PRE_TARGETDEPS += $$OUT_PWD/../DataChunk/release/libDataChunk.a
else:win32-g++:CONFIG(debug, debug|release): PRE_TARGETDEPS += $$OUT_PWD/../DataChunk/debug/libDataChunk.a
else:win32:!win32-g++:CONFIG(release, debug|release): PRE_TARGETDEPS += $$OUT_PWD/../DataChunk/release/DataChunk.lib
else:win32:!win32-g++:CONFIG(debug, debug|release): PRE_TARGETDEPS += $$OUT_PWD/../DataChunk/debug/DataChunk.lib
else:unix: PRE_TARGETDEPS += $$OUT_PWD/../DataChunk/libDataChunk.a

win32:CONFIG(release, debug|release): LIBS += -L$$OUT_PWD/../Network/release/ -lNetwork
else:win32:CONFIG(debug, debug|release): LIBS += -L$$OUT_PWD/../Network/debug/ -lNetwork
else:unix: LIBS += -L$$OUT_PWD/../Network/ -lNetwork

INCLUDEPATH += $$PWD/../Network
DEPENDPATH += $$PWD/../Network

win32-g++:CONFIG(release, debug|release): PRE_TARGETDEPS += $$OUT_PWD/../Network/release/libNetwork.a
else:win32-g++:CONFIG(debug, debug|release): PRE_TARGETDEPS += $$OUT_PWD/../Network/debug/libNetwork.a
else:win32:!win32-g++:CONFIG(release, debug|release): PRE_TARGETDEPS += $$OUT_PWD/../Network/release/Network.lib
else:win32:!win32-g++:CONFIG(debug, debug|release): PRE_TARGETDEPS += $$OUT_PWD/../Network/debug/Network.lib
else:unix: PRE_TARGETDEPS += $$OUT_PWD/../Network/libNetwork.a

win32:CONFIG(release, debug|release): LIBS += -L$$OUT_PWD/../Opacimetro/release/ -lOpacimetro
else:win32:CONFIG(debug, debug|release): LIBS += -L$$OUT_PWD/../Opacimetro/debug/ -lOpacimetro
else:unix: LIBS += -L$$OUT_PWD/../Opacimetro/ -lOpacimetro

INCLUDEPATH += $$PWD/../Opacimetro
DEPENDPATH += $$PWD/../Opacimetro
