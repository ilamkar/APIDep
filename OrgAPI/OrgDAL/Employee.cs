using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrgDAL
{
    [Table("Employee")]
    public class Employee

    {
        [Key]
        public int empid { get; set; }

        public string fname { get; set; }
        public string lname { get; set; }
        public string position { get; set; }

    }
}
