using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Transactions;

public class Worker(IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {

            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ProductsContext>();

            await EnsureDatabaseAsync(dbContext, cancellationToken);
            await RunMigrationAsync(dbContext, cancellationToken);
            await SeedDataAsync(dbContext, cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }
    }

    private async Task SeedDataAsync(ProductsContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Führen Sie diesen Block nur aus, wenn die Datenbank leer ist
            if (!await dbContext.Categories.AnyAsync(cancellationToken) && !await dbContext.Products.AnyAsync(cancellationToken))
            {
                // Erstellen der Kategorien
                var categoryElectronics = new Category { Name = "Electronics" };
                var categoryBooks = new Category { Name = "Books" };
                var categoryFashion = new Category { Name = "Fashion" }; 

                // Produkte erstellen
                var laptop = new Product { Name = "Laptop", Price = 1200m, Description = "Powerful laptop for work and gaming." };
                var mouse = new Product { Name = "Mouse", Price = 25m, Description = "Wireless ergonomic mouse." };
                var hitchhikersGuide = new Product { Name = "The Hitchhiker's Guide to the Galaxy", Price = 10m, Description = "A hilarious science fiction novel." };
                var tShirt = new Product { Name = "Cotton T-Shirt", Price = 30m, Description = "Comfortable everyday t-shirt." };

                // Laptop gehört zu Electronics
                laptop.Categories.Add(categoryElectronics);

                // Mouse gehört zu Electronics
                mouse.Categories.Add(categoryElectronics);

                // The Hitchhiker's Guide to the Galaxy gehört zu Books
                hitchhikersGuide.Categories.Add(categoryBooks);

                // T-Shirt gehört zu Fashion
                tShirt.Categories.Add(categoryFashion);

                // Hinzufügen der Entitäten zum DbContext
                await dbContext.Categories.AddAsync(categoryElectronics, cancellationToken);
                await dbContext.Categories.AddAsync(categoryBooks, cancellationToken);
                await dbContext.Categories.AddAsync(categoryFashion, cancellationToken);

                await dbContext.Products.AddAsync(laptop, cancellationToken);
                await dbContext.Products.AddAsync(mouse, cancellationToken);
                await dbContext.Products.AddAsync(hitchhikersGuide, cancellationToken);
                await dbContext.Products.AddAsync(tShirt, cancellationToken);

                // Speichern aller Änderungen in einer Transaktion
                await using (var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken))
                {
                    await dbContext.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }

                Console.WriteLine("Initial seed data added.");
            }
            else
            {
                Console.WriteLine("Database already contains data, skipping seed.");
            }
        });
    }

    private async Task RunMigrationAsync(ProductsContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }

    private static async Task EnsureDatabaseAsync(ProductsContext dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                await dbCreator.CreateAsync(cancellationToken);
            }
        });
    }
}