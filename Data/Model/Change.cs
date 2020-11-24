using System;

namespace ListaccFinance.API.Data.Model
{
    public class Change
    {
        public int Id { get; set; }
        public string Table { get; set; }
        public int? EntryId { get; set; }
        public string ChangeType { get; set; }
        public DateTime OfflineTimeStamp { get; set; }
        public int? UserId { get; set; }
        public DateTime OnlineTimeStamp { get; set; }
        public int? DesktopClientId { get; set; }
    }
}
