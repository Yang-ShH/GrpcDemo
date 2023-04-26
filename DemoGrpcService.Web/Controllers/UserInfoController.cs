using DemoGrpcService.Web.BaseService.Interface;
using DemoGrpcService.Web.Entities;
using DemoGrpcService.Web.Protos;
using Microsoft.AspNetCore.Mvc;

namespace DemoGrpcService.Web.Controllers
{
    /// <summary>
    /// demo controller
    /// </summary>
    [Route("api/userinfo/[action]")]
    public class UserInfoController : ControllerBase
    {
        private readonly ILogger<UserInfoController> _logger;
        private readonly IUserInfo _userInfo;
        /// <summary>
        /// controller 构造函数
        /// </summary>
        /// <param name="logger">日志</param>
        /// <param name="userInfo">userInfo服务接口</param>
        public UserInfoController(ILogger<UserInfoController> logger, IUserInfo userInfo)
        {
            _logger = logger;
            _userInfo = userInfo;
        }
        /// <summary>
        /// post请求获取user info，没有任何操作直接返回结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("get_user_info")]
        public Task<UserInfoResult> GetUserInfo([FromBody] UserInfoRequest request)
        {
            _logger.LogTrace($"收到客户端调用GetUserInfo请求，请求name:{request.UserName},age:{request.UserAge}");
            return Task.FromResult(_userInfo.GetUserInfo(request));
        }
        /// <summary>
        /// 测试从redis中读取
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("get_redis")]
        public Task<UserInfoRequest> GetRedis([FromQuery(Name = "user_name")] string userName)
        {
            _logger.LogTrace($"收到客户端调用GetRedis请求，请求user_name:{userName}");
            return Task.FromResult(_userInfo.GetRedis<UserInfoRequest>(userName));
        }
        /// <summary>
        /// 测试写入redis
        /// </summary>
        /// <param name="request"></param>
        [HttpPost]
        [ActionName("set_redis")]
        public void SetRedis([FromBody] UserInfoRequest request)
        {
            _logger.LogTrace($"收到客户端调用SetRedis请求，请求name:{request.UserName},age:{request.UserAge}");
            _userInfo.SetRedis(request.UserName, request);
        }
        /// <summary>
        /// 测试数据库读取
        /// </summary>
        /// <param name="cu">device表cu号</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("get_device")]
        public Task<Device> GetDevice([FromQuery(Name = "cu")] int cu)
        {
            _logger.LogTrace($"收到客户端调用 GetDevice 请求，请求cu:{cu}");
            return Task.FromResult(_userInfo.GetDevice(cu));
        }
        /// <summary>
        /// 从数据库获取user
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("get_user")]
        public Task<Entities.User> GetUser([FromQuery(Name = "name")] string name)
        {
            return Task.FromResult(_userInfo.GetUser(name));
        }

        /// <summary>
        /// 插入user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("insert_user")]
        public Task<int> InsertUser([FromBody] Entities.User user)
        {
            return Task.FromResult(_userInfo.InsertUser(user));
        }

        /// <summary>
        /// 更新user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("update_user")]
        public Task<int> UpdateUser([FromBody] Entities.User user)
        {
            return Task.FromResult(_userInfo.UpdateUserAge(user.Name, user.Age));
        }
    }
}
