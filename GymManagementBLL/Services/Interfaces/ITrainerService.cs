using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementBLL.ViewModels.TrainerViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainer();
        bool CreateTrainer(CreateTrainerViewModel createTrainer);
        bool UpdateTrainerDetails(UpdateTrainerViewModel updateTrainer, int trainerId);
        TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId);
        bool RemoveTrainer(int  trainerId);
        TrainerViewModel? GetTrainerDetails(int trainerId);
    }
}
