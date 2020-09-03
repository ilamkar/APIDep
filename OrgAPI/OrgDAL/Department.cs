using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace OrgDAL
{
    [Table("Department")]
    public class Department
    {
      [System.ComponentModel.DataAnnotations.Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Did { get; set; }
        public string Dname { get; set; }
        public string Description { get; set; }

    }
}
