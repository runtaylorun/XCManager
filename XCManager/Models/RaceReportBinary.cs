using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XCManager.Models
{
    public class RaceReportBinary
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public char ContentType { get; set; }
        public byte[] Data { get; set; }
        public int RaceId { get; set; }
    }
}