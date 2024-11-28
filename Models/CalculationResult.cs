namespace ProjetSynthese.Server.Models
{
    public class CalculationResult
    {
        public string Expression { get; set; } = string.Empty;
        public Dictionary<string, float> Symboles { get; set; } = new Dictionary<string, float>();
    }
}
