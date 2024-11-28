using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetSynthese.Server.Models.Question;
using System.Text.Json;

namespace ProjetSynthese.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly MyDbContext _context;

        public QuestionController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetListeQuestions")]
        public async Task<IActionResult> GetListQuestionsAsync()
        {
            try
            {
                var questions = await _context.Questions.ToListAsync();

                if (questions == null || questions.Count == 0)
                {
                    return NotFound("Aucune question trouvée.");
                }

                return Ok(questions);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération des questions.");
            }
        }

        [HttpGet("{id}", Name ="GetQuestion")]
        public async Task<ActionResult<QuestionBase>> GetQuestionAsync(int id)
        {
            try
            {
                var question = await _context.Questions.FindAsync(id);

                if (question == null)
                {
                    return NotFound("Aucune question trouvée avec cet ID.");
                }

                return Ok(question);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération de la question.");
            }
        }

        private async Task<bool> QuestionExistsAsync(int id)
        {
            return await _context.Questions.AnyAsync(q => q.Id == id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ModifierQuestionAsync(int id, QuestionBase question)
        {
            if (id != question.Id)
            {
                return BadRequest("L'ID de la question ne correspond pas.");
            }

            try
            {
                var questionExistante = await _context.Questions.FindAsync(id);
                if (questionExistante == null)
                {
                    return NotFound("Aucune question trouvée avec cet ID.");
                }

                // Mettre à jour les propriétés de la question existante
                questionExistante.Enonce = question.Enonce;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Une erreur est survenue lors de la mise à jour de la question.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur inattendue est survenue.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<QuestionBase>> AjouterQuestionAsync(QuestionBase question)
        {
            try
            {
                if (await QuestionExistsAsync(question.Id))
                {
                    return Conflict("Une question avec cet ID existe déjà.");
                }

                _context.Questions.Add(question);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetQuestion", new { id = question.Id }, question);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Une erreur est survenue lors de l'ajout de la question.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur inattendue est survenue.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SupprimerQuestionAsync(int id)
        {
            try
            {
                var question = await _context.Questions.FindAsync(id);
                if (question == null)
                {
                    return NotFound("Aucune question trouvée avec cet ID.");
                }

                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Une erreur est survenue lors de la suppression de la question.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur inattendue est survenue.");
            }
        }

        [HttpGet("indice/{id}")]
        public async Task<ActionResult<QuestionBase>> GetIndicesByQuestionID(int id)
        {
            try
            {
                var question = await _context.Questions
                    .Include(q => q.Indices)
                    .FirstOrDefaultAsync(q => q.Id == id); 

                if (question == null)
                {
                    return NotFound("Aucune question trouvée avec cet ID.");
                }

                return Ok(question);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Une erreur est survenue lors de la récupération de la question.");
            }
        }

    }
}
