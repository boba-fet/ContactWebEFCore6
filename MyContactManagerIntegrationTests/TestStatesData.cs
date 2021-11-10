using ContactWebModels;
using Microsoft.EntityFrameworkCore;
using MyContactManagerData;
using MyContactManagerRepositories;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MyContactManagerIntegrationTests
{
    public class TestStatesData
    {
        DbContextOptions<MyContactManagerDbContext> _options;
        private IStatesRepository _repository;
        private const int NUMBER_OF_STATES = 20;
        private const string IOWA_SPELLING = "Iowa";
        private const string IOWA_MISSPELLING = "Iwoa";

        public TestStatesData()
        {
            SetupOptions();
            BuildDefaults();
        }

        private void SetupOptions()
        {
            _options = new DbContextOptionsBuilder<MyContactManagerDbContext>()
                            .UseInMemoryDatabase(databaseName: "MyContactManagerStatesTests")
                            .Options;
        }

        public void BuildDefaults()
        {

            using (var context = new MyContactManagerDbContext(_options))
            {
                var existingStates = Task.Run(() => context.States.ToListAsync()).Result;

                if (existingStates is null || existingStates.Count < 15)
                {
                    var states = GetStatesTestData();
                    context.States.AddRange(states);
                    context.SaveChanges();
                }

                //could do more here to ensure starting state of tests...
            }
        }

        private List<State> GetStatesTestData()
        {
            return new List<State>() {
                new State() { Id = 1, Name = "Alabama", Abbreviation = "AL" },
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
                new State() { Id = 16, Name = "Iwoa", Abbreviation = "IA" },
                new State() { Id = 17, Name = "Kansas", Abbreviation = "KS" },
                new State() { Id = 18, Name = "Kentucky", Abbreviation = "KY" },
                new State() { Id = 19, Name = "Louisiana", Abbreviation = "LA" },
                new State() { Id = 20, Name = "Maine", Abbreviation = "ME" }
            };
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
            using (var context = new MyContactManagerDbContext(_options))
            {
                _repository = new StatesRepository(context);

                var states = await _repository.GetAllAsync();
                states.Count.ShouldBe(NUMBER_OF_STATES);
                states[index].Name.ShouldBe(name, StringCompareShould.IgnoreCase);
                states[index].Abbreviation.ShouldBe(abbreviation, StringCompareShould.IgnoreCase);
            }
        }

        [Theory]
        [InlineData("Alabama", "AL", 1)]
        [InlineData("Arizona", "AZ", 3)]
        [InlineData("California", "CA", 5)]
        [InlineData("Connecticut", "CT", 7)]
        [InlineData("District of Columbia", "DC", 9)]
        [InlineData("Georgia", "GA", 11)]
        [InlineData("Idaho", "ID", 13)]
        [InlineData("Indiana", "IN", 15)]
        [InlineData("Kansas", "KS", 17)]
        [InlineData("Louisiana", "LA", 19)]
        [InlineData("Maine", "ME", 20)]
        public async Task GetState(string expectedName, string expectedAbbreviation, int stateId)
        {
            using (var context = new MyContactManagerDbContext(_options))
            {
                _repository = new StatesRepository(context);

                var state = await _repository.GetAsync(stateId);
                state.ShouldNotBe(null);
                state.Name.ShouldBe(expectedName);
                state.Abbreviation.ShouldBe(expectedAbbreviation);
            }
        }

        [Fact]
        public async Task UpdateState()
        {
            using (var context = new MyContactManagerDbContext(_options))
            {
                _repository = new StatesRepository(context);

                var stateToUpdate = await _repository.GetAsync(16); //expected IWOA -> purposeful typo
                stateToUpdate.ShouldNotBeNull();
                stateToUpdate.Name.ShouldBe(IOWA_MISSPELLING, StringCompareShould.IgnoreCase);

                stateToUpdate.Name = IOWA_SPELLING;
                await _repository.AddOrUpdateAsync(stateToUpdate);

                var updatedState = await _repository.GetAsync(16); //expected IWOA -> purposeful typo
                updatedState.ShouldNotBeNull();
                updatedState.Name.ShouldBe(IOWA_SPELLING, StringCompareShould.IgnoreCase);

                //put it back:
                updatedState.Name = "iwoa";
                await _repository.AddOrUpdateAsync(updatedState);

                var revertedState = await _repository.GetAsync(16); //expected IWOA -> purposeful typo
                revertedState.ShouldNotBeNull();
                revertedState.Name.ShouldBe(IOWA_MISSPELLING, StringCompareShould.IgnoreCase);
            }
        }

        [Fact]
        public async Task AddAndDeleteState()
        {
            using (var context = new MyContactManagerDbContext(_options))
            {
                _repository = new StatesRepository(context);

                //add the state and validate it is stored
                var stateToAdd = new State() { Id = 0, Name = "Star Trek - The Next Generation", Abbreviation = "TNG" };
                await _repository.AddOrUpdateAsync(stateToAdd);

                var updatedState = await _repository.GetAsync(stateToAdd.Id);
                updatedState.ShouldNotBeNull();
                updatedState.Name.ShouldBe("Star Trek - The Next Generation", StringCompareShould.IgnoreCase);
                updatedState.Abbreviation.ShouldBe("TNG", StringCompareShould.IgnoreCase);

                //delete to keep current count and list in tact.
                await _repository.DeleteAsync(updatedState.Id);
                var deletedState = await _repository.GetAsync(updatedState.Id);
                deletedState.ShouldBeNull();
            }
        }
    }
}