using System.Diagnostics;
using DemoGrpcService.Web.BaseService.Cache.Interface;
using DemoGrpcService.Web.BaseService.Interface;
using DemoGrpcService.Web.Entities;
using DemoGrpcService.Web.Protos;
using SqlSugar;

namespace DemoGrpcService.Web.BaseService.Services
{
    public class UserInfoService : IUserInfo
    {
        private readonly IRedisHelper _redisHelper;
        private readonly ISqlSugarClient _dbContext;
        public UserInfoService(IRedisHelper redisHelper,
            ISqlSugarClient dbContext)
        {
            _redisHelper = redisHelper;
            _dbContext = dbContext;
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

        public Device GetDevice(int cu)
        {
            var timer = new Stopwatch();
            timer.Start();
            var device = _dbContext.Queryable<Device>().First(e => e.Cu == cu);
            timer.Stop();
            Console.WriteLine($"依据cu查询查询耗时：{timer.Elapsed.TotalMilliseconds} 毫秒");
            timer.Restart();
            var temp = _dbContext.Queryable<Device>().InSingle(12);
            timer.Stop();
            Console.WriteLine($"依据主键id查询查询耗时：{timer.Elapsed.TotalMilliseconds} 毫秒");
            return device;
        }
    }
}
