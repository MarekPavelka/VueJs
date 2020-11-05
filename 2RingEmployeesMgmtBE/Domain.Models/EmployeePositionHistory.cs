using Domain.Models.Interfaces;
using System;

namespace Domain.Models
{
    public class EmployeePositionHistory : ITrackLastModified
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime LastModified { get; set; }

        public virtual Position Position { get; set; }
    }
}
