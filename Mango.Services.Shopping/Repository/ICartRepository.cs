using Mango.Services.Shopping.Models.Dtos;

namespace Mango.Services.Shopping.Repository
{
    public interface ICartRepository
    {
        Task<CartDto> GetCartByUserId(string userId);
        Task<CartDto> UpsertCart(CartDto cartDto);
        Task<bool> RemoveFromCart(int cartDetailId);
        Task<bool> ClearCart(string userId);
    }
}
