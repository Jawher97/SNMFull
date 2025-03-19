using MediatR;
using SNM.Instagram.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Instagram.Application.Features.Queries.Brand.GetBrands
{
    public class GetBrandsQuery : IRequest<List<BrandDto>>
    {
    }
}
