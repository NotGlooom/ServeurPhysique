using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetSynthese.Server.Models;

namespace ProjetSynthese.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly MyDbContext _context;

        public NotificationController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetListNotificationsAsync()
        {
            try
            {
                var notifications = await _context.Notifications
                    .Include(n => n.Activite)
                    .ToListAsync();
                return Ok(notifications);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération des notifications.");
            }
        }

        [HttpGet("{id}", Name = "GetNotification")]
        public async Task<ActionResult<Notification>> GetNotificationAsync(int id)
        {
            try
            {
                var notification = await _context.Notifications
                    .FirstOrDefaultAsync(n => n.Id == id);

                if (notification == null)
                {
                    return NotFound("Aucune notification trouvée avec cet id.");
                }

                return Ok(notification);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la récupération de la notification.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNotificationAsync([FromBody] Notification notification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetNotification", new { id = notification.Id }, notification);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Une erreur est survenue lors de l'ajout de la notification.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur inattendue est survenue.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotificationAsync(int id)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(id);
                if (notification == null)
                {
                    return NotFound();
                }

                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Une erreur est survenue lors de la suppression de la notification.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur inattendue est survenue.");
            }
        }
    }
}
