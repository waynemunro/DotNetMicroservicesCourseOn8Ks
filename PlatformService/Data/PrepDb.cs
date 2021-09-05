
namespace PlatformService.Data;
public static class PrepDb
{
    public static void PrePopulation(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();


        if (context != null)
            SeedData(context);
    }

    private static void SeedData(AppDbContext context)
    {
        if (context.Platforms == null)
        {
            Console.WriteLine("No Platforms Data...");
            return;
        }

        if (!context.Platforms.Any())
        {
            // TODO: log properly with a logger

            Console.WriteLine("Seeding Data...");

            context.Platforms.AddRange(
                new Models.Platform() { Name = "Dotnet", Publisher = "Microsoft", Cost = "free" },
                new Models.Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "free" },
                new Models.Platform() { Name = "Kubernetes", Publisher = "Google", Cost = "free" }
                );

            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("Data is already populated");
        }
    }
}
