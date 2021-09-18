using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SALGADBLib
{
    public interface IDashboardPermissionsRepository
    {
        public Task<IEnumerable<DasboardLevelRole>> GetAllRoles();
        public Task<IEnumerable<DasboardProvinceAccess>> GetProvincialAccessList();
    }
}
