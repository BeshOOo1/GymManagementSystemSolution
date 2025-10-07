using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IMembershipsRepository
    {
        IEnumerable<MemberShip> GetAll();
        MemberShip? GetById(int id);
        int Add(MemberShip memberShip);
        int Update(MemberShip memberShip);
        int Delete(MemberShip memberShip);
    }
}
