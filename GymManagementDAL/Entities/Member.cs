using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        // joinDate == CreatedAt of BaseEntity
        public string? Photo { get; set; }

        #region Relationships

        #region Member - HealthRecord

        public HealthRecord HealthRecord { get; set; } = null!;
        #endregion

        #region Member - MemberShip

        public ICollection<MemberShip> MemberShips { get; set; } = null;
        #endregion

        #region Member - MemberSession
        public ICollection<MemberSession> MemberSessions { get; set; } = null;

        public static implicit operator Member(Member v)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
