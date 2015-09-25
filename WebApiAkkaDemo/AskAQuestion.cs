namespace WebApiAkkaDemo
{
	public class AskAQuestion
	{
        public string Question { get; private set; }

        public AskAQuestion(string question)
        {
            Question = question;
        }
	}
}
