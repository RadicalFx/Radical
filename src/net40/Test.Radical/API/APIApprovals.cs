using ApprovalTests;
using ApprovalTests.Reporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PublicApiGenerator;
using System.Runtime.CompilerServices;
using Topics.Radical.ComponentModel.Messaging;

namespace Test.Radical.API
{
    [TestClass]
    public class APIApprovals
    {
        [TestMethod]
        [TestCategory("APIApprovals")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        [UseReporter(typeof(DiffReporter))]
        public void Approve_FXV40_API()
        {
            var publicApi = ApiGenerator.GeneratePublicApi(typeof(IMessageBroker).Assembly);

            Approvals.Verify(publicApi);
        }
    }
}