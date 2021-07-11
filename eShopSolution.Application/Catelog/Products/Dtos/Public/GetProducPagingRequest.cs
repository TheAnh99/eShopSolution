using eShopSolution.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Application.Catelog.Products.Dtos.Public
{
    public class GetProducPagingRequest : PagingRequestBase
    { 
        public int? CategoryId { get; set; }
    }
}
