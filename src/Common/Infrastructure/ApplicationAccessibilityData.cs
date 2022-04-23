using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure
{
    public class ApplicationAccessibilityData
    {
        public DateTime LastIdleTime { get; set; }
        public string ApplicationInstanceID { get; set; }
        public string ApplicationName { get; set; }
    }
    

}
