using DemoGrpcService.Web.Protos;

namespace DemoGrpcService.Web.BaseService.Interface
{
    public interface IUserInfo
    {
        UserInfoResult GetUserInfo(UserInfoRequest userInfoRequest);
    }
}
