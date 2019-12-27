using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.Validation;
using System.Linq;

namespace Radical.Tests.Validation
{
    [TestClass()]
    class ValidatorBaseTests
    {
        class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        [TestMethod()]
        public void ValidatorBase_should_validate_single_property()
        {
            var sut = new ValidatorBase<Person>("");
            sut.AddRule(p => p.FirstName, "FirstName cannot be null", ctx =>
            {
                return ctx.Entity.FirstName == null
                    ? ComponentModel.Validation.RuleEvaluation.Failed
                    : ComponentModel.Validation.RuleEvaluation.Succeeded;
            });

            sut.AddRule(p => p.LastName, "LastName cannot be null", ctx =>
            {
                return ctx.Entity.LastName == null
                    ? ComponentModel.Validation.RuleEvaluation.Failed
                    : ComponentModel.Validation.RuleEvaluation.Succeeded;
            });

            var results = sut.Validate(new Person(), p => p.FirstName);
            Assert.AreEqual(1, results.Errors.Count());
        }
    }
}
