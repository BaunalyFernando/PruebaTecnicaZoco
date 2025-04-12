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
            if (string.IsNullOrEmpty(study.Nombre) || string.IsNullOrEmpty(study.Descripcion))
            {
                return BadRequest("Por favor ingrese todos los datos de los campos");
            }

            try
            {
                var createdStudy = await _studyService.CreateStudyAsync(study);
                return CreatedAtAction(nameof(GetStudyById), new { id = createdStudy.Id }, createdStudy);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllStudies()
        {
            try
            {
                var studies = await _studyService.GetAllStudiesAsync();
                return Ok(studies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetStudyById(int id)
        {
            try
            {
                var study = await _studyService.GetStudyByIdAsync(id);
                
                return Ok(study);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateStudy([FromBody] StudyModifyDTO study)
        {
            try
            {
                var studyUpdate = await _studyService.UpdateStudyAsync(study);
                return Ok(studyUpdate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteStudy(int id)
        {
            try
            {
                var studyToDelete = await _studyService.DeleteStudyAsync(id);
                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
