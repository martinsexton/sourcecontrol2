using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using doneillspa.Auth;
using doneillspa.Controllers;
using doneillspa.DataAccess;
using doneillspa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        //[TestMethod]
        //public void TestEmailSetupInSessionOnLogin()
        //{
        //    var mockUserManager = GetMockUserManager();
        //    var mockIJwtFactory = new Mock<IJwtFactory>();
        //    ApplicationUser mockUser = new ApplicationUser();
        //    var mockClaimsIdentity = new Mock<ClaimsIdentity>();
        //    var mockIdClaim = new Mock<Claim>();
        //    var mockRoleClaim = new Mock<Claim>();

        //    List<Claim> mockedClaims = new List<Claim>();
        //    mockedClaims.Add(mockIdClaim.Object);
        //    mockedClaims.Add(mockRoleClaim.Object);


        //    IList<string> roles = new List<string>();
        //    roles.Add("Administrator");

        //    //Setup the user manager to return the mocked application user.
        //    mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(mockUser));
        //    mockUserManager.Setup(m => m.GetRolesAsync(It.IsAny<ApplicationUser>())).Returns(Task.FromResult(roles));
        //    mockUserManager.Setup(m => m.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns(Task.FromResult(true));
        //    mockIJwtFactory.Setup(m => m.GenerateClaimsIdentity(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<IList<string>>())).Returns(mockClaimsIdentity.Object);
        //    mockClaimsIdentity.Setup(m => m.Claims).Returns(mockedClaims);
        //    mockIJwtFactory.Setup(m => m.GenerateEncodedToken(It.IsAny<string>(), It.IsAny<ClaimsIdentity>(), It.IsAny<TimeSpan>())).Returns(Task.FromResult(""));

        //    RegistrationDetails regDetails = new RegistrationDetails();
        //    regDetails.Email = "test@test.com";
        //    regDetails.FirstName = "Joe";
        //    regDetails.Surname = "Bloggs";
        //    regDetails.Role = "Administrator";
        //    regDetails.Password = "password";
        //    regDetails.Phone = "0877485958";

        //    AuthController testController = new AuthController(mockUserManager.Object, mockIJwtFactory.Object);
        //    JsonResult result = testController.Post(regDetails).Result;

        //    Assert.IsTrue(result.StatusCode == 401);
        //}

        //private Mock<UserManager<ApplicationUser>> GetMockUserManager()
        //{
        //    var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        //    return new Mock<UserManager<ApplicationUser>>(
        //        userStoreMock.Object, null, null, null, null, null, null, null, null);
        //}
    }
}
