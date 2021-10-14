using System;
using Mango.Services.Orders.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<OrderHeader> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
}
