using DemoGrpcService.Web.BaseService.Interface;
using DemoGrpcService.Web.Entities;
using DemoGrpcService.Web.Protos;
using Microsoft.AspNetCore.Mvc;

namespace DemoGrpcService.Web.Controllers
{
    [Route("api/userinfo/[action]")]
    public class UserInfoController : ControllerBase
    {
        private readonly ILogger<UserInfoController> _logger;
        private readonly IUserInfo _userInfo;

        public UserInfoController(ILogger<UserInfoController> logger, IUserInfo userInfo)
        {
            _logger = logger;
            _userInfo = userInfo;
        }

        [HttpPost]
        [ActionName("get_user_info")]
        public Task<UserInfoResult> GetUserInfo([FromBody] UserInfoRequest request)
        {
            Console.WriteLine($"收到客户端调用GetUserInfo请求，请求name:{request.UserName},age:{request.UserAge}");
            return Task.FromResult(_userInfo.GetUserInfo(request));
        }

        [HttpGet]
        [ActionName("get_redis")]
        public Task<UserInfoRequest> GetRedis([FromQuery(Name = "user_name")] string userName)
        {
            Console.WriteLine($"收到客户端调用GetRedis请求，请求user_name:{userName}");
            return Task.FromResult(_userInfo.GetRedis<UserInfoRequest>(userName));
        }

        [HttpPost]
        [ActionName("set_redis")]
        public void SetRedis([FromBody] UserInfoRequest request)
        {
            Console.WriteLine($"收到客户端调用SetRedis请求，请求name:{request.UserName},age:{request.UserAge}");
            _userInfo.SetRedis(request.UserName, request);
        }

        [HttpGet]
        [ActionName("get_device")]
        public Task<Device> GetDevice([FromQuery(Name = "cu")] int cu)
        {
            Console.WriteLine($"收到客户端调用 GetDevice 请求，请求cu:{cu}");
            return Task.FromResult(_userInfo.GetDevice(cu));
        }
    }
}
