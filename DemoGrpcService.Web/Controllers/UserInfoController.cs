using DemoGrpcService.Web.BaseService.Interface;
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
        //[ActionName("getuserinfo")]
        public Task<UserInfoResult> GetUserInfo()
        {
            Console.WriteLine($"收到客户端调用GetUserInfo请求，请求name:1,age:2");
            return Task.FromResult(new UserInfoResult
            {
                UserName = $"姓名：",
                UserAge = 1,
                Address = "成都"
            });
        }
    }
}
