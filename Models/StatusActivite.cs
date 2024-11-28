namespace ProjetSynthese.Server.Models
{
    public class StatusActivite
    {
        public int ActiviteId { get; set; }
        public int NbQuestionReussi { get; set; }
        public int NbQuestionTotal { get; set; }
        public InstanceStatus ActiviteStatus { get; set; }
        public List<StatusExercice> ExercicesStatus { get; set; } = new List<StatusExercice>();
    }

    public class StatusExercice
    {
        public int ExerciceId { get; set; }
        public InstanceStatus ExerciceStatus { get; set; }
        public int NbQuestionReussi { get; set; }
        public int NbQuestionTotal { get; set; }
    }
}
