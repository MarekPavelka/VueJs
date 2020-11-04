using Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Employee : ITrackLastModified
    {
        [Key] public int Id { get; set; }
        [Required] [StringLength(20)] public string FirstName { get; set; }
        [Required] [StringLength(20)] public string Surname { get; set; }
        [StringLength(50)] public string Address { get; set; }
        [Required] public DateTime Birthday { get; set; }
        [Required] public DateTime StartDate { get; set; }
        [Required] public float Salary { get; set; }
        [Required] public bool IsArchived { get; set; }
        public DateTime LastModified { get; set; }

        public virtual Position Position { get; set; }
        public virtual ICollection<EmployeePositionHistory> PositionHistory { get; set; } = new HashSet<EmployeePositionHistory>();
    }
}