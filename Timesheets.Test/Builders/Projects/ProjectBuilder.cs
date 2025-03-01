using Bogus;
using Timesheets.Domain.Entities.Projects;

namespace Timesheets.Test.Builders.Projects
{
    public class ProjectBuilder
    {
        private string _name = "";
        private bool _isDeleted = false;

        public ProjectBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ProjectBuilder WithIsDeleted(bool isdeleted)
        {
            _isDeleted = isdeleted;
            return this;
        }

        public Project Build()
        {
            var faker = new Faker<Project>()
                .RuleFor(x => x.Id, x => Guid.NewGuid())
                .RuleFor(x => x.DateCreated, x => DateTime.Now)
                .RuleFor(x => x.DateModified, x => DateTime.Now)
                .RuleFor(x => x.IsDeleted, x => _isDeleted)
                .RuleFor(x => x.Name, x => _name ?? x.Random.Word());

            return faker.Generate(1).First();
        }
    }
}
