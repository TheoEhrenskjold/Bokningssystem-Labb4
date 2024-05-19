using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BokningsModels
{
    public class AppHistory
    {
        [Key]
        public int HistoryId { get; set; }
        public int AppointmentID { get; set; }        
        public DateTime ChangeDate { get; set; }
        public int UserId { get; set; }
    }
}
