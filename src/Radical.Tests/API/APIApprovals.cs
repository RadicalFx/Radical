﻿using ApprovalTests;
using ApprovalTests.Core;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PublicApiGenerator;
using Radical.ComponentModel.Messaging;
using System.Runtime.CompilerServices;

namespace Radical.Tests.API
{
    [TestClass]
    public class APIApprovals
    {
        [TestMethod]
        [TestCategory("APIApprovals")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        [UseReporter(typeof(DiffReporter))]
#if NET_CORE
        [UseApprovalSubdirectory("netstandard")]
#else
        [UseApprovalSubdirectory("net")]
#endif
        public void Approve_API()
        {
            var publicApi = ApiGenerator.GeneratePublicApi(typeof(IMessageBroker).Assembly, options: null);

            Approvals.Verify(publicApi);
        }
    }
}