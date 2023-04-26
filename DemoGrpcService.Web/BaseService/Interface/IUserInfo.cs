using DemoGrpcService.Web.Entities;
using DemoGrpcService.Web.Protos;

namespace DemoGrpcService.Web.BaseService.Interface
{
    public interface IUserInfo
    {
        UserInfoResult GetUserInfo(UserInfoRequest userInfoRequest);
        T GetRedis<T>(string redisKey);
        void SetRedis<T>(string redisKey, T value, TimeSpan timeSpan = default(TimeSpan));
        Entities.User GetUser(string name);
        int InsertUser(Entities.User user);
        int UpdateUserAge(string name, short age);
        Device GetDevice(int cu);
    }
}
