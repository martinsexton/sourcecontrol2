using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using doneillspa.Auth;
using doneillspa.Controllers;
using doneillspa.DataAccess;
using doneillspa.Dtos;
using doneillspa.Models;
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
        public void EnsureGmailCalendarEventCreatedWhenHolidayApproved()
        {
            var mockCalendarService = new Mock<ICalendarService>();
            mockCalendarService.Setup(mock => mock.CreateEvent(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<string>()));

            HolidayRequest request = new HolidayRequest();
            request.User = new ApplicationUser();
            request.User.FirstName = "Martin";
            request.User.Surname = "Sexton";
            request.FromDate = DateTime.Now;
            request.Days = 2;

            HolidayRequestDto dto = new HolidayRequestDto();
            dto.Status = "Approved";

            request.Updated(dto, mockCalendarService.Object);

            //Verify that a call is made to create the event in calendar
            mockCalendarService.Verify(mock => mock.CreateEvent(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once());

        }

        [TestMethod]
        public void EnsureGmailCalendarEventNotCreatedWhenAlreadyApproved()
        {
            var mockCalendarService = new Mock<ICalendarService>();
            mockCalendarService.Setup(mock => mock.CreateEvent(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<string>()));

            HolidayRequest request = new HolidayRequest();
            request.Status = HolidayRequestStatus.Approved;
            request.User = new ApplicationUser();
            request.User.FirstName = "Martin";
            request.User.Surname = "Sexton";
            request.FromDate = DateTime.Now;
            request.Days = 2;

            HolidayRequestDto dto = new HolidayRequestDto();
            dto.Status = "Approved";

            request.Updated(dto, mockCalendarService.Object);

            //Verify that a call is not made to create an event in calendar if already approved
            mockCalendarService.Verify(mock => mock.CreateEvent(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never());

        }

    }
}
