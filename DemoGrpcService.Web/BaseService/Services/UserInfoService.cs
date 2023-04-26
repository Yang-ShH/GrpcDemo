using System.Diagnostics;
using DemoGrpcService.Web.BaseService.Cache.Interface;
using DemoGrpcService.Web.BaseService.Interface;
using DemoGrpcService.Web.Entities;
using DemoGrpcService.Web.Protos;
using SqlSugar;
using User = DemoGrpcService.Web.Entities.User;

namespace DemoGrpcService.Web.BaseService.Services
{
    public class UserInfoService : IUserInfo
    {
        private readonly IRedisHelper _redisHelper;
        private readonly ISqlSugarClient _dbContext;
        private readonly ILogger<UserInfoService> _logger;
        public UserInfoService(IRedisHelper redisHelper,
            ISqlSugarClient dbContext,
            ILogger<UserInfoService> logger)
        {
            _redisHelper = redisHelper;
            _dbContext = dbContext;
            _logger = logger;
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

        public User GetUser(string name)
        {
            var user = _dbContext.Queryable<User>().First(x => x.Name == name);
            return user;
        }

        public int InsertUser(User user)
        {
            var result = _dbContext.Insertable<User>(user).ExecuteCommand();
            return result;
        }

        public int UpdateUserAge(string name, short age)
        {
            var user = _dbContext.Queryable<User>().First(x => x.Name == name);
            user.Age = age;
            var result = _dbContext.Updateable<User>(user).ExecuteCommand();
            return result;
        }

        public Device GetDevice(int cu)
        {
            var timer = new Stopwatch();
            timer.Start();
            var device = _dbContext.Queryable<Device>().First(e => e.Cu == cu);
            timer.Stop();
            _logger.LogInformation($"依据cu查询查询耗时：{timer.Elapsed.TotalMilliseconds} 毫秒");
            timer.Restart();
            var temp = _dbContext.Queryable<Device>().InSingle(12);
            timer.Stop();
            _logger.LogInformation($"依据主键id查询查询耗时：{timer.Elapsed.TotalMilliseconds} 毫秒");
            return device;
        }
    }
}
