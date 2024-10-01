using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Timers;
using SPAMI.Util.Logger;

namespace ExactaEasy.Redis
{
    public class RedisComm
    {
        ConnectionMultiplexer _conMultiplixer;
        IDatabase _iRedisDB;
        ISubscriber _iRedisSubscriber;
        Timer _timerCheckConn;
        bool _timerBusy;

        string _hostName;
        string _port;
        string _password;

        bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
        }


        public RedisComm()
        {
            _conMultiplixer = null;
            _iRedisDB = null;

            _timerCheckConn = new Timer();
            _timerCheckConn.Interval = 30000;
            _timerCheckConn.Elapsed += _timerCheckConn_Elapsed;
            //_timerCheckConn.Start();

            _timerBusy = false;
        }


        bool CreateConnection(bool insertLog, string hostname, string port, string password)
        {
            _hostName = hostname;
            _port = port;
            _password = password;

            try
            {
                _conMultiplixer = ConnectionMultiplexer.Connect(hostname + ":" + port + ",password=" + password);
                _iRedisDB = _conMultiplixer.GetDatabase();
                _iRedisSubscriber = _conMultiplixer.GetSubscriber();
                _isConnected = true;
                if(insertLog)
                    Log.Line(LogLevels.Pass, "RedisComm.CreateConnection", $"redis is connected to Hostname='{hostname}' Port='{port}'");
                return true;
            }
            catch (Exception ex)
            {
                _isConnected = false;
                if(insertLog)
                    Log.Line(LogLevels.Error, "RedisComm.CreateConnection", $"redis connection error to Hostname='{hostname}' Port='{port}'");
                return false;
            }
        }

        public bool CreateConnection(string hostname, string port, string password)
        {
            //start timer
            _timerCheckConn.Start();
            return CreateConnection(true, hostname, port, password);
        }

        private void _timerCheckConn_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(_timerBusy)
                return;
            _timerBusy = true;

            bool toRetry = false;
            if (_conMultiplixer == null)
            {
                _isConnected = false;
                toRetry = true;
            }
            else if (_conMultiplixer.IsConnected == false)
                toRetry = true;

            if (toRetry)
            {
                //retry the connection
                try
                {
                    bool success = CreateConnection(false, _hostName, _port, _password);
                    if (success)
                        Log.Line(LogLevels.Pass, "RedisComm._timerCheckConn_Elapsed", $"redis retry connection successfully to Hostname='{_hostName}' Port='{_port}'");
                    else
                        Log.Line(LogLevels.Error, "RedisComm._timerCheckConn_Elapsed", $"redis retry connection failed to Hostname='{_hostName}' Port='{_port}'");

                    _isConnected = success;
                }
                catch(Exception ex)
                {
                    _isConnected = false;
                    Log.Line(LogLevels.Error, "RedisComm._timerCheckConn_Elapsed", $"redis retry connection FATAL ERROR to Hostname='{_hostName}' Port='{_port}', message: {ex.Message}");
                }
            }

            //set is connected
            if (_isConnected == false && _conMultiplixer.IsConnected)
                _isConnected = true;

            _timerBusy = false;
        }




        public string GetValue(string key)
        {
            var value = _iRedisDB.StringGet(key);
            return value;
        }


        public List<string> GetValues(string key)
        {
            List<string> results = new List<string>();

            RedisValue[] items = _iRedisDB.ListRange(key) ?? new RedisValue[0];
            foreach (RedisValue redisValue in items)
            {
                results.Add(redisValue.ToString());
            }

            return results;
        }


        public bool SetValue(string key, string value)
        {
            return _iRedisDB.StringSet(key, value);
        }

        public Task<bool> SetValueAsync(string key, string value)
        {
            return _iRedisDB.StringSetAsync(key, value);
        }


        public long PublishMessage(string channel, string message)
        {
            return _iRedisSubscriber.Publish(channel, message);
        }

        public Task<long> PublishMessaheAsync(string channel, string message)
        {
            return _iRedisSubscriber.PublishAsync(channel, message);
        }
    }
}
