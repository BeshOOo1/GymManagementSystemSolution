using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class MembershipRepository : IMembershipsRepository
    {
        private readonly GymDbContext _dbContext;

        public MembershipRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Add(MemberShip memberShip)
        {
            _dbContext.MemberShips.Add(memberShip);
            return _dbContext.SaveChanges();
        }

        public int Delete(MemberShip memberShip)
        {
            _dbContext.MemberShips.Remove(memberShip);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<MemberShip> GetAll() => _dbContext.MemberShips.ToList();

        public MemberShip? GetById(int id) => _dbContext.MemberShips.Find(id);

        public int Update(MemberShip memberShip)
        {
            _dbContext.MemberShips.Update(memberShip);
            return _dbContext.SaveChanges();
        }
    }
}
