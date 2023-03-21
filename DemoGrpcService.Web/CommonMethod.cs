using Newtonsoft.Json;

namespace DemoGrpcService.Web
{
    public class CommonMethod
    {
        public static T? JsonDeserialize<T>(string? param)
        {
            return string.IsNullOrEmpty(param) ? default : JsonConvert.DeserializeObject<T>(param);
        }

        public static string? JsonSerialize(object? o)
        {
            return o == null ? null : JsonConvert.SerializeObject(o);
        }
    }
}
