using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DisplayManager;
using SPAMI.Util.Logger;

namespace TattileCameras {
    class TattileStationBase : IStation {

        /// <summary>
        /// Connect to station
        /// </summary>
        public void Connect() {
            if (!Initialized)
                throw new CameraException("Camera not initialized");
            InputBuffer.Clear();
            if (Connected) Disconnect();
            if (alertThread == null || !alertThread.IsAlive) {
                alertThread = new Thread(new ThreadStart(AlertThread));
                alertThread.Name = "TattileCamera Alert Thread";
            }
            if (acqThread == null || !acqThread.IsAlive) {
                acqThread = new Thread(new ThreadStart(AcquisitionThread));
                acqThread.Name = "TattileCamera Acquisition Thread";
            }
            int res = 0;
            long Value = 0;

            uint RxBufferSize = 0;
            uint RxQueueSize = 0;
            uint RxQueueSizeMax = 0;
            uint channels = 1;
            if (CameraType == "M12")
                channels = 3;

            CameraInfoDict[cameraIdentity] = GetCameraInfo();
            RxBufferSize = (uint)(CameraInfoDict[cameraIdentity].widthImage * CameraInfoDict[cameraIdentity].heightImage * channels);

            RxQueueSize = 0;
            RxQueueSizeMax = 20;
            string cameraIP = Utilities.IPV4AddressUInt2String(CameraInfoDict[cameraIdentity].ipAddress);
            string nicIP = Utilities.IPV4AddressUInt2String(CameraInfoDict[cameraIdentity].pcIfAddress);

            if (NICCameraDict.ContainsKey(nicIP))
                return;

            res = TattileTagFilterSvc.TAG_ConnectAdapter(new StringBuilder(nicIP), out m_Eth_port_handle);
            if (res != 0)
                throw new CameraException("TAG_ConnectAdapter return " + ((TAGFILTER_ERROR_CODE)res).ToString());

            res = TattileTagFilterSvc.TAG_ConnectDevice(m_Eth_port_handle, new StringBuilder(cameraIP), RxBufferSize, RxQueueSizeMax, ref RxQueueSize, out m_Camera_handle);
            if (res != 0)
                throw new CameraException("TAG_ConnectDevice return " + ((TAGFILTER_ERROR_CODE)res).ToString());


            long LiveRun_port = 0;

            RxProtocol = ImageProtocol.IMAGE_PROTOCOL_TOJECT;

            if (CameraType == "M9") {
                port = 20000;
                RxProtocol = ImageProtocol.IMAGE_PROTOCOL_TOJECT;
            }
            else if (CameraType == "M12") {
                port = 12345;
                RxProtocol = ImageProtocol.IMAGE_PROTOCOL_GIGE;
            }
            else {
                Log.Line(LogLevels.Error, "TattileCamera.Connect", "Protocol not supported yet or invalid protocol");
                throw new CameraException("Protocol not supported yet or invalid protocol");
            }

            res = TattileTagFilterSvc.TAG_SetMode(m_Camera_handle, (int)RxProtocol, port);
            if (res != 0)
                throw new CameraException("TAG_SetMode return " + ((TAGFILTER_ERROR_CODE)res).ToString());

            if (!alertThread.IsAlive)
                alertThread.Start();
            if (!acqThread.IsAlive)
                acqThread.Start();

            try {
                AddConnection(Utilities.IPV4AddressUInt2String(CameraInfoDict[cameraIdentity].pcIfAddress),
                    Utilities.IPV4AddressUInt2String(CameraInfoDict[cameraIdentity].ipAddress));
            }
            catch {
                throw;
            }

            Connected = true;
        }

        /// <summary>
        /// Disconnect camera
        /// </summary>
        public void Disconnect() {

            Connected = false;
            StopGrab();
            //while (wait4kill) 
            Thread.Sleep(500);
            int res = 0;
            if (m_Camera_handle != IntPtr.Zero) {
                res = TattileTagFilterSvc.TAG_ResetDeviceQueue(m_Camera_handle, 1);
                if (res != 0)
                    throw new CameraException("TAG_ResetDeviceQueue return " + ((TAGFILTER_ERROR_CODE)res).ToString());

                res = TattileTagFilterSvc.TAG_DisconnectDevice(ref m_Camera_handle);
                if (res != 0)
                    throw new CameraException("TAG_DisconnectDevice return " + ((TAGFILTER_ERROR_CODE)res).ToString());

                if (m_Eth_port_handle != IntPtr.Zero) {
                    res = TattileTagFilterSvc.TAG_DisconnectAdapter(ref m_Eth_port_handle);
                    if (res != 0)
                        throw new CameraException("TAG_DisconnectAdapter return " + ((TAGFILTER_ERROR_CODE)res).ToString());
                }
            }
            m_Camera_handle = IntPtr.Zero;
            m_Eth_port_handle = IntPtr.Zero;
            try {
                RemoveConnection(Utilities.IPV4AddressUInt2String(CameraInfoDict[cameraIdentity].pcIfAddress),
                    Utilities.IPV4AddressUInt2String(CameraInfoDict[cameraIdentity].ipAddress));
            }
            catch {
                throw;
            }
        }
    }
}
