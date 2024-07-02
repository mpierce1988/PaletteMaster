using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaletteMaster.Models.Domain;

namespace PaletteMaster.Repository;

public class ApplicationDbContext : DbContext
{
    public virtual DbSet<Color> Colors { get; set; }
    public virtual DbSet<Palette> Palettes { get; set; }
    public virtual DbSet<PaletteUseTracking> PaletteUseTrackings { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext> 
{ 
    public ApplicationDbContext CreateDbContext(string[] args) 
    { 
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/appsettings.json").Build(); 
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>(); 
        var sqliteDataSource = configuration.GetConnectionString("SqlLiteDataSource");
        builder.UseSqlite(sqliteDataSource);
        return new ApplicationDbContext(builder.Options); 
    } 
}

public static class RegisterDbContext
{
    public static void RegisterApplicationDbContext(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            //IConfigurationRoot localConfiguration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/appsettings.json").Build(); 
            string dbName = "palettemaster.db";
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                dbName);
            options.UseSqlite($"Data Source = {dbPath}");
        });
    }
}
