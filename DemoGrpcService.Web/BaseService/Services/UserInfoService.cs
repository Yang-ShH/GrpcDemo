using DemoGrpcService.Web.BaseService.Cache.Interface;
using DemoGrpcService.Web.BaseService.Interface;
using DemoGrpcService.Web.Protos;

namespace DemoGrpcService.Web.BaseService.Services
{
    public class UserInfoService : IUserInfo
    {
        private readonly IRedisHelper _redisHelper;

        public UserInfoService(IRedisHelper redisHelper)
        {
            _redisHelper = redisHelper;
        }

        public UserInfoResult GetUserInfo(UserInfoRequest userInfoRequest)
        {
            return new UserInfoResult
            {
                UserName = $"姓名：{userInfoRequest.UserName}",
                UserAge = userInfoRequest.UserAge,
                Address = "成都"
            };
        }

        public T GetRedis<T>(string redisKey)
        {
            return _redisHelper.Get<T>(redisKey);
        }

        public void SetRedis<T>(string redisKey, T value, TimeSpan timeSpan = default(TimeSpan))
        {
            _redisHelper.Set(redisKey, value, timeSpan);
        }
    }
}
