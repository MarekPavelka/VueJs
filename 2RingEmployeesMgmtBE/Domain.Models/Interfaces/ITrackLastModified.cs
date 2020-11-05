using System;

namespace Domain.Models.Interfaces
{
    public interface ITrackLastModified
    {
        DateTime LastModified { get; set; }
    }
}
