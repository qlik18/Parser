// <copyright file="PexAssemblyInfo.cs">Copyright ©  2010</copyright>
using Microsoft.Pex.Framework.Coverage;
using Microsoft.Pex.Framework.Creatable;
using Microsoft.Pex.Framework.Instrumentation;
using Microsoft.Pex.Framework.Settings;
using Microsoft.Pex.Framework.Validation;

// Microsoft.Pex.Framework.Settings
[assembly: PexAssemblySettings(TestFramework = "VisualStudioUnitTest")]

// Microsoft.Pex.Framework.Instrumentation
[assembly: PexAssemblyUnderTest("GUI")]
[assembly: PexInstrumentAssembly("System.Xml.Linq")]
[assembly: PexInstrumentAssembly("Utility")]
[assembly: PexInstrumentAssembly("System.Core")]
[assembly: PexInstrumentAssembly("Microsoft.CSharp")]
[assembly: PexInstrumentAssembly("System.Configuration")]
[assembly: PexInstrumentAssembly("Logging")]
[assembly: PexInstrumentAssembly("SautinSoft.HtmlToRtf")]
[assembly: PexInstrumentAssembly("Entities")]
[assembly: PexInstrumentAssembly("Logic")]
[assembly: PexInstrumentAssembly("System.Windows.Forms")]
[assembly: PexInstrumentAssembly("System.Drawing")]
[assembly: PexInstrumentAssembly("NetSpell.SpellChecker")]
[assembly: PexInstrumentAssembly("Atlassian.Jira")]

// Microsoft.Pex.Framework.Creatable
[assembly: PexCreatableFactoryForDelegates]

// Microsoft.Pex.Framework.Validation
[assembly: PexAllowedContractRequiresFailureAtTypeUnderTestSurface]
[assembly: PexAllowedXmlDocumentedException]

// Microsoft.Pex.Framework.Coverage
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Xml.Linq")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Utility")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Core")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.CSharp")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Configuration")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Logging")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "SautinSoft.HtmlToRtf")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Entities")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Logic")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Windows.Forms")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Drawing")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "NetSpell.SpellChecker")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Atlassian.Jira")]

