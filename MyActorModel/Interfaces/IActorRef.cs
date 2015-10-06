namespace MyActorModel
{
    public interface IActorRef
    {
        ActorPath Path { get; set; }

        void Tell(object message);
        void Ask(object message);
    }
}
