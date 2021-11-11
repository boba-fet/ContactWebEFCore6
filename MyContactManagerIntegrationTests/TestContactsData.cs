using Microsoft.EntityFrameworkCore;
using MyContactManagerData;
using ContactWebModels;
using MyContactManagerRepositories;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MyContactManagerIntegrationTests
{
    public class TestContactsData
    {
        DbContextOptions<MyContactManagerDbContext> _options;
        private IContactsRepository _repository;

        private const int NUMBER_OF_CONTACTS = 5;
        private const string USERID1 = "477f8f3a-c343-40b5-b34a-7b8977e1a65d";
        private const string USERID2 = "f4a2d35d-4a49-4a12-8445-752857124620";
        private const string USERID3 = "6d597b0e-b259-4127-b7e2-02537a754c75";
        private const string FIRST_NAME_1 = "John";
        private const string LAST_NAME_1 = "Smith";
        private const string EMAIL_1 = "john.smith@example.com";
        private const string FIRST_NAME_2 = "Jane";
        private const string LAST_NAME_2 = "Smith";
        private const string EMAIL_2 = "jane.smith@example.com";
        private const string FIRST_NAME_3 = "Tim";
        private const string LAST_NAME_3 = "Thomas";
        private const string EMAIL_3 = "tim.thomas@example.com";
        private const string FIRST_NAME_4 = "Amanda";
        private const string LAST_NAME_4 = "Thomas";
        private const string EMAIL_4 = "amanda.thomas@example.com";
        private const string FIRST_NAME_5 = "James";
        private const string LAST_NAME_5 = "Davis";
        private const string EMAIL_5 = "james.davis@example.com";
        private const string FIRST_NAME_2_UPDATED = "Janet";

        public TestContactsData()
        {
            SetupOptions();
            BuildDefaults();
        }

        private void SetupOptions()
        {
            _options = new DbContextOptionsBuilder<MyContactManagerDbContext>()
                            .UseInMemoryDatabase(databaseName: "MyContactManagerContactsTests")
                            .Options;
        }

        public void BuildDefaults()
        {
            using (var context = new MyContactManagerDbContext(_options))
            {
                var existingStates = Task.Run(() => context.States.ToListAsync()).Result;

                if (existingStates is null || existingStates.Count < 5)
                {
                    var states = GetStatesTestData();
                    context.States.AddRange(states);
                    context.SaveChanges();
                }

                var existingContacts = Task.Run(() => context.Contacts.ToListAsync()).Result;

                if (existingContacts is null || existingContacts.Count < 5)
                {
                    var contacts = GetContactsTestData();
                    context.Contacts.AddRange(contacts);
                    context.SaveChanges();
                }
            }
        }

        private List<State> GetStatesTestData()
        {
            return new List<State>() {
                new State() { Id = 1, Name = "Alabama", Abbreviation = "AL" },
                new State() { Id = 2, Name = "Alaska", Abbreviation = "AK" },
                new State() { Id = 3, Name = "Arizona", Abbreviation = "AZ" },
                new State() { Id = 4, Name = "Arkansas", Abbreviation = "AR" },
                new State() { Id = 5, Name = "California", Abbreviation = "CA" }
            };
        }

        private List<Contact> GetContactsTestData()
        {
            return new List<Contact>() {
                new Contact() { Id = 1, Birthday = new DateTime(1947, 1, 1), City = "Los Angeles", Email=EMAIL_1, FirstName = FIRST_NAME_1
                                , LastName = LAST_NAME_1, PhonePrimary = "555-555-1111", PhoneSecondary = "555-555-1111", StateId= 5
                                , StreetAddress1 = "111 First St", StreetAddress2 = "Lot 11", UserId = USERID1, Zip="11111" },
                new Contact() { Id = 2, Birthday = new DateTime(1972, 11, 3), City = "Little Rock", Email=EMAIL_2, FirstName = FIRST_NAME_2
                                , LastName = LAST_NAME_2, PhonePrimary = "555-555-2222", PhoneSecondary = "555-555-2222", StateId= 4
                                , StreetAddress1 = "222 Second St", StreetAddress2 = "Apt 22", UserId = USERID1, Zip="22222" },
                new Contact() { Id = 3, Birthday = new DateTime(2001, 2, 4), City = "Phoenix", Email=EMAIL_3, FirstName = FIRST_NAME_3
                                , LastName = LAST_NAME_3, PhonePrimary = "555-555-3333", PhoneSecondary = "555-555-3333", StateId= 3
                                , StreetAddress1 = "333 Third St", StreetAddress2 = "STE 33", UserId = USERID2, Zip="33333" },
                new Contact() { Id = 4, Birthday = new DateTime(1982, 7, 23), City = "Fairbanks", Email=EMAIL_4, FirstName = FIRST_NAME_4
                                , LastName = LAST_NAME_4, PhonePrimary = "555-555-4444", PhoneSecondary = "555-555-4444", StateId= 2
                                , StreetAddress1 = "444 Fourth St", StreetAddress2 = "UNIT 44", UserId = USERID2, Zip="44444" },
                new Contact() { Id = 5, Birthday = new DateTime(1937, 8, 12), City = "Birmingham", Email=EMAIL_5, FirstName = FIRST_NAME_5
                                , LastName = LAST_NAME_5, PhonePrimary = "555-555-5555", PhoneSecondary = "555-555-5555", StateId= 1
                                , StreetAddress1 = "555 Fifth St", StreetAddress2 = "BOX 55", UserId = USERID3, Zip="55555" }
            };
        }

        //reminder: The repo sends them back in alphabetical order
        [Theory]
        [InlineData(FIRST_NAME_1, LAST_NAME_1, EMAIL_1, USERID1, 1, 2)]
        [InlineData(FIRST_NAME_2, LAST_NAME_2, EMAIL_2, USERID1, 0, 2)]
        [InlineData(FIRST_NAME_3, LAST_NAME_3, EMAIL_3, USERID2, 1, 2)]
        [InlineData(FIRST_NAME_4, LAST_NAME_4, EMAIL_4, USERID2, 0, 2)]
        [InlineData(FIRST_NAME_5, LAST_NAME_5, EMAIL_5, USERID3, 0, 1)]
        public async Task TestGetAllContacts(string firstName, string lastName, string email, string userId, int index, int expectedCount)
        {
            using (var context = new MyContactManagerDbContext(_options))
            {
                _repository = new ContactsRepository(context);

                var contacts = await _repository.GetAllAsync(userId);
                contacts.Count.ShouldBe(expectedCount);
                contacts[index].FirstName.ShouldBe(firstName, StringCompareShould.IgnoreCase);
                contacts[index].LastName.ShouldBe(lastName, StringCompareShould.IgnoreCase);
                contacts[index].Email.ShouldBe(email, StringCompareShould.IgnoreCase);
                contacts[index].UserId.ShouldBe(userId, StringCompareShould.IgnoreCase);
            }
        }

        [Theory]
        [InlineData(FIRST_NAME_1, LAST_NAME_1, EMAIL_1, USERID1, 1)]
        [InlineData(FIRST_NAME_2, LAST_NAME_2, EMAIL_2, USERID1, 2)]
        [InlineData(FIRST_NAME_3, LAST_NAME_3, EMAIL_3, USERID2, 3)]
        [InlineData(FIRST_NAME_4, LAST_NAME_4, EMAIL_4, USERID2, 4)]
        [InlineData(FIRST_NAME_5, LAST_NAME_5, EMAIL_5, USERID3, 5)]
        public async Task GetContact(string firstName, string lastName, string email, string userId, int contactId)
        {
            using (var context = new MyContactManagerDbContext(_options))
            {
                _repository = new ContactsRepository(context);

                var contact = await _repository.GetAsync(contactId, userId);
                contact.ShouldNotBeNull();
                contact.FirstName.ShouldBe(firstName, StringCompareShould.IgnoreCase);
                contact.LastName.ShouldBe(lastName, StringCompareShould.IgnoreCase);
                contact.Email.ShouldBe(email, StringCompareShould.IgnoreCase);
                contact.UserId.ShouldBe(userId, StringCompareShould.IgnoreCase);
            }
        }

        //ensure can't get someone else's contact information
        [Theory]
        [InlineData(FIRST_NAME_1, LAST_NAME_1, EMAIL_1, USERID2, 1)]
        [InlineData(FIRST_NAME_2, LAST_NAME_2, EMAIL_2, USERID3, 2)]
        [InlineData(FIRST_NAME_3, LAST_NAME_3, EMAIL_3, USERID1, 3)]
        [InlineData(FIRST_NAME_4, LAST_NAME_4, EMAIL_4, USERID3, 4)]
        [InlineData(FIRST_NAME_5, LAST_NAME_5, EMAIL_5, USERID2, 5)]
        public async Task CannotGetSomeoneElsesContacts(string firstName, string lastName, string email, string userId, int contactId)
        {
            using (var context = new MyContactManagerDbContext(_options))
            {
                _repository = new ContactsRepository(context);

                var contact = await _repository.GetAsync(contactId, userId);
                contact.ShouldBeNull();
            }
        }

        [Fact]
        public async Task UpdateContact()
        {
            using (var context = new MyContactManagerDbContext(_options))
            {
                _repository = new ContactsRepository(context);

                var contactToUpdate = await _repository.GetAsync(2, USERID1);
                contactToUpdate.ShouldNotBeNull();
                contactToUpdate.FirstName.ShouldBe(FIRST_NAME_2, StringCompareShould.IgnoreCase);

                contactToUpdate.FirstName = FIRST_NAME_2_UPDATED;
                await _repository.AddOrUpdateAsync(contactToUpdate, USERID1);

                var updatedContact = await _repository.GetAsync(2, USERID1);
                updatedContact.ShouldNotBe(null);
                updatedContact.FirstName.ShouldBe(FIRST_NAME_2_UPDATED, StringCompareShould.IgnoreCase);

                //put it back:
                updatedContact.FirstName = FIRST_NAME_2;
                await _repository.AddOrUpdateAsync(updatedContact, USERID1);

                var revertedState = await _repository.GetAsync(2, USERID1);
                revertedState.ShouldNotBe(null);
                revertedState.FirstName.ShouldBe(FIRST_NAME_2, StringCompareShould.IgnoreCase);
            }
        }

        [Fact]
        public async Task AddAndDeleteContact()
        {
            using (var context = new MyContactManagerDbContext(_options))
            {
                _repository = new ContactsRepository(context);

                //add the state and validate it is stored
                var contactToAdd = new Contact()
                {
                    Birthday = new DateTime(1971, 10, 12),
                    City = "Somewhere",
                    Email = "jlpicard@starfleet.com",
                    FirstName = "Jean-Luc"
                                ,
                    LastName = "Picard",
                    PhonePrimary = "555-555-9999",
                    PhoneSecondary = "555-555-9999",
                    StateId = 4
                                ,
                    StreetAddress1 = "999 Fourth St",
                    StreetAddress2 = "UNIT 99",
                    UserId = USERID2,
                    Zip = "99999"
                };

                await _repository.AddOrUpdateAsync(contactToAdd, USERID2);

                var updatedContact = await _repository.GetAsync(contactToAdd.Id, USERID2);
                updatedContact.ShouldNotBeNull();
                updatedContact.FirstName.ShouldBe("Jean-Luc", StringCompareShould.IgnoreCase);
                updatedContact.Email.ShouldBe("jlpicard@starfleet.com", StringCompareShould.IgnoreCase);

                //delete to keep current count and list in tact.
                await _repository.DeleteAsync(updatedContact.Id, USERID2);
                var deletedContact = await _repository.GetAsync(updatedContact.Id, USERID2);
                deletedContact.ShouldBeNull();
            }
        }

        //TODO: //should also test can't update/delete someone elses contacts
    }
}
