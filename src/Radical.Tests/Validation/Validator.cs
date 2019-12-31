using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.Validation;
using System.Linq;

namespace Radical.Tests.Validation
{
    [TestClass()]
    public class Validator
    {
        class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        [TestMethod()]
        public void Should_validate_all_properties()
        {
            var sut = new Validator<Person>();
            sut.AddRule(p => p.FirstName, ctx =>
            {
                return ctx.Entity.FirstName == null
                    ? ctx.Failed("FirstName cannot be null")
                    : ctx.Succeeded();
            });

            sut.AddRule(p => p.LastName, ctx =>
            {
                return ctx.Entity.LastName == null
                    ? ctx.Failed("LastName cannot be null")
                    : ctx.Succeeded();
            });

            var results = sut.Validate(new Person());

            Assert.AreEqual(2, results.Errors.Count());
        }

        [TestMethod()]
        public void Should_validate_single_property()
        {
            var sut = new Validator<Person>();
            sut.AddRule(p => p.FirstName, ctx =>
            {
                return ctx.Entity.FirstName == null
                    ? ctx.Failed("FirstName cannot be null")
                    : ctx.Succeeded();
            });

            sut.AddRule(p => p.LastName, ctx =>
            {
                return ctx.Entity.LastName == null
                    ? ctx.Failed("LastName cannot be null")
                    : ctx.Succeeded();
            });

            var results = sut.ValidateProperty(new Person(), p => p.FirstName);

            Assert.AreEqual(1, results.Errors.Count());
        }

        [TestMethod()]
        public void Should_evalate_all_rules_for_single_property()
        {
            var entity = new Person() 
            {
                FirstName = "mau"
            };
            var sut = new Validator<Person>();
            sut.AddRule(p => p.FirstName, ctx =>
            {
                return ctx.Entity.FirstName.Length <= 3
                    ? ctx.Failed("FirstName must be longer than 3 characters")
                    : ctx.Succeeded();
            });

            sut.AddRule(p => p.FirstName, ctx =>
            {
                return char.IsLower(ctx.Entity.FirstName.First())
                    ? ctx.Failed("First character of FirstName must be uppercase")
                    : ctx.Succeeded();
            });

            var results = sut.ValidateProperty(entity, p => p.FirstName);

            Assert.AreEqual(1, results.Errors.Count());
            Assert.AreEqual(2, results.Errors.Single().DetectedProblems.Count());
        }
    }
}
