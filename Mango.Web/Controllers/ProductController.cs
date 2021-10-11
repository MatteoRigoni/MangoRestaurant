using Mango.Web.Models.Dtos;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            List<ProductDto> products = new();
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var res = await _productService.GetAllProductsAsync<List<ProductDto>>(accessToken);
            if (res != null && res.Success)
                products = res.Result;
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var res = await _productService.CreateProductAsync<ProductDto>(model, accessToken);
                if (res != null && res.Success)
                    return RedirectToAction(nameof(Index));                
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int productId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var res = await _productService.GetAllProductByIdAsync<ProductDto>(productId, accessToken);
            if (res != null && res.Success)
                return View(res.Result);
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var res = await _productService.UpdateProductAsync<ProductDto>(model, accessToken);
                if (res != null && res.Success)
                    return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int productId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var res = await _productService.GetAllProductByIdAsync<ProductDto>(productId, accessToken);
            if (res != null && res.Success)
                return View(res.Result);
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var res = await _productService.DeleteProductAsync<ProductDto>(model.ProductId, accessToken);
                if (res != null && res.Success)
                    return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}
