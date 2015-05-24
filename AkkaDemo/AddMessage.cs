namespace AkkaDemo
{
    public class AddMessage
    {
        public int Value { get; private set; }

        public AddMessage(int value)
        {
            this.Value = value;
        }
    }
}
