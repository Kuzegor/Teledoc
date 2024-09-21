using Microsoft.AspNetCore.Mvc;
using Teledoc.Dto;
using Teledock.Interface;
using Teledock.Models;
using Teledock.Repository;

namespace Teledock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConstitutorController : Controller
    {
        private readonly IConstitutorRepository _constitutorRepository;
        public ConstitutorController(IConstitutorRepository constitutorRepository)
        {
            _constitutorRepository = constitutorRepository;
        }

        [HttpGet]
        public IActionResult GetConstitutors()
        {
            var constitutors = _constitutorRepository.GetConstitutors();

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            return Ok(constitutors);
        }

        [HttpGet("{constitutorId}")]
        public IActionResult GetConstitutor(int constitutorId)
        {
            if (_constitutorRepository.ConstitutorExists(constitutorId) == false)
                return NotFound();

            var constitutor = _constitutorRepository.GetConstitutor(constitutorId);

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            return Ok(constitutor);
        }

        [HttpGet("clients/{constitutorId}")]
        public IActionResult GetClientsByConstitutor(int constitutorId)
        {
            var clients = _constitutorRepository.GetClientsByConstitutor(constitutorId);

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            return Ok(clients);
        }


        [HttpPost]
        public IActionResult CreateConstitutor([FromQuery]ConstitutorDto constitutorDto, [FromQuery] int[]? clientIds)
        {
            if (constitutorDto == null)
                return BadRequest(ModelState);

            var existingConstitutor = _constitutorRepository.GetConstitutors()
                .Where(c => c.ConstitutorName.Trim().ToUpper() == constitutorDto.ConstitutorName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (existingConstitutor != null)
            {
                ModelState.AddModelError("", "Учредитель уже есть в базе");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Constitutor constitutor = new Constitutor 
            {
                INN = constitutorDto.INN,
                ConstitutorName = constitutorDto.ConstitutorName,
                DateAdded = DateTime.Today,
                DateOfUpdate = DateTime.Today
            };


            bool constitutorAdded;
            if (clientIds != null)
            {
                constitutorAdded = _constitutorRepository.CreateConstitutor(constitutor, clientIds);
            }
            else
            {
                constitutorAdded = _constitutorRepository.CreateConstitutor(constitutor);
            }

            if (!constitutorAdded)
            {
                ModelState.AddModelError("", "Ошибка во время сохранения");
                return StatusCode(500, ModelState);
            }

            return Ok("Учредитель добавлен");
        }

        [HttpPut("{constitutorId}")]
        public IActionResult UpdateConstitutor(int constitutorId,[FromQuery] ConstitutorDto updatedConstitutor)
        {
            if (updatedConstitutor == null)
                return BadRequest(ModelState);

            //if (constitutorId != updatedConstitutor.Id)
            //    return BadRequest(ModelState);

            if (!_constitutorRepository.ConstitutorExists(constitutorId))
                return NotFound();

            var existingConstitutor = _constitutorRepository.GetConstitutor(constitutorId);

            if (!ModelState.IsValid)
                return BadRequest();

            Constitutor constitutor = new Constitutor
            {
                INN = updatedConstitutor.INN,
                ConstitutorName = updatedConstitutor.ConstitutorName,
                DateAdded = existingConstitutor.DateAdded,
                DateOfUpdate = DateTime.Today
            };

            if (!_constitutorRepository.UpdateConstitutor(constitutor))
            {
                ModelState.AddModelError("", "Ошибка");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{constitutorId}")]
        public IActionResult DeleteConstitutor(int constitutorId)
        {
            if (!_constitutorRepository.ConstitutorExists(constitutorId))
            {
                return NotFound();
            }

            var constitutorToDelete = _constitutorRepository.GetConstitutor(constitutorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_constitutorRepository.DeleteConstitutor(constitutorToDelete))
            {
                ModelState.AddModelError("", "Ошибка");
            }

            return NoContent();
        }
    }
}
