using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;
using Microsoft.EntityFrameworkCore;

namespace doneillspa.DataAccess
{
    public class NotificationRepository : INotificationRepository
    {

        private readonly ApplicationContext _context;

        public NotificationRepository(ApplicationContext context)
        {
            _context = context;
        }
        public void DeleteNotification(Notification not)
        {
            _context.Entry(not).State = EntityState.Deleted;
            Save();
        }

        public EmailNotification GetEmailNotificationById(long id)
        {
            return _context.EmailNotification
                        .Where(b => b.Id == id)
                        .FirstOrDefault();
        }

        public IEnumerable<EmailNotification> GetEmailNotificationsByUserId(string userId)
        {
            return _context.EmailNotification
                        .Where(b => b.UserId.ToString() == userId)
                        .ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
