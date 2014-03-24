namespace Castle.Windsor
{
	using System;
	using System.Linq;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Rhino.Mocks;
	using SharpTestsEx;
	using Castle.MicroKernel.Registration;
	using Castle.MicroKernel.SubSystems.Naming;
	using Castle.MicroKernel;

	[TestClass]
	public class ContainerExtensionsTests
	{
		[TestMethod]
		public void containerExtensions_isRegistered_using_a_registered_type_should_return_true()
		{
			var target = new WindsorContainer();
			target.Register( Component.For<Object>() );

			var actual = ContainerExtensions.IsRegistered<Object>( target );

			actual.Should().Be.True();
		}

		[TestMethod]
		public void containerExtensions_isRegistered_using_a_non_registered_type_should_return_false()
		{
			var target = new WindsorContainer();

			var actual = ContainerExtensions.IsRegistered<Object>( target );

			actual.Should().Be.False();
		}

		interface IFoo { }

		class AFoo : IFoo { }
		class AnOtherFoo : IFoo { }

		[TestMethod]
		[TestCategory( "ContainerExtensions" )]
		public void ContainerExtensions_overrideRegistration_should_override_a_previous_component()
		{
			var nss = new DelegateNamingSubSystem()
			{
				SubSystemHandler = ( s, hs ) =>
				{
					var containsOverridableServices = hs.Where( h => h.ComponentModel.IsOverridable() )
						.Any();

					if( containsOverridableServices && hs.Count() == 2 )
					{
						return hs.Single( h => !h.ComponentModel.IsOverridable() );
					}

					return null;
				}
			};

			var sut = new WindsorContainer();
			sut.Kernel.AddSubSystem( SubSystemConstants.NamingKey, nss );

			sut.Register( Component.For<IFoo>().ImplementedBy<AFoo>().Overridable() );
			sut.Register( Component.For<IFoo>().ImplementedBy<AnOtherFoo>() );

			var foo = sut.Resolve<IFoo>();
			foo.Should().Be.OfType<AnOtherFoo>();
		}
	}
}
