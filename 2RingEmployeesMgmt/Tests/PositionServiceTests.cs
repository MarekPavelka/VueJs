using Domain.Models;
using Domain.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class PositionServiceTests : InMemoryDbTest
    {
        private readonly PositionService _testObject;

        public PositionServiceTests()
        {
            _testObject = new PositionService(DbContext);
        }

        [Fact]
        public async Task GivenPositionDto_WhenAddNewPositionCalled_ThenPositionSavedToDb()
        {
            var position = new Position { Id = 1, PositionName = "Tester" };

            await _testObject.AddNewPosition(position);

            DbContext.Positions.Should().ContainSingle().Which.Should().Be(position);
        }

        [Fact]
        public async Task WhenDeletePositionCalled_ThenPositionDeletedFromDb()
        {
            var position = new Position { Id = 1, PositionName = "Tester" };
            DbContext.Positions.Add(position);
            DbContext.SaveChanges();
            DetachAllEntites();

            await _testObject.DeletePosition(position);

            DbContext.Positions.Should().BeEmpty();
        }

        [Fact]
        public async Task WhenGetAllPositionsCalled_ThenPositionDeletedFromDb()
        {
            var positions = GetValidPositions();
            positions.ForEach(p => DbContext.Positions.Add(p));
            DbContext.SaveChanges();
            DetachAllEntites();

            var allPositions = await _testObject.GetAllPositions();

            allPositions.Should().HaveCount(3);
        }

        [Fact]
        public async Task WhenGetPositionCalledWithIdProvided_ThenRetrievePositionWithSameIdFromDB()
        {
            var positions = GetValidPositions();
            positions.ForEach(p => DbContext.Positions.Add(p));
            DbContext.SaveChanges();
            DetachAllEntites();

            var retrievedPosition = await _testObject.GetPosition(2);

            retrievedPosition.PositionName.Should().Be("Salesman");
        }

        private List<Position> GetValidPositions()
        {
            var position1 = new Position { Id = 1, PositionName = "Tester" };
            var position2 = new Position { Id = 2, PositionName = "Salesman" };
            var position3 = new Position { Id = 3, PositionName = "Analytist" };

            return new List<Position> { position1, position2, position3 };
        }
    }
}
