using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace _2RingEmployeesMgmt.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeePositionController : ControllerBase
    {
        private readonly IPositionService _positionService;
        private readonly IValidator<Position> _validator;

        public EmployeePositionController(IPositionService positionService, IValidator<Position> validator)
        {
            _positionService = positionService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IEnumerable<Position>> GetAllPositions()
        {
            var positions = await _positionService.GetAllPositions();
            return positions;
        }
        [HttpGet("{id}")]
        public async Task<Position> GetPosition(int id)
        {
            var position = await _positionService.GetPosition(id);
            return position;
        }

        [HttpPost]
        public async Task<IActionResult> AddNewPosition([FromBody] Position position)
        {
            var result = _validator.Validate(position);
            if (!result.IsValid)
                return BadRequest(result);

            await _positionService.AddNewPosition(position);
            return Created($"/EmployeePosition/{position.Id}", position);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var isSuccess = await _positionService.DeletePosition(new Position { Id = id });
            if (!isSuccess)
                return NotFound();
            return Ok();
        }
    }
}