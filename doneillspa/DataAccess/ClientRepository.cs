using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;
using Microsoft.EntityFrameworkCore;

namespace doneillspa.DataAccess
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationContext _context;

        public ClientRepository(ApplicationContext context)
        {
            _context = context;
        }

        public Client GetClientById(long id)
        {
            return _context.Client
            .Include(b => b.Projects)
            .Where(b => b.Id == id)
            .FirstOrDefault();
        }

        public IEnumerable<Client> GetClients()
        {
            return _context.Client.Include(b => b.Projects).ToList();
        }

        public long InsertClient(Client c)
        {
            _context.Client.Add(c);
            _context.SaveChanges();
            return c.Id;
        }

        public void UpdateClient(Client c)
        {
            _context.Entry(c).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
