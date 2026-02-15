using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFirstMCP.Models
{
    [Table("EMPLOYEE", Schema = "dbo")]
    public class EMPLOYEE
    {
        [Key]
        [Column("EMPNO", TypeName = "numeric(4,0)")]
        public int EmpNo { get; set; }

        [Column("ENAME", TypeName = "varchar(10)")]
        [MaxLength(10)]
        public string? EName { get; set; }

        [Column("JOB", TypeName = "varchar(9)")]
        [MaxLength(9)]
        public string? Job { get; set; }

        [Column("MGR", TypeName = "numeric(4,0)")]
        public int? Mgr { get; set; }

        [Column("HIREDATE", TypeName = "datetime")]
        public DateTime? HireDate { get; set; }

        [Column("SAL", TypeName = "numeric(7,2)")]
        public decimal? Sal { get; set; }

        [Column("COMM", TypeName = "numeric(7,2)")]
        public decimal? Comm { get; set; }

        [Column("DEPTNO", TypeName = "numeric(2,0)")]
        public int? DeptNo { get; set; }

    }
}
