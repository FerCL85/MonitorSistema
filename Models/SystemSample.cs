using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorSistema.Models
{
    public class SystemSample
    {
        public DateTime Timestamp { get; set; }
        public string CpuSerial { get; set; }
        public string MotherboardSerial { get; set; }
        public string GpuSerial { get; set; }
        public float CpuUsage { get; set; }
        public float RamUsage { get; set; }
    }    
}
