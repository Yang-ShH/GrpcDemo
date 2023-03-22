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
            //{
            //    // test
            //    var hashV = _redisHelper.HashGet<T>("HashKey", "HashField");
            //    var listRightPop = _redisHelper.ListRightPop<string>("ListKey");
            //}
            return _redisHelper.Get<T>(redisKey);
        }

        public void SetRedis<T>(string redisKey, T value, TimeSpan timeSpan = default(TimeSpan))
        {
            //{
            //    // test
            //    _redisHelper.HashSet("HashKey", "HashField", value);
            //    _redisHelper.ListLeftPush("ListKey", "left push 1");
            //    _redisHelper.SetNx("SetNxKey", "value", TimeSpan.FromSeconds(60));
            //}
            _redisHelper.Set(redisKey, value, timeSpan);
        }
    }
}
