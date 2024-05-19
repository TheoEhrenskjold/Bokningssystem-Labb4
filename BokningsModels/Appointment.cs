using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BokningsModels
{
    public class Appointment
    {
        [Key]
        public int AppointmentID {get;set;}

       

        [Required]
        public DateTime StartTime { get;set;}

        [Required]
        public DateTime EndTime { get;set;}

        
        public bool IsCancelled { get;set;} = false;

        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        public int CompanyID { get; set; }
        public Company Company { get; set; }


    }
}
