using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;
using API.Helpers;

namespace API.Controllers
{
        public class ProductsController: BaseApiController
    {
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IMapper _mapper;
        
        public ProductsController(IGenericRepository<Product> productsRepo,
        IGenericRepository<ProductBrand> productBrandRepo, 
        IGenericRepository<ProductType> productTypeRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _productsRepo = productsRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams productparams)
        {
            var spec= new ProductsWithTypesAndBrandsSpecification(productparams);
            var countSpec = new ProductWithFiltersForCountSpecificication(productparams);
            var totalItems= await _productsRepo.CountAsync(countSpec);
            var products= await _productsRepo.ListAsync(spec);
            var data = _mapper
                .Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(products);

            
            
            return Ok(new Pagination<ProductToReturnDto>(productparams.PageIndex, productparams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec= new ProductsWithTypesAndBrandsSpecification(id);

            var product= await _productsRepo.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]

        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        } 

            [HttpGet("types")]

        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProducttypes()
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        } 
    }
}