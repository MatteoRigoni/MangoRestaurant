using Mango.Services.Products.Models.Dtos;
using Mango.Services.Products.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }
                
        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<ResponseDto<List<ProductDto>>>> Get()
        {
            ResponseDto<object> res = new ResponseDto<object>();

            try
            {
                var productDtos = await _repository.GetProducts();
                res.Result = productDtos;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Errors = new List<string>() { ex.ToString() };
            }

            return Ok(res);
        }

        [HttpGet]
        //[Authorize]
        [Route("{id}")]
        public async Task<ActionResult<ResponseDto<ProductDto>>> Get(int id)
        {
            ResponseDto<ProductDto> res = new ResponseDto<ProductDto>();

            try
            {
                var productDto = await _repository.GetProductById(id);
                if (productDto == null) return NotFound();
                res.Result = productDto;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Errors = new List<string>() { ex.ToString() };
            }

            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ProductDto>>> Create([FromBody] ProductDto productDto)
        {
            ResponseDto<ProductDto> res = new ResponseDto<ProductDto>();

            try
            {
                var model = await _repository.UpsertProduct(productDto);
                res.Result = model;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Errors = new List<string>() { ex.ToString() };
            }

            return Ok(res);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<ProductDto>>> Update([FromBody] ProductDto productDto)
        {
            ResponseDto<ProductDto> res = new ResponseDto<ProductDto>();  

            try
            {
                var model = await _repository.UpsertProduct(productDto);
                res.Result = model;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Errors = new List<string>() { ex.ToString() };
            }

            return Ok(res);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        public async Task<ActionResult<ResponseDto<bool>>> Delete(int id)
        {
            ResponseDto<bool> res = new ResponseDto<bool>();

            try
            {
                res.Result = await _repository.DeleteProduct(id);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Errors = new List<string>() { ex.ToString() };
            }

            return Ok(res);
        }
    }
}
