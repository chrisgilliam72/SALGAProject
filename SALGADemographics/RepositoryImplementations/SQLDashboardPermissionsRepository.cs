using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SALGADBLib
{
    public class SQLDashboardPermissionsRepository : IDashboardPermissionsRepository
    {
        private SALGADbContext _dbContext;
        public SQLDashboardPermissionsRepository(SALGADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DasboardLevelRole>> GetAllRoles()
        {
           var roles = await _dbContext.DasboardLevelRoles.ToListAsync();
            return roles;
        }

        public async Task<IEnumerable<DasboardProvinceAccess>> GetProvincialAccessList()
        {
            var provinceRoles = await _dbContext.DasboardProvinceAccesses.Include(x=>x.Role)
                                                                         .Include(x=>x.Province).ToListAsync();
            return provinceRoles;
        }
    }
}
