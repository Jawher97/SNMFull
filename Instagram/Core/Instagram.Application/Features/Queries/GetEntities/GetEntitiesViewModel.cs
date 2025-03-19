namespace SNM.Instagram.Application.Features.Queries.GetEntities
{
    public class GetEntitiesViewModel
    {
        public GetEntitiesViewModel(Guid id, string message)
        {
            Id = id;
            Message = message;
        }

        public Guid Id { get; set; }
        public string Message { get; set; }
    }
}