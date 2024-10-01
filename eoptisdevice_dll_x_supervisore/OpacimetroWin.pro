TEMPLATE = subdirs

SUBDIRS += \
    Opacimetro \
    OpacimetroTest \
    DataChunk \
    DataChunkTest \
    Network \
    EoptisDeviceManager

Opacimetro.depends = DataChunk
Opacimetro.depends = Network
OpacimetroTest.depends = Opacimetro
OpacimetroTest.depends = EoptisDeviceManager
DataChunkTest.depends = DataChunk
