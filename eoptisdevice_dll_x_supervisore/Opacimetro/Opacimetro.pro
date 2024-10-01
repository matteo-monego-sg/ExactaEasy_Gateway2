#-------------------------------------------------
#
# Project created by QtCreator 2015-01-12T16:14:21
#
#-------------------------------------------------

QT       -= gui

TARGET = Opacimetro
TEMPLATE = lib

DEFINES += OPACIMETRO_LIBRARY
DEFINES += _CRT_SECURE_NO_WARNINGS
DEFINES -=  UNICODE \
            _UNICODE

SOURCES += opacimetro.cpp

HEADERS += opacimetro.h\
        opacimetro_global.h

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
