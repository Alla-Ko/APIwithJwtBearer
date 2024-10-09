using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


public class UserContext : IdentityDbContext
{
    public UserContext(DbContextOptions<UserContext> options)
    : base(options) { }
}

