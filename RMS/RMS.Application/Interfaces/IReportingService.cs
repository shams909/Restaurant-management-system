using System.Threading.Tasks;
using RMS.Application.DTOs;

namespace RMS.Application.Interfaces
{
    public interface IReportingService
    {
        Task<DashboardDto> GetDailyDashboardAsync();
    }
}
