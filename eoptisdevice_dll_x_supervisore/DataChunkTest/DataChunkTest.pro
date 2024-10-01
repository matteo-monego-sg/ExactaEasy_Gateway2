#-------------------------------------------------
#
# Project created by QtCreator 2015-01-16T12:52:14
#
#-------------------------------------------------

QT       += core

QT       -= gui

TARGET = DataChunkTest
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
