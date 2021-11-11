using Moq;
using ContactWebModels;
using MyContactManagerRepositories;
using MyContactManagerServices;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MyContactManagerUnitTests
{
    public class TestContactsService
    {
        private IContactsService _contactsService;
        private Mock<IContactsRepository> _contactsRepository;
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
        //you could go crazy here.

        public TestContactsService()
        {
            CreateMocks();
            _contactsService = new ContactsService(_contactsRepository.Object);
        }

        private void CreateMocks()
        {
            _contactsRepository = new Mock<IContactsRepository>();
            var contacts = GetContactsTestData();
            var singleContact = GetSingleContact();
            _contactsRepository.Setup(x => x.GetAllAsync(It.IsAny<string>())).Returns(contacts);
            _contactsRepository.Setup(x => x.GetAsync(It.IsAny<int>(), It.IsAny<string>())).Returns(singleContact);
        }

        private async Task<IList<Contact>> GetContactsTestData()
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

        private async Task<Contact?> GetSingleContact()
        {
            return new Contact()
            {
                Id = 4,
                Birthday = new DateTime(1982, 7, 23),
                City = "Fairbanks",
                Email = EMAIL_4,
                FirstName = FIRST_NAME_4
                                ,
                LastName = LAST_NAME_4,
                PhonePrimary = "555-555-4444",
                PhoneSecondary = "555-555-4444",
                StateId = 2
                                ,
                StreetAddress1 = "444 Fourth St",
                StreetAddress2 = "UNIT 44",
                UserId = USERID2,
                Zip = "44444"
            };
        }

        [Theory]
        [InlineData(FIRST_NAME_1, LAST_NAME_1, EMAIL_1, USERID1, 0)]
        [InlineData(FIRST_NAME_2, LAST_NAME_2, EMAIL_2, USERID1, 1)]
        [InlineData(FIRST_NAME_3, LAST_NAME_3, EMAIL_3, USERID2, 2)]
        [InlineData(FIRST_NAME_4, LAST_NAME_4, EMAIL_4, USERID2, 3)]
        [InlineData(FIRST_NAME_5, LAST_NAME_5, EMAIL_5, USERID3, 4)]
        public async Task TestGetAllContacts(string firstName, string lastName, string email, string userId, int index)
        {
            var contacts = await _contactsService.GetAllAsync(USERID1);
            contacts.Count.ShouldBe(NUMBER_OF_CONTACTS);
            contacts[index].FirstName.ShouldBe(firstName, StringCompareShould.IgnoreCase);
            contacts[index].LastName.ShouldBe(lastName, StringCompareShould.IgnoreCase);
            contacts[index].Email.ShouldBe(email, StringCompareShould.IgnoreCase);
            contacts[index].UserId.ShouldBe(userId, StringCompareShould.IgnoreCase);
        }

        [Fact]
        public async Task TestGetAContact()
        {
            var contact = await _contactsService.GetAsync(2, USERID1);
            contact.ShouldNotBe(null);
            contact.FirstName.ShouldBe(FIRST_NAME_4, StringCompareShould.IgnoreCase);
            contact.LastName.ShouldBe(LAST_NAME_4, StringCompareShould.IgnoreCase);
            contact.Email.ShouldBe(EMAIL_4, StringCompareShould.IgnoreCase);
            contact.UserId.ShouldBe(USERID2, StringCompareShould.IgnoreCase);
            contact.StateId.ShouldBe(2);
        }


    }
}
