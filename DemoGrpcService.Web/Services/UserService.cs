
using DemoGrpcService.Web.Protos;
using Grpc.Core;

namespace DemoGrpcService.Web.Services
{
    public class UserService : User.UserBase
    {
        public override Task<UserInfoResult> GetUserInfo(UserInfoRequest request, ServerCallContext context)
        {
            Console.WriteLine($"收到客户端调用GetUserInfo请求，请求name:{request.UserName},age:{request.UserAge}");
            return Task.FromResult(new UserInfoResult
            {
                UserName = $"姓名：{request.UserName}",
                UserAge = request.UserAge,
                Address = "成都"
            });
        }
    }
}
