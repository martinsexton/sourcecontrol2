using persistancelayer.api.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace persistancelayer.api
{
    public interface IPersistanceLayer
    {
        void CreateProject(IProject p);
        void CreateEmployee(IEmployee emp);
        void UpdateProject(int identifier, IProject p);
        void CreateTimesheet(ITimeSheet t);

        List<IProject> RetrieveProjects();
        List<IEmployee> RetrieveEmployees();
        List<ITimeSheet> RetrieveTimesheets();
        List<ITimeSheetItem> RetrieveTimesheetItems(int timesheetId);
    }
}
