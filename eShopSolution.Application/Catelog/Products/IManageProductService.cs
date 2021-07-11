using eShopSolution.Application.Catelog.Products.Dtos;
using eShopSolution.Application.Catelog.Products.Dtos.Manage;
using eShopSolution.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catelog.Products
{
    public interface IManageProductService
    {
        Task<int> Create(ProductCteateRepuest repuest);

        Task<int> Update(ProductUpdateRequest request);

        Task<bool> UpdatePrice(int productId, decimal newPrice);

        Task<bool> UpdateStook(int productId, int addedQuantity);

        Task AddViewcount(int productId);

        Task<int> Delete(int productId);

        Task<PageResult<ProductViewModel>> GetAllPaging(GetProducPagingRequest request);
    }
}
