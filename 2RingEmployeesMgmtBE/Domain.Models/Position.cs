using Domain.Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Position : ITrackLastModified
    {
        [Key] public int Id { get; set; }
        [Required] [StringLength(50)] public string PositionName { get; set; }
        public DateTime LastModified { get; set; }
    }
}
