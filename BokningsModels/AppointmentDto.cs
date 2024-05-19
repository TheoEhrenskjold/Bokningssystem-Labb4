using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BokningsModels
{
    public class AppointmentDto
    {       
        
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public bool IsCancelled { get; set; }
            public int CustomerID { get; set; }
            public int CompanyID { get; set; }
        
    }
}
