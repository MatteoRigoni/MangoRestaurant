using Mango.Services.Shopping.Models.Dtos;
using Mango.Services.Shopping.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.Shopping.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : Controller
    {
        private readonly ICartRepository _repository;

        public CartController(ICartRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ActionResult<ResponseDto<CartDto>>> GetCart(string userId)
        {
            ResponseDto<CartDto> res = new ResponseDto<CartDto>();

            try
            {
                CartDto cartDto = await _repository.GetCartByUserId(userId);
                res.Result = cartDto;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Errors = new List<string>() { ex.Message };
            }

            return res;
        }

        [HttpPost("AddCart")]
        public async Task<ActionResult<ResponseDto<CartDto>>> AddCart([FromBody] CartDto cartDto)
        {
            ResponseDto<CartDto> res = new ResponseDto<CartDto>();

            try
            {
                CartDto resCartDto = await _repository.UpsertCart(cartDto);
                res.Result = resCartDto;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Errors = new List<string>() { ex.Message };
            }

            return res;
        }

        [HttpPost("UpdateCart")]
        public async Task<ActionResult<ResponseDto<CartDto>>> UpdateCart([FromBody] CartDto cartDto)
        {
            ResponseDto<CartDto> res = new ResponseDto<CartDto>();

            try
            {
                CartDto resCartDto = await _repository.UpsertCart(cartDto);
                res.Result = resCartDto;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Errors = new List<string>() { ex.Message };
            }

            return res;
        }

        [HttpDelete("DeleteCart")]
        public async Task<ActionResult<ResponseDto<bool>>> RemoveCart([FromBody] int cartId)
        {
            ResponseDto<bool> res = new ResponseDto<bool>();

            try
            {
                bool success = await _repository.RemoveFromCart(cartId);
                res.Result = success;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Errors = new List<string>() { ex.Message };
            }

            return res;
        }
    }
}
