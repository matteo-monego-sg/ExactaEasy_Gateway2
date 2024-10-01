#-------------------------------------------------
#
# Project created by QtCreator 2015-02-06T09:49:44
#
#-------------------------------------------------

QT       -= core gui

TARGET = Network
TEMPLATE = lib
CONFIG += staticlib

DEFINES += _CRT_SECURE_NO_WARNINGS
DEFINES -=  UNICODE \
            _UNICODE

SOURCES += \
    ClientControl.cpp \
    NetworkControl.cpp \
    ServerControl.cpp

HEADERS += \
    ClientControl.h \
    NetworkControl.h \
    ServerControl.h
unix {
    target.path = /usr/lib
    INSTALLS += target
}

win32:CONFIG(release, debug|release): LIBS += -L$$PWD/../ -lpthreadVCE2
else:win32:CONFIG(debug, debug|release): LIBS += -L$$PWD/../ -lpthreadVCE2d
else:unix:!macx: LIBS += -L$$PWD/../ -lpthreadVCE2

win32:INCLUDEPATH += $$PWD/../PthreadHeader
win32:DEPENDPATH += $$PWD/../PthreadHeader
