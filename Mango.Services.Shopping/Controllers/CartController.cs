using Mango.MessageBus;
using Mango.Services.Shopping.Messages;
using Mango.Services.Shopping.Models.Dtos;
using Mango.Services.Shopping.Repository;
using Mango.Services.Shopping.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.Shopping.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : Controller
    {
        private readonly ICartRepository _repository;
        private readonly IMessageBus _messageBus;
        private readonly ICouponRepository _couponRepository;

        public CartController(ICartRepository repository, IMessageBus messageBus, ICouponRepository couponRepository)
        {
            _repository = repository;
            _messageBus = messageBus;
            _couponRepository = couponRepository;
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

        [HttpDelete("RemoveCart")]
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

        [HttpDelete("ApplyCoupon")]
        public async Task<ActionResult<ResponseDto<bool>>> ApplyCoupon([FromBody] CartDto cartDto)
        {
            ResponseDto<bool> res = new ResponseDto<bool>();

            try
            {
                bool success = await _repository.ApplyCoupon(cartDto.CartHeader.UserId, cartDto.CartHeader.CouponCode);
                res.Result = success;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Errors = new List<string>() { ex.Message };
            }

            return res;
        }

        [HttpDelete("RemoveCoupon")]
        public async Task<ActionResult<ResponseDto<bool>>> RemoveCoupon([FromBody] string userId)
        {
            ResponseDto<bool> res = new ResponseDto<bool>();

            try
            {
                bool success = await _repository.RemoveCoupon(userId);
                res.Result = success;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Errors = new List<string>() { ex.Message };
            }

            return res;
        }

        [HttpPost("Checkout")]
        public async Task<ActionResult<ResponseDto<object>>> Checkout([FromBody] CheckoutHederDto checkoutHeaderDto)
        {
            ResponseDto<object> res = new ResponseDto<object>();

            try
            {
                CartDto resCartDto = await _repository.GetCartByUserId(checkoutHeaderDto.UserId);
                if (resCartDto == null) return BadRequest();

                if (!string.IsNullOrEmpty(checkoutHeaderDto.CouponCode))
                {
                    var coupon = await _couponRepository.GetCoupon(checkoutHeaderDto.CouponCode);
                    if (checkoutHeaderDto.DiscountTotal != coupon.DiscountAmount)
                    {
                        res.Success = false;
                        res.Errors = new List<string>() { "Coupon price has changed, please confirm" };
                        return res;
                    }
                }

                checkoutHeaderDto.CartDetails = resCartDto.CartDetails;

                // send message to process order...
                await _messageBus.PublishMessage(checkoutHeaderDto, "checkoutmessagetopic");
                await _repository.ClearCart(checkoutHeaderDto.UserId);
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
