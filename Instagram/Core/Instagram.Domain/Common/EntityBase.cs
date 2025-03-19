namespace SNM.Instagram.Domain.Common
{
    public abstract class EntityBase<Guid>
    {
        protected EntityBase() { }
        protected EntityBase(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}