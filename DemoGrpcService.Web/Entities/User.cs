using SqlSugar;

namespace DemoGrpcService.Web.Entities
{
    [SugarTable("user")]
    public class User
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "id")]
        public int Id { get; set; }
        [SugarColumn(ColumnName = "name")]
        public string Name { get; set; }
        [SugarColumn(ColumnName = "age")]
        public short Age { get; set; }
        [SugarColumn(ColumnName = "address")]
        public string Address { get; set; }
    }
}
