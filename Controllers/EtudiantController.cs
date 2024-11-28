using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetSynthese.Server.Models;

namespace ProjetSynthese.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EtudiantController : ControllerBase
    {
        private readonly MyDbContext _context;

        public EtudiantController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEtudiantAsync()
        {
            try
            {
                var entities = await _context.Etudiants.ToListAsync();
                return Ok(entities);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération des étudiants.");
            }
        }

        // GET: api/Etudiant/5
        [HttpGet("{id}", Name ="GetEtudiant")]
        public async Task<ActionResult<Etudiant>> GetEtudiantAsync(int id)
        {
            try
            {
                var etudiant = await _context.Etudiants.Include(e => e.Cours).FirstOrDefaultAsync(e => e.Id == id);

                if (etudiant == null)
                {
                    return NotFound("Aucun étudiant trouvé avec cet ID.");
                }

                return Ok(etudiant);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération de l'étudiant.");
            }
        }

        // PUT: api/Etudiant/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEtudiantAsync(int id, Etudiant etudiant)
        {
            if (id != etudiant.Id)
            {
                return BadRequest("L'ID de l'étudiant ne correspond pas.");
            }

            _context.Entry(etudiant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EtudiantExists(id))
                {
                    return NotFound("Aucun étudiant trouvé avec cet ID.");
                }
                return StatusCode(500, "Une erreur de concurrence s'est produite lors de la mise à jour de l'étudiant.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la mise à jour de l'étudiant.");
            }

            return NoContent();
        }

        // POST: api/Etudiants
        [HttpPost]
        public async Task<ActionResult<Etudiant>> PostEtudiantAsync(Etudiant etudiant)
        {
            try
            {
                _context.Etudiants.Add(etudiant);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetEtudiant", new { id = etudiant.Id }, etudiant);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de l'ajout de l'étudiant.");
            }
        }

        // DELETE: api/Etudiant/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEtudiantAsync(int id)
        {
            try
            {
                var etudiant = await _context.Etudiants.FindAsync(id);
                if (etudiant == null)
                {
                    return NotFound("Aucun étudiant trouvé avec cet ID.");
                }

                _context.Etudiants.Remove(etudiant);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la suppression de l'étudiant.");
            }
        }

        private bool EtudiantExists(int id)
        {
            return _context.Etudiants.Any(e => e.Id == id);
        }

        // GET: api/Etudiant/Groupesetudiant/5
        [HttpGet("Groupesetudiant/{id}")]
        public async Task<IActionResult> GetGroupesEtudiantAsync(int id)
        {
            try
            {
                var etudiant = await _context.Etudiants.Include(e => e.Cours).FirstOrDefaultAsync(e => e.Id == id);

                if (etudiant == null)
                {
                    return NotFound("Aucun étudiant trouvé avec cet ID.");
                }

                return Ok(etudiant.Cours);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération des cours de l'étudiant.");
            }
        }
    }
}
