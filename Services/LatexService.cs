using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using ProjetSynthese.Server.Helpers;
using ProjetSynthese.Server.Models;

namespace ProjetSynthese.Server.Services
{
    public interface ILatexService
    {
        Task<string> CalculateLatex(string expression, List<string> nomSymboles, List<float> valeurSymboles);
        Task<string> CalculateLatexFromJson(string json);
    }

    public class LatexService : ILatexService
    {
        private readonly IConfiguration _configuration;

        public LatexService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual async Task<string> CalculateLatex(string expression, List<string> nomSymboles, List<float> valeurSymboles)
        {
            // Verification si null
            if (string.IsNullOrWhiteSpace(expression) || nomSymboles == null || valeurSymboles == null)
            {
                throw new ArgumentException("Input cannot be null or empty.");
            }

            // Crée le dictionnaire de symboles et leur valeurs
            var symboles = new Dictionary<string, float>();
            for (int i = 0; i < nomSymboles.Count; i++)
            {
                if (i < valeurSymboles.Count) // Pour éviter le out of range
                {
                    symboles[nomSymboles[i]] = valeurSymboles[i];
                }
            }

            // Crée l'objet à sérialiser
            var result = new CalculationResult
            {
                Expression = expression,
                Symboles = symboles
            };

            // Sérialise l'objet
            string json = JsonSerializer.Serialize(result);

            // Calcul le latex
            try
            {
                return await CalculateLatexFromJson(json);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual async Task<string> CalculateLatexFromJson(string json)
        {
            // Set les paths
            string pythonPath = PythonHelper.GetPythonPath(_configuration);
            string scriptPath = @"./Scripts/calculateLatex.py";

            // Encode le json en base64
            string base64Json = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

            // Process start info pour le script
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = $"\"{scriptPath}\" \"{base64Json}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            try
            {
                using (Process process = Process.Start(psi))
                {
                    if (process == null)
                    {
                        throw new InvalidOperationException("Failed to start the process.");
                    }

                    // Lis les stream async
                    Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
                    Task<string> errorTask = process.StandardError.ReadToEndAsync();

                    // Attend pour la fin du process
                    process.WaitForExit();

                    // Attend que les tâches finisent
                    string output = await outputTask;
                    string error = await errorTask;

                    if (!string.IsNullOrEmpty(error))
                    {
                        throw new FormatException("Format latex invalide");
                    }

                    // Essaie de parse en float pour vérifie la validité du retour
                    bool isValid = double.TryParse(output,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out double outputFloat);

                    if (!isValid)
                    {
                        throw new ArgumentException("Symboles invalide");
                    }

                    return output;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
