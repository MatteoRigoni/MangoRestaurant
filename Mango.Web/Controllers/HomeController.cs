using Mango.Web.Models;
using Mango.Web.Models.Dtos;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> products = new();
            var response = await _productService.GetAllProductsAsync<List<ProductDto>>("");
            if (response != null && response.Success)
            {
                products = response.Result;
            }

            return View(products);
        }

        public async Task<IActionResult> Details(int productId)
        {
            ProductDto product = new();
            var response = await _productService.GetAllProductByIdAsync<ProductDto>(productId, "");
            if (response != null && response.Success)
            {
                product = response.Result;
            }

            return View(product);
        }

        [HttpPost]
        [ActionName("Details")]
        [Authorize]
        public async Task<IActionResult> DetailsPost(ProductDto productDto)
        {
            CartDto cartDto = new CartDto()
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
                }
            };

            CartDetailDto cartDetail = new CartDetailDto()
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId
            };

            var respProduct = await _productService.GetAllProductByIdAsync<ProductDto>(productDto.ProductId, "");
            if (respProduct != null && respProduct.Success)
                cartDetail.Product = respProduct.Result;

            List<CartDetailDto> cartDetailsDto = new List<CartDetailDto>();
            cartDetailsDto.Add(cartDetail);

            cartDto.CartDetails = cartDetailsDto;

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var respAddToCart = await _cartService.AddToCartAsync<CartDto>(cartDto, accessToken);
            if (respAddToCart != null && respAddToCart.Success)
                return RedirectToAction(nameof(Index));

            return View(productDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> Login()
        {            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}