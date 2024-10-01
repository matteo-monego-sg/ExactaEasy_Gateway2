#-------------------------------------------------
#
# Project created by QtCreator 2015-01-12T16:14:51
#
#-------------------------------------------------

QT       += core

QT       -= gui

TARGET = OpacimetroTest
CONFIG   += console
CONFIG   -= app_bundle

TEMPLATE = app

DEFINES += _CRT_SECURE_NO_WARNINGS
DEFINES -=  UNICODE \
            _UNICODE

SOURCES += \
    ConsoleApp.cpp \
    InputParameters.cpp \
    main.cpp

win32:CONFIG(release, debug|release): LIBS += -L$$OUT_PWD/../Opacimetro/release/ -lOpacimetro
else:win32:CONFIG(debug, debug|release): LIBS += -L$$OUT_PWD/../Opacimetro/debug/ -lOpacimetro
else:unix: LIBS += -L$$OUT_PWD/../Opacimetro/ -lOpacimetro

INCLUDEPATH += $$PWD/../Opacimetro
DEPENDPATH += $$PWD/../Opacimetro

HEADERS += \
    ConsoleApp.h \
    InputParameters.h \
    main.h

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

win32:CONFIG(release, debug|release): LIBS += -L$$OUT_PWD/../EoptisDeviceManager/release/ -lEoptisDeviceManager
else:win32:CONFIG(debug, debug|release): LIBS += -L$$OUT_PWD/../EoptisDeviceManager/debug/ -lEoptisDeviceManager
else:unix: LIBS += -L$$OUT_PWD/../EoptisDeviceManager/ -lEoptisDeviceManager

INCLUDEPATH += $$PWD/../EoptisDeviceManager
DEPENDPATH += $$PWD/../EoptisDeviceManager
