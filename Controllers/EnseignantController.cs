using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetSynthese.Server.Models;
using ProjetSynthese.Server.Models.Reponse;

namespace ProjetSynthese.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnseignantController : ControllerBase
    {
        private readonly MyDbContext _context;

        public EnseignantController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Enseignant
        [HttpGet]
        public async Task<IActionResult> GetAllEnseignantsAsync()
        {
            try
            {
                var entities = await _context.Enseignants.ToListAsync();
                return Ok(entities);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération des enseignants.");
            }
        }

        // GET: api/Enseignant/5
        [HttpGet("{id}", Name ="GetEnseignant")]
        public async Task<ActionResult<Enseignant>> GetEnseignantAsync(int id)
        {
            try
            {
                var enseignant = await _context.Enseignants.FindAsync(id);

                if (enseignant == null)
                {
                    return NotFound("Aucun enseignant trouvé avec cet ID.");
                }

                return Ok(enseignant);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération de l'enseignant.");
            }
        }

        // PUT: api/Enseignant/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnseignantAsync(int id, Enseignant enseignant)
        {
            if (id != enseignant.Id)
            {
                return BadRequest("L'ID de l'enseignant ne correspond pas.");
            }

            _context.Entry(enseignant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnseignantExists(id))
                {
                    return NotFound("Aucun enseignant trouvé avec cet ID.");
                }
                return StatusCode(500, "Une erreur de concurrence s'est produite lors de la mise à jour de l'enseignant.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la mise à jour de l'enseignant.");
            }

            return NoContent();
        }

        // POST: api/Enseignants
        [HttpPost]
        public async Task<ActionResult<Enseignant>> PostEnseignantAsync(Enseignant enseignant)
        {
            try
            {
                _context.Enseignants.Add(enseignant);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetEnseignant", new { id = enseignant.Id }, enseignant);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de l'ajout de l'enseignant.");
            }
        }

        // DELETE: api/Enseignant/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnseignantAsync(int id)
        {
            try
            {
                var enseignant = await _context.Enseignants.FindAsync(id);
                if (enseignant == null)
                {
                    return NotFound("Aucun enseignant trouvé avec cet ID.");
                }

                _context.Enseignants.Remove(enseignant);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la suppression de l'enseignant.");
            }
        }

        private bool EnseignantExists(int id)
        {
            return _context.Enseignants.Any(e => e.Id == id);
        }

        // GET: api/Enseignant/GroupesEnseignant/5
        [HttpGet("GroupesEnseignant/{id}")]
        public async Task<IActionResult> GetGroupesEnseignantAsync(int id)
        {
            try
            {
                var enseignant = await _context.Enseignants.Include(e => e.GroupeCours).FirstOrDefaultAsync(e => e.Id == id);

                if (enseignant == null)
                {
                    return NotFound("Aucun enseignant trouvé avec cet ID.");
                }

                return Ok(enseignant.GroupeCours);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération des groupes de l'enseignant.");
            }
        }
    }
}
