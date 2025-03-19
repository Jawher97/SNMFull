namespace SNM.Publishing.Aggregator.Models.Common
{
    public abstract class EntityBase<Tid>
    {
        protected EntityBase() { }
        protected EntityBase(Tid id)
        {
            Id = id;
        }

        public Tid Id { get; set; }
    }
}