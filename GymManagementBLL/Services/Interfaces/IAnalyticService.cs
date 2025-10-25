using GymManagementBLL.ViewModels.AnalyticsViewModel;
using GymManagementBLL.ViewModels.SessionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IAnalyticService
    {
        AnalyticsViewModel GetAnalyticsData();
    }
}
