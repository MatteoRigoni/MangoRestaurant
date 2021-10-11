using AutoMapper;
using Mango.Services.Shopping.Models;
using Mango.Services.Shopping.Models.Dtos;
using System.Data.Entity;

namespace Mango.Services.Shopping.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CartRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> ClearCart(string userId)
        {
            var cartHeaderDb = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cartHeaderDb != null)
            {
                _context.CartDetails
                    .RemoveRange(_context.CartDetails.Where(d => d.CartHeaderId == cartHeaderDb.CartHeaderId));
                _context.CartHeaders.Remove(cartHeaderDb);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<CartDto> GetCartByUserId(string userId)
        {
            Cart cart = new()
            {
                CartHeader = await _context.CartHeaders
                .FirstOrDefaultAsync(c => c.UserId == userId)
            };

            cart.CartDetails = _context.CartDetails
                .Where(d => d.CartHeaderId == cart.CartHeader.CartHeaderId)
                .Include(d => d.Product);

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<bool> RemoveFromCart(int cartDetailId)
        {
            CartDetail cartDetails = await _context.CartDetails
                .FirstOrDefaultAsync(d => d.CartDetailId == cartDetailId);

            int totalCountOfCartItems = _context.CartDetails
                .Where(d => d.CartHeaderId == cartDetails.CartHeaderId).Count();

            _context.CartDetails.Remove(cartDetails);
            if (totalCountOfCartItems == 1)
            {
                var cartHeaderToRemove = await _context.CartHeaders
                    .FirstOrDefaultAsync(c => c.CartHeaderId == cartDetails.CartHeaderId);
                _context.CartHeaders.Remove(cartHeaderToRemove);
            }

            _context.SaveChangesAsync();
            return true;
        }

        public async Task<CartDto> UpsertCart(CartDto cartDto)
        {
            Cart cart = _mapper.Map<CartDto, Cart>(cartDto);

            // Insert product of unique cart detail if not exists...
            var prodDb = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == cartDto.CartDetails.FirstOrDefault().ProductId);

            if (prodDb == null)
            {
                _context.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                _context.SaveChangesAsync();
            }

            var cartHeaderDb = await _context.CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == cartDto.CartHeader.UserId);

            if (cartHeaderDb == null)
            {
                // Adding cart mode...
                _context.CartHeaders.Add(cart.CartHeader);
                await _context.SaveChangesAsync();

                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;
                cart.CartDetails.FirstOrDefault().Product = null;
                _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _context.SaveChangesAsync();
            }
            else
            {
                // Update cart mode...
                var cartDetailDb = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    d => cart.CartDetails.FirstOrDefault().ProductId == d.ProductId 
                    && cartHeaderDb.CartHeaderId == d.CartHeaderId);

                if (cartDetailDb == null)
                {
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderDb.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
                else
                {
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetailDb.Count;
                    _context.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
            }

            return _mapper.Map<Cart, CartDto>(cart);
        }
    }
}
