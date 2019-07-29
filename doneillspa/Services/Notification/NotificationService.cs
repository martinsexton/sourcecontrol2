using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;

namespace doneillspa.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;

        public NotificationService(INotificationRepository nr)
        {
            _repository = nr;
        }
        public void DeleteNotification(Notification not)
        {
            _repository.DeleteNotification(not);
        }

        public EmailNotification GetEmailNotificationById(long id)
        {
            return _repository.GetEmailNotificationById(id);
        }

        public IEnumerable<EmailNotification> GetEmailNotificationsByUserId(string userId)
        {
            return _repository.GetEmailNotificationsByUserId(userId);
        }
    }
}
