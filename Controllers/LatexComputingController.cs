using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetSynthese.Server.Services;
using System.Text.Json;

namespace ProjetSynthese.Server.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Authorize]
    public class LatexComputingController : Controller
    {
        private readonly ILatexService _latexService;

        public LatexComputingController(ILatexService latexService)
        {
            _latexService = latexService;
        }

        /**
           {
                "Expression": "\\sqrt{\\frac{v^2}{g}}",
                "Symboles": {"v": 20, "g": 9.81}
            }
        **/
        [HttpGet]
        public async Task<IActionResult> CalculateLatex(string json)
        {
            try
            {
                if (string.IsNullOrEmpty(json) || !TryParseAndValidateJson(json))
                {
                    return BadRequest("Exception: Json invalide");
                }

                var output = await _latexService.CalculateLatexFromJson(json);
                return Content(output);
            }
            catch(FormatException ex)
            {
                // Handle latex invalide
                return BadRequest(new { message = $"Exception: {ex.Message}", errorType = "LatexError" });
            }
            catch (ArgumentException ex)
            {
                // Handle variables invalide
                return BadRequest(new { message = $"Exception: {ex.Message}", errorType = "SymbolError" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Exception: {ex.Message}");
            }
        }

        /**
         * Essaie de parse et valide si le json contient toutes les informations
         */
        private static bool TryParseAndValidateJson(string json)
        {
            try
            {
                var jsonDocument = JsonDocument.Parse(json);
                var root = jsonDocument.RootElement;

                // Validate la structure
                if (!root.TryGetProperty("Expression", out _))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}