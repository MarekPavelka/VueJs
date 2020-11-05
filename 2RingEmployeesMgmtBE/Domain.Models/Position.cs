using Domain.Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Position : ITrackLastModified
    {
        public int Id { get; set; }
        public string PositionName { get; set; }
        public DateTime LastModified { get; set; }
    }
}
