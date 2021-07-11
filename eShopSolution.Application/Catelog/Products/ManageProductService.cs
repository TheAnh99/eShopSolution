using eShopSolution.Application.Catelog.Products.Dtos.Manage;
using eShopSolution.Application.Dtos;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using static eShopSolution.Utilities.Exceptions.EShopExeption;
using Microsoft.EntityFrameworkCore;
using eShopSolution.Application.Catelog.Products.Dtos;

namespace eShopSolution.Application.Catelog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly EShopDbContext _context;
        public ManageProductService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task AddViewcount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
            
        }

        public async Task<int> Create(ProductCteateRepuest repuest)
        {
            var product = new Product()
            {
                Price = repuest.Price,
                OriginalPrice = repuest.OriginalPrice,
                Stock = repuest.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>
                {
                    new ProductTranslation()
                    {
                        Name = repuest.Name,
                        Description = repuest.Description,
                        Details = repuest.Details,
                        SeoDescription = repuest.SeoDescription,
                        SeoAlias = repuest.SeoAlias,
                        SeoTitle = repuest.SeoTitle,
                        LanguageId = repuest.LanguageId,
                    }
                }
            };
            _context.Products.Add(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int productID)
        {
            var product = await _context.Products.FindAsync(productID);
            if (product == null) throw new EShopException($"Cannot find a product: {productID}");
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
               

        }

        public async Task<PageResult<ProductViewModel>> GetAllPaging(GetProducPagingRequest request)
        {
            //1. Select jont
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };
            //2. Filler
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            if(request.CategoryIds.Count > 0)
            {
                query = query.Where(p => request.CategoryIds.Contains(p.pic.CategoryId));
            }
            //3. Paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount =x.p.ViewCount
                }).ToListAsync();
            //4. 
            var pagedResult = new PageResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pagedResult;
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslations = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id
            && x.LanguageId == request.LanguageId);
            if(product == null || productTranslations == null) throw new EShopException($"Cannot find a product with id: {request.Id}");

            productTranslations.Name = request.Name;
            productTranslations.SeoAlias = request.SeoAlias;
            productTranslations.SeoDescription = request.SeoDescription;
            productTranslations.Description = request.Description;
            productTranslations.Description = request.Description;
            return await _context.SaveChangesAsync();

        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {productId}");

            product.Price = newPrice;
           return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStook(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {productId}");

            product.Stock = addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
