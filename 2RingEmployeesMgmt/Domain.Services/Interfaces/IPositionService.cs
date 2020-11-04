using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Services.Interfaces
{
    public interface IPositionService
    {
        Task AddNewPosition(Position position);
        Task<bool> DeletePosition(Position position);
        Task<IReadOnlyCollection<Position>> GetAllPositions();
        Task<Position> GetPosition(int id);
    }
}
