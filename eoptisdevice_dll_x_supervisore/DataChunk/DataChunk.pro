#-------------------------------------------------
#
# Project created by QtCreator 2015-01-16T12:52:00
#
#-------------------------------------------------

QT       -= core gui

TARGET = DataChunk
TEMPLATE = lib
CONFIG += staticlib

DEFINES += _CRT_SECURE_NO_WARNINGS
DEFINES -=  UNICODE \
            _UNICODE

SOURCES += \
    DataChunk.cpp \
    VariantDatum.cpp

HEADERS += \
    DataChunk.h \
    DataChunkDefs.h \
    VariantDatum.h
unix {
    target.path = /usr/lib
    INSTALLS += target
}
