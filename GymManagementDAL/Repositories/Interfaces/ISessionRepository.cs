using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ISessionRepository
    {
        IEnumerable<Session> GetAll();
        Session? GetById(int id);
        int Add(Session session);
        int Update(Session session);
        int Delete(Session session);
    }
}
