namespace ProjetSynthese.Server.Helpers
{
    public static class PythonHelper
    {
        /**
         * Retourne la commande ou le chemin pour executer python
         */
        public static string GetPythonPath(IConfiguration configuration)
        {
            var pythonPath = configuration["PythonSettings:PythonPath"];

            // Verifie si le chemin vers python et le fichier existe
            if (!string.IsNullOrEmpty(pythonPath) && File.Exists(pythonPath))
            {
                return pythonPath;
            }

            // Sinon retourne python
            return "python";
        }
    }
}
