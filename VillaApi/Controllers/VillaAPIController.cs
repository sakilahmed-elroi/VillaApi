using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.AccessControl;
using VillaApi.Model.VillaDTO;
using VillaApi.Data;
using VillaApi.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
namespace VillaApi.Controllers
   
{
    [Route("api/[controller]")]
    [ApiController]   
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;
        private readonly ApplicationDbContext _db;
        public VillaAPIController(ILogger<VillaAPIController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _db = dbContext;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult< IEnumerable<VillaDTO>>  GetVillas()
        {
            _logger.LogInformation("Getting all villas");
            _db.villas.ToList();
            return Ok(_db.villas.ToList());

        }
        [HttpGet("id:int", Name ="getvilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Get Villa Error with Id " + id);
                return BadRequest("Invalid ID");
            }
            else 
            {
                var gtVilla = _db.villas.FirstOrDefault(x => x.Id == id);
                if (gtVilla == null)
                {
                    _logger.LogError("Get Villa Error with Id " + id);
                    return NotFound("No Data Found");
                }
                _logger.LogInformation("Getting Villa with Id " + id);
                return Ok(gtVilla);
            }

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult CreateVilla([FromBody] VillaDTO villa)
        {

                if(_db.villas.FirstOrDefault(x => x.Name.ToLower() == villa.Name.ToLower())!= null)
                {
                    _logger.LogError("Villa Name already exists");
                    ModelState.AddModelError("custom Name Validation", "Name Already Exists");
                    return BadRequest(ModelState);
                }
                else
                {
                    _logger.LogInformation("Creating Villa with Name " + villa.Name); 
                    if(villa == null)
                    {
                        return BadRequest(villa);
                    }                   
                    Villa model = new()
                    {
                        Name= villa.Name,
                        VillaType = villa.VillaType,
                        Rate = villa.Rate,
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now
                    };
                
                    _db.villas.Add(model);
                    _db.SaveChanges();
                return CreatedAtRoute("getvilla", model);
                }
                               
                 

        }

        [HttpDelete("id:int", Name ="DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteVilla(int id)
        {
            if(id > 0)
            {
                var dtvilla = (_db.villas.FirstOrDefault(x => x.Id == id));
                if (dtvilla != null)
                {
                    _logger.LogInformation("Deleting Villa with Id " + id);
                    _db.villas.Remove(dtvilla);
                    _db.SaveChanges();
                    return NoContent();
                }
                else
                {
                    _logger.LogError("Villa not found with Id " + id);
                    return NotFound("Id not found");
                }
                    

            }
            else
            {
                _logger.LogError("Invalid Id " + id);
                return BadRequest("Invalid Id");
            }
                
        }

        [HttpPut("id:int", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaDTO villa)
        {
            if(villa  != null &&  id != villa.Id)
            {
                _logger.LogError("In Valid Data for Id " + id);
                return BadRequest("In Valid Data");
            }
            var updvilla = _db.villas.FirstOrDefault(x => x.Id == id);
            if (updvilla != null)
            {

                updvilla.Name = villa.Name;
                updvilla.VillaType = villa.VillaType;
                updvilla.Rate = villa.Rate;
                updvilla.LastModifiedDate = DateTime.Now;               
                
                _db.SaveChanges();
                return NoContent();
            }
            else
            {
                _logger.LogError("Villa not found with Id " + id);
                return BadRequest();
            }
               
        }
        [HttpPatch("id:int", Name = "PatchUpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PatchUpdateVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if(patchDTO  != null &&  id == 0)
            {
                _logger.LogError("In Valid Data for Id " + id);
                return BadRequest("In Valid Data");
            }
            var updvilla = _db.villas.AsNoTracking().FirstOrDefault(x => x.Id == id);
            VillaDTO villadto = new()
            {               
                Name = updvilla.Name,
                VillaType = updvilla.VillaType,
                Rate = updvilla.Rate
            };
            if (updvilla == null)
            {
                _logger.LogError("Villa is Null");
                return BadRequest();
            }
            patchDTO.ApplyTo(villadto, ModelState);
            Villa villa = new()
            {
                Id = updvilla.Id,
                Name = villadto.Name,
                VillaType = villadto.VillaType,
                Rate = villadto.Rate,
                CreatedDate = updvilla.CreatedDate,
                LastModifiedDate = DateTime.Now
            };
            _db.Update(villa);
            _db.SaveChanges();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();

        }

       
    }


}
