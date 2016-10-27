using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyTask.WebUI.ViewModels
{
    public class CustomerViewModel
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    }
}