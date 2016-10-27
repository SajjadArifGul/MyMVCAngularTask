using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTask.Models
{
    public class Customer
    {
        [Key]
        public int Customer_Id_Pk { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Details { get; set; }
        public string Customer_Address { get; set; }
        public string Customer_Contact { get; set; }
        public Nullable<int> Customer_Key_Account_Fk { get; set; }
        public int Created_By { get; set; }
        public System.DateTime Created_On { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public bool Is_Active { get; set; }

        public virtual ICollection<CustomerNumber> CustomerNumbers { get; set; }
    }
}
