// using NetArchTest.Rules;

// namespace Architecture.Tests;

// public class ArchitectureTests
// {
//     [Fact]
//     public void ServiceClassesShouldHaveNameEndingWithService()
//     {
//         var result = Types.InCurrentLinkedIn.Domain()
//                     .That().ResideInNamespace("LinkedIn.Application")
//                     .And().AreClasses()
//                     .Should().HaveNameEndingWith("Service")
//                     .GetResult();
//         Assert.True(result.IsSuccessful);
//     }

//     [Fact]
//     public void ServiceClassesShouldImplementIBaseServiceInterface()
//     {
//         var result = Types.InCurrentLinkedIn.Domain()
//                     .That().ResideInNamespace("LinkedIn.Application")
//                     .And().AreClasses()
//                     .Should().ImplementInterface(typeof(IBaseService))
//                     .GetResult();
//         Assert.True(result.IsSuccessful);
//     }

//     [Fact]
//     public void ServiceClassesShouldBePublicAndNotSealed()
//     {
//         var result = Types.InCurrentLinkedIn.Domain()
//                     .That().ResideInNamespace("LinkedIn.Application")
//                     .Should().BePublic().And().NotBeSealed()
//                     .GetResult();
//         Assert.True(result.IsSuccessful);
//     }
// }