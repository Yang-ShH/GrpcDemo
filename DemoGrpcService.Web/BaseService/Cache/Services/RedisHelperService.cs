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

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await GetDatabase().StringGetAsync(key);
            return CommonMethod.JsonDeserialize<T>((string)value);
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

        public async Task SetAsync<T>(string key, T value, TimeSpan expireTimeSpan = default(TimeSpan))
        {
            if (expireTimeSpan.TotalSeconds > 0.0)
            {
                await GetDatabase().StringSetAsync(key, CommonMethod.JsonSerialize(value), expireTimeSpan);
            }
            else
            {
                await GetDatabase().StringSetAsync(key, CommonMethod.JsonSerialize(value));
            }
        }

        public bool Remove(string key)
        {
            return GetDatabase().KeyDelete((RedisKey) key);
        }

        public Task<bool> RemoveAsync(string key)
        {
            return GetDatabase().KeyDeleteAsync((RedisKey) key);
        }

        public async Task ListLeftPushAsync<T>(string key, T value)
        {
            await GetDatabase().ListLeftPushAsync((RedisKey) key, (RedisValue) CommonMethod.JsonSerialize(value));
        }

        public async Task ListRightPushAsync<T>(string key, T value)
        {
            await GetDatabase().ListRightPushAsync((RedisKey)key, (RedisValue)CommonMethod.JsonSerialize(value));
        }

        public async Task<T> ListLeftPopAsync<T>(string key)
        {
            var value = await GetDatabase().ListLeftPopAsync((RedisKey) key);
            T obj = CommonMethod.JsonDeserialize<T>((string) value);
            return obj;
        }

        public async Task<T> ListRightPopAsync<T>(string key)
        {
            var value = await GetDatabase().ListRightPopAsync((RedisKey) key);
            T obj = CommonMethod.JsonDeserialize<T>((string) value);
            return obj;
        }

        public T ListRightPop<T>(string key)
        {
            var value = GetDatabase().ListRightPop((RedisKey) key);
            T obj = CommonMethod.JsonDeserialize<T>((string) value);
            return obj;
        }

        public T ListLeftPop<T>(string key)
        {
            var value = GetDatabase().ListLeftPop((RedisKey) key);
            T obj = CommonMethod.JsonDeserialize<T>((string) value);
            return obj;
        }

        public long ListRightPush<T>(string key, T value)
        {
            return GetDatabase().ListRightPush((RedisKey)key, (RedisValue)CommonMethod.JsonSerialize(value));
        }

        public long ListLeftPush<T>(string key, T value)
        {
            return GetDatabase().ListLeftPush((RedisKey)key, (RedisValue)CommonMethod.JsonSerialize(value));
        }

        public bool HashSet<T>(string redisKey, string hashField, T value)
        {
            return GetDatabase().HashSet((RedisKey) redisKey, (RedisValue) hashField, (RedisValue) CommonMethod.JsonSerialize(value));
        }

        public long HashDelete(string redisKey, params string[] hashFields)
        {
            return GetDatabase().HashDelete((RedisKey)redisKey, hashFields.Select(e => (RedisValue)e).ToArray());
        }

        public List<T> HashGetAllValue<T>(string redisKey)
        {
            return GetDatabase().HashGetAll((RedisKey) redisKey).Select(e => CommonMethod.JsonDeserialize<T>(e.Value))
                .ToList();
        }

        public T HashGet<T>(string redisKey, string hashField)
        {
            var value = GetDatabase().HashGet((RedisKey) redisKey, (RedisValue) hashField);
            return CommonMethod.JsonDeserialize<T>((string)value);
        }

        public bool Exists(string key)
        {
            return GetDatabase().KeyExists((RedisKey) key);
        }

        public void RemoveRange(string[] keys)
        {
            GetDatabase().KeyDelete(keys.Select(e => (RedisKey) e).ToArray());
        }

        public async Task<long> RemoveRangeAsync(string[] keys)
        {
            var value = await GetDatabase().KeyDeleteAsync(keys.Select(e => (RedisKey) e).ToArray());
            return value;
        }

        public bool SetNx(string key, string value, TimeSpan expireTimeSpan = default(TimeSpan))
        {
            if (expireTimeSpan.TotalSeconds > 0.0)
            {
                return GetDatabase().StringSet((RedisKey)key, (RedisValue)value, expireTimeSpan, When.NotExists);
            }
            else
            {
                return GetDatabase().StringSet((RedisKey)key, (RedisValue)value, null, When.NotExists);
            }
        }

        

        public IServer GetServer(string host, int port)
        {
            return _connection.GetServer(host, port);
        }

        public long Publish(string channel, string message)
        {
            return _connection.GetSubscriber().Publish((RedisChannel)channel, (RedisValue)message);
        }

        public void Subscribe(string channelFrom)
        {
            _connection.GetSubscriber().Subscribe((RedisChannel)channelFrom, (Action<RedisChannel, RedisValue>)((channel, message) => Console.WriteLine((string)message)));
        }
    }
}
