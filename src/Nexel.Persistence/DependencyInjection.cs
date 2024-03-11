using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexel.Application.Abstractions;
using Nexel.Domain.Modules.Sheets;
using Nexel.Persistence.Repositories;

namespace Nexel.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                x => x.MigrationsAssembly("Nexel.Persistence")));

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ISheetRepository, SheetRepository>();
        services.AddHostedService<DatabaseInitHandler>();

        return services;
    }
}