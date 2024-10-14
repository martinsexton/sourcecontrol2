using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class Project
    {
        public long Id { get; set; }
        public long OwningClientId { get; set; }
        public virtual Client OwningClient { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Details { get; set; }
        public DateTime StartDate { get; set; }
        public bool Chargeable { get; set; }

        public Project() { }

        public Project(string code, string name, string details, DateTime startDate, bool chargeable)
        {
            Code = code;
            Name = name;
            Details = details;
            StartDate = startDate;
            IsActive = true;
            Chargeable = chargeable;
        }

        public void Activate()
        {
            this.IsActive = true;
        }

        public void Disable()
        {
            this.IsActive = false;
        }
    }
}
