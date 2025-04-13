using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaZoco.Services.StudyService;
using PruebaTecnicaZoco.Services.StudyService.StudiesDto;
using PruebaTecnicaZoco.Services.StudyService.StudiesDTO;

namespace PruebaTecnicaZoco.Controllers
{
    [ApiController]
    [Route("api/Estudios")]
    public class StudyController : ControllerBase
    {
        private readonly IStudyService _studyService;

        public StudyController(IStudyService studyService)
        {
            _studyService = studyService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateStudy([FromBody] StudyDTO study)
        {
            var createdStudy = await _studyService.CreateStudyAsync(study);
            return CreatedAtAction(nameof(GetStudyById), new { id = createdStudy.Id }, createdStudy);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllStudies()
        {
            var studies = await _studyService.GetAllStudiesAsync();
            return Ok(studies);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetStudyById(int id)
        {
            var study = await _studyService.GetStudyByIdAsync(id);
            return Ok(study);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateStudy([FromBody] StudyModifyDTO study)
        {
            var studyUpdate = await _studyService.UpdateStudyAsync(study);
            return Ok(studyUpdate);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteStudy(int id)
        {
            await _studyService.DeleteStudyAsync(id);
            return NoContent();
        }
    }
}
