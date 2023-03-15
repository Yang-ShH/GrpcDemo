using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoGrpcService.Protos;

namespace DemoGrpcClient
{
    public class UserTestIoc
    {
        private readonly User.UserClient _userClient;

        public UserTestIoc(User.UserClient userClient)
        {
            _userClient = userClient;
        }

        public void GetUserInfo()
        {
            var userInfo = _userClient.GetUserInfo(new UserInfoRequest
            {
                UserName = "young - IOC",
                UserAge = 24
            });

            Console.WriteLine($"{userInfo.UserName},{userInfo.UserAge},{userInfo.Address}");

            Console.ReadKey();
        }
    }
}
