using Mango.Web.Models.Dtos;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICouponService _couponService;

        public CartController(ICartService cartService, ICouponService couponService)
        {
            _cartService = cartService;
            _couponService = couponService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await this.LoadCartDtoBasedOnLoggedUser());
        }

        public async Task<IActionResult> Checkout()
        {
            return View(await this.LoadCartDtoBasedOnLoggedUser());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await _cartService.CheckoutAsync<object>(cartDto.CartHeader, accessToken);
                if (!response.Success)
                {
                    TempData["Error"] = response.DisplayMessage;
                    return RedirectToAction(nameof(Checkout));
                }

                return RedirectToAction(nameof(Confirmation));
            }
            catch (Exception ex)
            {
                return View(cartDto);
            }

            return View(await this.LoadCartDtoBasedOnLoggedUser());
        }

        public async Task<IActionResult> Confirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.ApplyCouponAsync<CouponDto>(cartDto, accessToken);
            if (response != null && response.Success)
                return RedirectToAction(nameof(Index));

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.RemoveCouponAsync<CouponDto>(cartDto.CartHeader.UserId, accessToken);
            if (response != null && response.Success)
                return RedirectToAction(nameof(Index));

            return View();
        }

        public async Task<IActionResult> Remove(int cartDetailId)
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.RemoveFromCartAsync<CartDto>(cartDetailId, accessToken);
            if (response != null && response.Success)
                return RedirectToAction(nameof(Index));

            return View();
        }

        private async Task<CartDto> LoadCartDtoBasedOnLoggedUser()
        {
            CartDto result = new CartDto();

            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.GetCartByUserIdAsync<CartDto>(userId, accessToken);
            if (response != null && response.Success)
                result = response.Result;

            if (result.CartHeader != null)
            {
                if (!string.IsNullOrEmpty(result.CartHeader.CouponCode))
                {
                    var coupon = await _couponService.GetCouponAsync<CouponDto>(result.CartHeader.CouponCode, accessToken);
                    if (coupon != null && coupon.Success)
                    {
                        result.CartHeader.DiscountTotal = coupon.Result.DiscountAmount;
                    }
                }

                foreach (var detail in result.CartDetails)
                {
                    result.CartHeader.OrderTotal += (detail.Count * detail.Product.Price);
                }

                result.CartHeader.OrderTotal -= result.CartHeader.DiscountTotal;
            }
            return result;
        }
    }
}
