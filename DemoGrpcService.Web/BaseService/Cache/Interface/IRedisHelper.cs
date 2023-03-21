using StackExchange.Redis;

namespace DemoGrpcService.Web.BaseService.Cache.Interface
{
    public interface IRedisHelper
    {
        T Get<T>(string key);

        Task<T> GetAsync<T>(string key);

        void Set<T>(string key, T value, TimeSpan expireTimeSpan = default(TimeSpan));

        Task SetAsync<T>(string key, T value, TimeSpan expireTimeSpan = default(TimeSpan));

        bool Remove(string key);

        Task<bool> RemoveAsync(string key);

        Task ListLeftPushAsync<T>(string key, T value);

        Task ListRightPushAsync<T>(string key, T value);

        Task<T> ListLeftPopAsync<T>(string key);

        Task<T> ListRightPopAsync<T>(string key);

        T ListRightPop<T>(string key);

        T ListLeftPop<T>(string key);

        long ListRightPush<T>(string key, T value);

        long ListLeftPush<T>(string key, T value);

        bool HashSet<T>(string redisKey, string hashField, T value);

        long HashDelete(string redisKey, params string[] hashFields);

        List<T> HashGetAllValue<T>(string redisKey);

        T HashGet<T>(string redisKey, string hashField);

        bool Exists(string key);

        void RemoveRange(string[] keys);

        Task<long> RemoveRangeAsync(string[] keys);

        bool SetNx(string key, string value);

        IDatabase GetDatabase(int dbIndex = 0);

        IServer GetServer(string host, int port);

        long Publish(string channel, string message);

        void Subscribe(string channelFrom);
    }
}
