using PruebaTecnicaZoco.Repository.Studies;
using PruebaTecnicaZoco.Services.StudyService.StudiesDto;
using PruebaTecnicaZoco.Services.StudyService.StudiesDTO;

namespace PruebaTecnicaZoco.Services.StudyService
{
    public interface IStudyService
    {
        Task<Study> CreateStudyAsync(StudyDTO study);
        Task<IEnumerable<Study>> GetAllStudiesAsync();
        Task<Study> GetStudyByIdAsync(int id);
        Task<Study> UpdateStudyAsync(StudyModifyDTO study);
        Task<bool> DeleteStudyAsync(int id);
    }
}
