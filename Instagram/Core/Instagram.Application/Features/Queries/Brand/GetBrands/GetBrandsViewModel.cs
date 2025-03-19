using AutoMapper;
using SNM.Instagram.Application.Interfaces;
using MediatR;
using SNM.Instagram.Application.DTO;

namespace SNM.Instagram.Application.Features.Queries.Brand.GetBrands

{
    public class GetBrandsViewModel
    {
        public GetBrandsViewModel(Guid id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }

        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }

}
