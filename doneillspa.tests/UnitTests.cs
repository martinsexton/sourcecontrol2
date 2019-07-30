using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using doneillspa.Auth;
using doneillspa.Controllers;
using doneillspa.DataAccess;
using doneillspa.Dtos;
using doneillspa.Models;
using doneillspa.Services;
using doneillspa.Services.Calendar;
using doneillspa.Services.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace doneillspa.tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void EnsureEmailIsSentWhenNotificationCreated()
        {
            string notificationBody = "gerard.sexton@gmail.com";
            string notificationDestinationEmail = "new notification body";
            string notificationSubject = "new notification subject";

            var mockEmailService = new Mock<IEmailService>();
            mockEmailService.Setup(mock => mock.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            EmailNotification notification = new EmailNotification();
            notification.DestinationEmail = notificationDestinationEmail;
            notification.Body = notificationBody;
            notification.Subject = notificationSubject;

            notification.Created(mockEmailService.Object);

            mockEmailService.Verify(mock => mock.SendMail(It.IsAny<string>(), notificationDestinationEmail, notificationSubject,
                                notificationBody, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());

        }
        [TestMethod]
        public void EnsureMailIsSentToApproverWhenHolidayRequestCreated()
        {
            var mockEmailService = new Mock<IEmailService>();
            mockEmailService.Setup(mock => mock.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            HolidayRequest request = new HolidayRequest();
            request.User = new ApplicationUser();
            request.User.FirstName = "Martin";
            request.User.Surname = "Sexton";

            request.Approver = new ApplicationUser();
            request.Approver.FirstName = "Gerard";
            request.Approver.Surname = "Sexton";
            request.Approver.Email = "gerard.sexton@gmail.com";
            request.FromDate = DateTime.Now;
            request.Days = 2;

            request.Created(mockEmailService.Object);

            mockEmailService.Verify(mock => mock.SendMail(It.IsAny<string>(), "gerard.sexton@gmail.com", It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void EnsureGmailCalendarUpdatedAndMailCreatedWhenHolidayApproved()
        {
            var mockCalendarService = new Mock<ICalendarService>();
            mockCalendarService.Setup(mock => mock.CreateEvent(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<string>()));

            var mockEmailService = new Mock<IEmailService>();
            mockEmailService.Setup(mock => mock.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            var mockTimesheetService = new Mock<ITimesheetService>();
            mockTimesheetService.Setup(mock => mock.RecordAnnualLeave(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>()));


            HolidayRequest request = new HolidayRequest();
            request.User = new ApplicationUser();
            request.User.FirstName = "Martin";
            request.User.Surname = "Sexton";
            request.User.Email = "user.mail@gmail.com";
            request.FromDate = DateTime.Now;
            request.Days = 2;

            HolidayRequestDto dto = new HolidayRequestDto();
            dto.Status = "Approved";

            request.Updated(dto, mockCalendarService.Object, mockEmailService.Object, mockTimesheetService.Object);

            //Verify that a call is made to create the event in calendar
            mockCalendarService.Verify(mock => mock.CreateEvent(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once());

            //Verify that an approval mail has been sent also to the user.
            mockEmailService.Verify(mock => mock.SendMail(It.IsAny<string>(), "user.mail@gmail.com", "Holiday Request Approved",
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());

        }

        [TestMethod]
        public void EnsureRejectionMailSentToCustomerWhenHolidayRejected()
        {
            var mockEmailService = new Mock<IEmailService>();
            mockEmailService.Setup(mock => mock.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            var mockTimesheetService = new Mock<ITimesheetService>();
            mockTimesheetService.Setup(mock => mock.RecordAnnualLeave(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>()));

            HolidayRequest request = new HolidayRequest();
            request.User = new ApplicationUser();
            request.User.FirstName = "Martin";
            request.User.Surname = "Sexton";
            request.User.Email = "user.mail@gmail.com";
            request.FromDate = DateTime.Now;
            request.Days = 2;

            HolidayRequestDto dto = new HolidayRequestDto();
            dto.Status = "Rejected";

            request.Updated(dto, null, mockEmailService.Object, mockTimesheetService.Object);

            //Verify that an approval mail has been sent also to the user.
            mockEmailService.Verify(mock => mock.SendMail(It.IsAny<string>(), "user.mail@gmail.com", "Holiday Request Rejected",
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void EnsureGmailCalendarEventNotCreatedWhenAlreadyApproved()
        {
            var mockCalendarService = new Mock<ICalendarService>();
            mockCalendarService.Setup(mock => mock.CreateEvent(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<string>()));

            var mockEmailService = new Mock<IEmailService>();
            mockEmailService.Setup(mock => mock.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            var mockTimesheetService = new Mock<ITimesheetService>();
            mockTimesheetService.Setup(mock => mock.RecordAnnualLeave(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>()));

            HolidayRequest request = new HolidayRequest();
            request.Status = HolidayRequestStatus.Approved;
            request.User = new ApplicationUser();
            request.User.FirstName = "Martin";
            request.User.Surname = "Sexton";
            request.FromDate = DateTime.Now;
            request.Days = 2;

            HolidayRequestDto dto = new HolidayRequestDto();
            dto.Status = "Approved";

            request.Updated(dto, mockCalendarService.Object, mockEmailService.Object, mockTimesheetService.Object);

            //Verify that a call is not made to create an event in calendar if already approved
            mockCalendarService.Verify(mock => mock.CreateEvent(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void TestCertificateHasNotExpired()
        {
            Certification cert = new Certification();
            cert.Expiry = DateTime.UtcNow.AddDays(1);

            Assert.IsFalse(cert.HasExpired());
        }

        [TestMethod]
        public void TestCertificateHasExpired()
        {
            Certification cert = new Certification();
            cert.Expiry = DateTime.UtcNow.AddDays(-1);

            Assert.IsTrue(cert.HasExpired());
        }

        [TestMethod]
        public void TestCertificateHasExpiredOnSameDay()
        {
            //If expiry date is today then its considered expired.
            Certification cert = new Certification();
            cert.Expiry = DateTime.UtcNow;

            Assert.IsTrue(cert.HasExpired());
        }
    }
}
