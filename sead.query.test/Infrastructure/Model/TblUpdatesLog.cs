using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblUpdatesLog
    {
        public int UpdatesLogId { get; set; }
        public string TableName { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
