using eShopSolution.Application.Catelog.Products.Dtos;
using eShopSolution.Application.Catelog.Products.Dtos.Public;
using eShopSolution.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catelog.Products
{
    public interface IPublicProductService
    {
      Task<PageResult<ProductViewModel>> GetAllByCaterogyId(GetProducPagingRequest request);
    }
}
