// <copyright file="WFSModelerFormTest.cs">Copyright ©  2010</copyright>
using System;
using GUI;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GUI.Tests
{
    /// <summary>This class contains parameterized unit tests for WFSModelerForm</summary>
    [PexClass(typeof(WFSModelerForm))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class WFSModelerFormTest
    {
    }
}
