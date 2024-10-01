using InterProcessComm.Messaging;
using MessagePack;
using SPAMI.Util.Logger;
using System;
using System.IO;

namespace OptrelInterProcessComm.Utils
{
    /// <summary>
    /// Serialization/deserialization logic of the IPC communication.
    /// </summary>
    public class IPCSerialization
    {
        #region Newtonsoft.JSON serialization
        ///// <summary>
        ///// Gets an IPCMessage from a JSON serialized structure.  
        ///// </summary>
        //public static IPCMessage Deserialize(string json)
        //{
        //    if (string.IsNullOrWhiteSpace(json))
        //    {
        //        Log.Line(
        //            LogLevels.Warning,
        //            "IpcSerialization::Deserialize",
        //            "JSON string reference is null or empty");
        //        return null;
        //    }

        //    try
        //    {
        //        return JsonConvert.DeserializeObject<IPCMessage>(json);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Line(
        //            LogLevels.Error,
        //            "IPCMessage::FromJson",
        //            $"JSON deserialization failed: {ex.Message}");
        //        return null;
        //    }
        //}
        ///// <summary>
        ///// Serializes the object to a MemoryStream.
        ///// </summary>
        //public static byte[] Serialize(IPCMessage message)
        //{
        //    try
        //    {
        //        return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Line(
        //            LogLevels.Error,
        //            "IPCMessage::ToJson",
        //            $"JSON serialization failed: {ex.Message}");
        //        return null;
        //    }
        //}
        #endregion

        #region MessagePack serialization
        /*  
         *  https://github.com/MessagePack-CSharp/MessagePack-CSharp
         */

        /// <summary>
        /// Gets an IPCMessage from a JSON serialized structure.  
        /// </summary>
        public static bool Deserialize(byte[] serializedData, out IPCMessage message, out Exception deserializationException)
        {
            message = null;
            deserializationException = null;

            if (serializedData is null || serializedData.Length.Equals(0))
            {
                Log.Line(
                    LogLevels.Warning,
                    "IPCMessage::Deserialize",
                    "MessagePack deserialization failed: byte[] parameter reference is null or empty.");
                return false;
            }

            try
            {
                /*
                 *   https://github.com/MessagePack-CSharp/MessagePack-CSharp?tab=readme-ov-file#lz4-compression
                 *   
                 *   Lz4BlockArray compresses an entire MessagePack sequence as a array of LZ4 blocks. 
                 *   Compressed/decompressed blocks are chunked and thus do not enter the GC's Large-Object-Heap, 
                 *   but the compression ratio is slightly worse.
                 *   We recommend to use Lz4BlockArray as the default when using compression. 
                 *   For compatibility with MessagePack v1.x, use Lz4Block.
                */
                var lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);
                message = MessagePackSerializer.Deserialize<IPCMessage>(serializedData, lz4Options);
                return true;
            }
            catch (MessagePackSerializationException mpsex)
            {
                Log.Line(
                    LogLevels.Error,
                    "IPCMessage::Deserialize",
                    $"MessagePack deserialization failed [MessagePackSerializationException]: {mpsex.Message}");
                deserializationException = mpsex;
                return false;
            }
            catch (Exception ex)
            {
                Log.Line(
                    LogLevels.Error,
                    "IPCMessage::Deserialize",
                    $"MessagePack deserialization failed [Exception]: {ex.Message}");
                deserializationException = ex;
                return false;
            }
        }
        /// <summary>
        /// Serializes the object to a MemoryStream.
        /// </summary>
        public static bool Serialize(IPCMessage message, out byte[] serializedMessage, out Exception serializationException)
        {
            serializedMessage = null;
            serializationException = null;

            if (message is null)
            {
                Log.Line(
                    LogLevels.Error,
                    "IPCMessage::Serialize",
                    $"MessagePack serialization failed: the reference to the IPCMessage parameter is a null reference.");
                return false;
            }

            try
            {
                /*
                *   https://github.com/MessagePack-CSharp/MessagePack-CSharp?tab=readme-ov-file#lz4-compression
                *   
                *   Lz4BlockArray compresses an entire MessagePack sequence as a array of LZ4 blocks. 
                *   Compressed/decompressed blocks are chunked and thus do not enter the GC's Large-Object-Heap, 
                *   but the compression ratio is slightly worse.
                *   We recommend to use Lz4BlockArray as the default when using compression. 
                *   For compatibility with MessagePack v1.x, use Lz4Block.
               */
                var ms = new MemoryStream();
                var lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);
                MessagePackSerializer.Serialize(ms, message, lz4Options);
                serializedMessage = ms.ToArray();
                return true;
            }
            catch (MessagePackSerializationException mpsex)
            {
                Log.Line(
                    LogLevels.Error,
                    "IPCMessage::Serialize",
                    $"MessagePack serialization failed [MessagePackSerializationException]: {mpsex.Message}");
                serializationException = mpsex;
                return false;
            }
            catch (Exception ex)
            {
                Log.Line(
                    LogLevels.Error,
                    "IPCMessage::Serialize",
                    $"MessagePack serialization failed: {ex.Message}");
                serializationException = ex;
                return false;
            }
        }
        #endregion
    }
}
