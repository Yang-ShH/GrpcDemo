using System.Collections.Concurrent;
using DemoGrpcService.Web.BaseService.Cache.Interface;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace DemoGrpcService.Web.BaseService.Cache.Services
{
    public class RedisHelperService : IRedisHelper
    {
        private string _redisConnectionString = "127.0.0.1:6379,password=123456,connecttimeout=2000,defaultdatabase=1";
        private ConfigurationOptions _options;
        private ConnectionMultiplexer _connection;
        private readonly ConcurrentDictionary<int, IDatabase> _dbDictionary = new ConcurrentDictionary<int, IDatabase>();

        public RedisHelperService(string connectionStr)
        {
            if (!string.IsNullOrEmpty(connectionStr))
            {
                _redisConnectionString = connectionStr;
            }
            _options = RedisSetting(_redisConnectionString);
            _connection = RedisConnection();
        }

        private ConfigurationOptions RedisSetting(string connectionStr)
        {
            var options =
                ConfigurationOptions.Parse(connectionStr);
            options.AbortOnConnectFail = false;
            return options;
        }

        private object _locker = new object();
        private ConnectionMultiplexer RedisConnection()
        {
            if (_connection == null || !_connection.IsConnected)
            {
                lock (_locker)
                {
                    if (_connection == null || !_connection.IsConnected)
                    {
                        // 释放 重连
                        if (_connection != null)
                        {
                            _connection.Dispose();
                        }
                        _connection = ConnectionMultiplexer.Connect(_options);

                        _connection.ConnectionFailed += new EventHandler<ConnectionFailedEventArgs>(MuxerConnectionFailed);
                        _connection.ConnectionRestored += new EventHandler<ConnectionFailedEventArgs>(MuxerConnectionRestored);
                        _connection.ErrorMessage += new EventHandler<RedisErrorEventArgs>(MuxerErrorMessage);
                        _connection.ConfigurationChanged += new EventHandler<EndPointEventArgs>(MuxerConfigurationChanged);
                        _connection.HashSlotMoved += new EventHandler<HashSlotMovedEventArgs>(MuxerHashSlotMoved);
                        _connection.InternalError += new EventHandler<InternalErrorEventArgs>(MuxerInternalError);
                    }
                }
            }

            return _connection;
        }

        private void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
        }

        private void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
        }

        private void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
        }

        private void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
        }

        private void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
        }

        private void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
        }

        public T Get<T>(string key)
        {
            return CommonMethod.JsonDeserialize<T>((string)GetDatabase().StringGet(key));
        }

        public Task<T> GetAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string key, T value, TimeSpan expireTimeSpan = default(TimeSpan))
        {
            if (expireTimeSpan.TotalSeconds > 0.0)
            {
                GetDatabase().StringSet(key, CommonMethod.JsonSerialize(value), expireTimeSpan);
            }
            else
            {
                GetDatabase().StringSet(key, CommonMethod.JsonSerialize(value));
            }
            
        }

        public Task SetAsync<T>(string key, T value, TimeSpan expireTimeSpan = default(TimeSpan))
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task ListLeftPushAsync<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public Task ListRightPushAsync<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public Task<T> ListLeftPopAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task<T> ListRightPopAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public T ListRightPop<T>(string key)
        {
            throw new NotImplementedException();
        }

        public T ListLeftPop<T>(string key)
        {
            throw new NotImplementedException();
        }

        public long ListRightPush<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public long ListLeftPush<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public bool HashSet<T>(string redisKey, string hashField, T value)
        {
            throw new NotImplementedException();
        }

        public long HashDelete(string redisKey, params string[] hashFields)
        {
            throw new NotImplementedException();
        }

        public List<T> HashGetAllValue<T>(string redisKey)
        {
            throw new NotImplementedException();
        }

        public T HashGet<T>(string redisKey, string hashField)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string key)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(string[] keys)
        {
            throw new NotImplementedException();
        }

        public Task<long> RemoveRangeAsync(string[] keys)
        {
            throw new NotImplementedException();
        }

        public bool SetNx(string key, string value)
        {
            throw new NotImplementedException();
        }

        public IDatabase GetDatabase(int dbIndex = 0)
        {
            int num = dbIndex > 0 ? dbIndex : _options.DefaultDatabase.GetValueOrDefault();
            IDatabase database1;
            if (_dbDictionary.TryGetValue(num, out database1))
            {
                return database1;
            }
            IDatabase database2 = _connection.GetDatabase(num);
            _dbDictionary.TryAdd(num, database2);
            return database2;
        }

        public IServer GetServer(string host, int port)
        {
            throw new NotImplementedException();
        }

        public long Publish(string channel, string message)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(string channelFrom)
        {
            throw new NotImplementedException();
        }
    }
}
