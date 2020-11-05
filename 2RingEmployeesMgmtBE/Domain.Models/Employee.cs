using Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Employee : ITrackLastModified
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime StartDate { get; set; }
        public float Salary { get; set; }
        public bool IsArchived { get; set; }
        public DateTime LastModified { get; set; }

        public virtual Position Position { get; set; }
        public virtual ICollection<EmployeePositionHistory> PositionHistory { get; set; } = new HashSet<EmployeePositionHistory>();
    }
}