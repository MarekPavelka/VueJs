using Domain.Models;
using Domain.Services.Interfaces;
using Domain.Services.Validation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class PositionService : IPositionService
    {
        private readonly IUnitOfWork _dbContext;

        public PositionService(IUnitOfWork dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddNewPosition(Position position)
        {
            _dbContext.Add(position);
            return _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeletePosition(Position position)
        {
            _dbContext.Delete(position);
            var affectedRows = await _dbContext.SaveChangesAsync();
            return affectedRows != 0;
        }

        public async Task<IReadOnlyCollection<Position>> GetAllPositions()
        {
            var positions = await _dbContext.Query<Position>().ToListAsync();
            return positions;
        }

        public async Task<Position> GetPosition(int id)
        {
            var position = await _dbContext.Query<Position>().Where(p => p.Id == id).FirstOrDefaultAsync();
            return position;
        }
    }
}