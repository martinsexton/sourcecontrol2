using doneillspa.Controllers;
using doneillspa.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace doneillspa.tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // create mock version
            var mockCertificationRepository = new Mock<ICertificationRepository>();

            CertificationController testController = new CertificationController(mockCertificationRepository.Object);

            Assert.IsTrue(testController.ShouldICertify());
        }
    }
}
