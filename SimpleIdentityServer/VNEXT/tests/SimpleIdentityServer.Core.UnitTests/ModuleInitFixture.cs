﻿using Moq;
using SimpleIdentityServer.Common;
using SimpleIdentityServer.Core.Helpers;
using System;
using Xunit;

namespace SimpleIdentityServer.Core.UnitTests
{
    public sealed class ModuleInitFixture
    {
        private IModule _moduleInit;

        [Fact]
        public void When_Passing_Null_Parameter_To_Initialize_Then_Exception_Is_Thrown()
        {
            // ARRANGE
            InitializeFakeObjets();

            // ACT & ASSERT
            Assert.Throws<ArgumentNullException>(() => _moduleInit.Initialize(null));
        }

        [Fact]
        public void When_Calling_Initialize_Then_Register_Methods_Are_Called()
        {
            // ARRANGE
            InitializeFakeObjets();

            // ACT
            var registerFake = new Mock<IModuleRegister>();
            _moduleInit.Initialize(registerFake.Object);

            // ASSERT
            registerFake.Verify(r => r.RegisterType<ISecurityHelper, SecurityHelper>());
        }

        private void InitializeFakeObjets()
        {
            _moduleInit = new ModuleInit();
        }
    }
}
