namespace Radical.Tests.Model
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical.Model;
    using SharpTestsEx;
    using System.Collections.Generic;
    using System.Linq;

    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    [TestClass]
    public class EntityItemViewTests
    {
        [TestMethod]
        public void entityItemView_equals_normal_should_behave_as_expected()
        {
            var expected = true;

            var dataSource = new List<Person>()
            {
                new Person(){ FirstName = "Mauro", LastName = "Servienti" }
            };

            var view1 = new EntityView<Person>(dataSource);
            var view2 = new EntityView<Person>(dataSource);

            view1.ApplySort("FirstName");

            var item1 = view1.ElementAt(0);
            var item2 = view2.ElementAt(0);

            var actual = item1.Equals(item2);

            actual.Should().Be.EqualTo(expected);
        }
    }
}
