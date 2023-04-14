using SqlSugar;

namespace DemoGrpcService.Web.Entities
{
    [SugarTable("device")]
    public class Device
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public short Cu { get; set; }

        public string Type { get; set; }
        [SugarColumn(ColumnName = "pick_area_number")]
        public short PickAreaNumber { get; set; }
        public bool Occupied { get; set; }
        [SugarColumn(ColumnName = "with_bucket")]
        public bool WithBucket { get; set; }
        [SugarColumn(ColumnName = "is_master")]
        public bool IsMaster { get; set; }
        [SugarColumn(ColumnName = "use_agv")]
        public bool UseAgv { get; set; }
        [SugarColumn(ColumnName = "is_collective_filling")]
        public bool IsCollectiveFilling { get; set; }
        [SugarColumn(ColumnName = "is_retrieving_empty_box")]
        public bool IsRetrievingEmptyBox { get; set; }
        [SugarColumn(ColumnName = "is_default_move_device")]
        public bool IsDefaultMoveDevice { get; set; }
        [SugarColumn(ColumnName = "is_system_member")]
        public bool IsSystemMember { get; set; }
        [SugarColumn(ColumnName = "online")]
        public bool Online { get; set; }
        [SugarColumn(ColumnName = "hardware_version")]
        public string HardwareVersion { get; set; }
        [SugarColumn(ColumnName = "software_version")]
        public string SoftwareVersion { get; set; }
        [SugarColumn(ColumnName = "outdoor_agv_storing")]
        public bool OutdoorAgvStoring { get; set; }
        [SugarColumn(ColumnName = "update_time")]
        public DateTime UpdateTime { get; set; }
        [SugarColumn(ColumnName = "create_time")]
        public DateTime CreateTime { get; set; }
    }
}
