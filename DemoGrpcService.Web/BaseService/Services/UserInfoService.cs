using DemoGrpcService.Web.BaseService.Interface;
using DemoGrpcService.Web.Protos;

namespace DemoGrpcService.Web.BaseService.Services
{
    public class UserInfoService : IUserInfo
    {
        public UserInfoResult GetUserInfo(UserInfoRequest userInfoRequest)
        {
            return new UserInfoResult
            {
                UserName = $"姓名：{userInfoRequest.UserName}",
                UserAge = userInfoRequest.UserAge,
                Address = "成都"
            };
        }
    }
}
