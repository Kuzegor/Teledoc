using Microsoft.AspNetCore.Mvc;
using Teledoc.Dto;
using Teledock.Interface;
using Teledock.Models;
using Teledock.Repository;

namespace Teledock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : Controller
    {
        private readonly IClientRepository _clientRepository;
        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpGet]
        public IActionResult GetClients()
        {
            var clients = _clientRepository.GetClients();

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            return Ok(clients);
        }

        [HttpGet("{clientId}")]
        public IActionResult GetClient(int clientId)
        {
            if (_clientRepository.ClientExists(clientId) == false)
                return NotFound();

            var client = _clientRepository.GetClient(clientId);

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            return Ok(client);
        }

        [HttpGet("constitutors/{clientId}")]
        public IActionResult GetConstitutorsByClient(int clientId)
        {
            var constitutors = _clientRepository.GetConstitutorsByClient(clientId);

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            return Ok(constitutors);
        }

        [HttpPost]
        public IActionResult CreateClient([FromQuery]ClientDto clientDto, [FromQuery] int[]? constitutorIds)
        {
            if (clientDto == null)
                return BadRequest(ModelState);

            var existingClient = _clientRepository.GetClients()
                .Where(c => c.ClientName.Trim().ToUpper() == clientDto.ClientName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (existingClient != null)
            {
                ModelState.AddModelError("", "Клиент с этим имененм уже есть в базе");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Client client = new Client
            {
                INN = clientDto.INN,
                ClientName = clientDto.ClientName,
                ClientType = clientDto.ClientType,
                DateAdded = DateTime.Today,
                DateOfUpdate = DateTime.Today
            };

            bool clientAdded;
            if (constitutorIds != null)
            {
                clientAdded = _clientRepository.CreateClient(client, constitutorIds);
            }
            else
            {
                clientAdded = _clientRepository.CreateClient(client);
            }

            if (!clientAdded)
            {
                ModelState.AddModelError("", "Ошибка во время сохранения");
                return StatusCode(500, ModelState);
            }

            return Ok("Клиент добавлен");
        }

        [HttpPut("{clientId}")]
        public IActionResult UpdateClient(int clientId,[FromQuery] ClientDto updatedClient)
        {
            if (updatedClient == null)
                return BadRequest(ModelState);

            //if (clientId != updatedClient.Id)
            //    return BadRequest(ModelState);

            if (!_clientRepository.ClientExists(clientId))
                return NotFound();

            var existingClient = _clientRepository.GetClient(clientId);

            if (!ModelState.IsValid)
                return BadRequest();

            Client client = new Client
            {
                INN = updatedClient.INN,
                ClientName = updatedClient.ClientName,
                ClientType = updatedClient.ClientType,
                DateAdded = existingClient.DateAdded,
                DateOfUpdate = DateTime.Today
            };

            if (!_clientRepository.UpdateClient(client))
            {
                ModelState.AddModelError("", "Ошибка");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{clientId}")]
        public IActionResult DeleteConstitutor(int clientId)
        {
            if (!_clientRepository.ClientExists(clientId))
            {
                return NotFound();
            }

            var clientToDelete = _clientRepository.GetClient(clientId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_clientRepository.DeleteClient(clientToDelete))
            {
                ModelState.AddModelError("", "Ошибка");
            }

            return NoContent();
        }
    }
}
