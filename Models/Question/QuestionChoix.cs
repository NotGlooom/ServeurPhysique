namespace ProjetSynthese.Server.Models.Question
{
    public class QuestionChoix: QuestionBase
    {
        public QuestionChoix() { }

        public override object Clone()
        {
            QuestionChoix clone = (QuestionChoix)CloneBase();
            return clone;
        }
    }
}
