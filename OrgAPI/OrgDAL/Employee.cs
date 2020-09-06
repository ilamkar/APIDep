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
        public int Empid { get; set; }

        public string Name { get; set; }
        public string Gender { get; set; }

        [ForeignKey("Department")]
        public int Did { get; set; }
        public Department Department { get; set; } //navigation property
        //user created by
        public string Id { get; set; }

    }
}
