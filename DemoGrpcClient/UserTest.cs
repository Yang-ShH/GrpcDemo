using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoGrpcService.Protos;
using Grpc.Net.Client;

namespace DemoGrpcClient
{
    public class UserTest
    {
        public void GetUserInfo()
        {
            var url = "https://localhost:5134";
            using (var channel = GrpcChannel.ForAddress(url))
            {
                var client = new User.UserClient(channel);
                var userInfo = client.GetUserInfo(new UserInfoRequest
                {
                    UserName = "young",
                    UserAge = 24
                });

                Console.WriteLine($"{userInfo.UserName},{userInfo.UserAge},{userInfo.Address}");
            }

            Console.ReadKey();
        }
    }
}
