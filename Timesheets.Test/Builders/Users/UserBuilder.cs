using Bogus;
using Timesheets.Domain.Entities.Users;
using Timesheets.Test.Builders.Projects;

namespace Timesheets.Test.Builders.Users
{
    public class UserBuilder
    {
        private string _userName;
        private bool _isDeleted = false;

        public UserBuilder WithUserName(string name)
        {
            _userName = name;
            return this;
        }

        public UserBuilder WithIsDeleted(bool isdeleted)
        {
            _isDeleted = isdeleted;
            return this;
        }

        public User Build()
        {
            var faker = new Faker<User>()
                .RuleFor(x => x.Id, x => Guid.NewGuid())
                .RuleFor(x => x.DateCreated, x => DateTime.Now)
                .RuleFor(x => x.DateModified, x => DateTime.Now)
                .RuleFor(x => x.IsDeleted, x => _isDeleted)
                .RuleFor(x => x.UserName, x => _userName ?? x.Name.FullName());

            return faker.Generate(1).First();
        }
    }
}
