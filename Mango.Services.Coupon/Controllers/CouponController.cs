using Mango.Services.Coupon.Models.Dtos;
using Mango.Services.Coupon.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.Coupon.Controllers
{
    [ApiController]
    [Route("api/coupon")]
    public class CouponController : Controller
    {
        private readonly ICouponRepository _repository;

        public CouponController(ICouponRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<ResponseDto<CouponDto>>> GetCart(string code)
        {
            ResponseDto<CouponDto> res = new ResponseDto<CouponDto>();

            try
            {
                CouponDto couponDto = await _repository.GetCouponByCode(code);
                res.Result = couponDto;
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
