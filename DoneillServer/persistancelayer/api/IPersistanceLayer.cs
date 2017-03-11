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
        List<IProject> RetrieveProjects();
    }
}
