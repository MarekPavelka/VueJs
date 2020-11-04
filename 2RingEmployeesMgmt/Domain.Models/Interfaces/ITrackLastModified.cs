using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Interfaces
{
    public interface ITrackLastModified
    {
        DateTime LastModified { get; set; }
    }
}
