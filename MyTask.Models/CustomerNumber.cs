using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTask.Models
{
    public class CustomerNumber
    {
        [Key]
        public int Customer_Number_Id_Pk { get; set; }
        public int Customer_Id_Fk { get; set; }
        public string Customer_Number_Value { get; set; }
        public string Customer_Number_Details { get; set; }
        
        //changed from Int to String since i want to pass the current loggedin userId here which is by default in string
        public string Created_By { get; set; }
        public System.DateTime Created_On { get; set; }

        //changed from Int to String since i want to pass the current loggedin userId here which is by default in string
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public bool Is_Active { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
