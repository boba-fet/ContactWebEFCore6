using ContactWebModels;
using Moq;
using MyContactManagerRepositories;
using MyContactManagerServices;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MyContactManagerUnitTests
{
    public class TestStatesService
    {
        private IStatesService _statesService;
        private Mock<IStatesRepository> _repository;
        private int NUMBER_OF_STATES = 20;

        public TestStatesService()
        {
            CreateMocks();
            _statesService = new StatesService(_repository.Object);
        }

        private void CreateMocks()
        {
            _repository = new Mock<IStatesRepository>();
            var states = GetStatesTestData();
            var singleState = GetSingleState();

            _repository.Setup(x => x.GetAllAsync()).Returns(states);
            _repository.Setup(x => x.GetAsync(It.IsAny<int>())).Returns(singleState);
        }

        private async Task<IList<State>> GetStatesTestData()
        {
            return new List<State>()
            {
                new State() { Id = 1, Name = "Alabama", Abbreviation = "AL"},
                new State() { Id = 2, Name = "Alaska", Abbreviation = "AK" },
                new State() { Id = 3, Name = "Arizona", Abbreviation = "AZ" },
                new State() { Id = 4, Name = "Arkansas", Abbreviation = "AR" },
                new State() { Id = 5, Name = "California", Abbreviation = "CA" },
                new State() { Id = 6, Name = "Colorado", Abbreviation = "CO" },
                new State() { Id = 7, Name = "Connecticut", Abbreviation = "CT" },
                new State() { Id = 8, Name = "Delaware", Abbreviation = "DE" },
                new State() { Id = 9, Name = "District of Columbia", Abbreviation = "DC" },
                new State() { Id = 10, Name = "Florida", Abbreviation = "FL" },
                new State() { Id = 11, Name = "Georgia", Abbreviation = "GA" },
                new State() { Id = 12, Name = "Hawaii", Abbreviation = "HI" },
                new State() { Id = 13, Name = "Idaho", Abbreviation = "ID" },
                new State() { Id = 14, Name = "Illinois", Abbreviation = "IL" },
                new State() { Id = 15, Name = "Indiana", Abbreviation = "IN" },
                new State() { Id = 16, Name = "Iowa", Abbreviation = "IA" },
                new State() { Id = 17, Name = "Kansas", Abbreviation = "KS" },
                new State() { Id = 18, Name = "Kentucky", Abbreviation = "KY" },
                new State() { Id = 19, Name = "Louisiana", Abbreviation = "LA" },
                new State() { Id = 20, Name = "Maine", Abbreviation = "ME" }
            };
        }

        private async Task<State> GetSingleState()
        {
            return new State() { Id = 2, Name = "Alaska", Abbreviation = "AK" };
        }

        [Theory]
        [InlineData("Alabama", "AL", 0)]
        [InlineData("Arizona", "AZ", 2)]
        [InlineData("California", "CA", 4)]
        [InlineData("Connecticut", "CT", 6)]
        [InlineData("District of Columbia", "DC", 8)]
        [InlineData("Georgia", "GA", 10)]
        [InlineData("Idaho", "ID", 12)]
        [InlineData("Indiana", "IN", 14)]
        [InlineData("Kansas", "KS", 16)]
        [InlineData("Louisiana", "LA", 18)]
        [InlineData("Maine", "ME", 19)]
        public async Task TestGetAllStates(string name, string abbreviation, int index)
        {
            var states = await _statesService.GetAllAsync();
            states.Count.ShouldBe(NUMBER_OF_STATES);
            states[index].Name.ShouldBe(name, StringCompareShould.IgnoreCase);
            states[index].Abbreviation.ShouldBe(abbreviation, StringCompareShould.IgnoreCase);
        }

        [Fact]
        public async Task TestGetAState()
        {
            var state = await _statesService.GetAsync(12);
            state.ShouldNotBeNull();
            state.Name.ShouldBe("Alaska", StringCompareShould.IgnoreCase);
            state.Abbreviation.ShouldBe("AK", StringCompareShould.IgnoreCase);
        }
    }
}