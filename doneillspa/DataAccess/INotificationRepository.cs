﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.DataAccess
{
    public interface INotificationRepository
    {
        IEnumerable<EmailNotification> GetEmailNotificationsByUserId(string userId);
        EmailNotification GetEmailNotificationById(long id);
        void DeleteNotification(Notification not);
        void Save();
    }
}
