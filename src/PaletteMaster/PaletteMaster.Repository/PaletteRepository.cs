using Microsoft.EntityFrameworkCore;
using PaletteMaster.Models.Domain;
using PaletteMaster.Models.DTO.Palettes;
using PaletteMaster.Services.Palettes;

namespace PaletteMaster.Repository;

public class PaletteRepository : IPaletteRepository
{
    private readonly ApplicationDbContext _context;
    
    public PaletteRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Palette>> GetPalettesAsync(GetPalettesRequest request)
    {
        IQueryable<Palette> query = _context.Palettes.Include<Palette, List<Color>>(p => p.Colors);

        if (request.Name is not null)
        {
            query = query.Where(p => p.Name.ToLower().Contains(request.Name.ToLower()));
        }
        
        if (request.Colors is not null && request.Colors.Count > 0)
        {
            query = query.Where(p => p.Colors.Any(c => request.Colors.Contains(c)));
        }
        
        query = request.Sorting switch
        {
            GetPalettesSorting.NameAsc => query.OrderBy(p => p.Name),
            GetPalettesSorting.NameDesc => query.OrderByDescending(p => p.Name),
            GetPalettesSorting.CreatedDateAsc => query.OrderBy(p => p.CreatedDate),
            GetPalettesSorting.CreatedDateDesc => query.OrderByDescending(p => p.CreatedDate),
            GetPalettesSorting.ModifiedDateAsc => query.OrderBy(p => p.ModifiedDate),
            GetPalettesSorting.ModifiedDateDesc => query.OrderByDescending(p => p.ModifiedDate),
            GetPalettesSorting.NumColorsAsc => query.OrderBy(p => p.Colors.Count),
            GetPalettesSorting.NumColorsDesc => query.OrderByDescending(p => p.Colors.Count),
            _ => query
        };
        
        query = query.Skip(request.PageSize * (request.Page - 1)).Take(request.PageSize);
        
        return await query.ToListAsync();
    }

    public async Task<int> GetPalettesCountAsync(GetPalettesRequest request)
    {
        IQueryable<Palette> query = _context.Palettes.Include<Palette, List<Color>>(p => p.Colors);

        if (request.Name is not null)
        {
            query = query.Where(p => p.Name.ToLower().Contains(request.Name.ToLower()));
        }
        
        if (request.Colors is not null && request.Colors.Count > 0)
        {
            query = query.Where(p => p.Colors.Any(c => request.Colors.Contains(c)));
        }
        
        query = request.Sorting switch
        {
            GetPalettesSorting.NameAsc => query.OrderBy(p => p.Name),
            GetPalettesSorting.NameDesc => query.OrderByDescending(p => p.Name),
            GetPalettesSorting.CreatedDateAsc => query.OrderBy(p => p.CreatedDate),
            GetPalettesSorting.CreatedDateDesc => query.OrderByDescending(p => p.CreatedDate),
            GetPalettesSorting.ModifiedDateAsc => query.OrderBy(p => p.ModifiedDate),
            GetPalettesSorting.ModifiedDateDesc => query.OrderByDescending(p => p.ModifiedDate),
            GetPalettesSorting.NumColorsAsc => query.OrderBy(p => p.Colors.Count),
            GetPalettesSorting.NumColorsDesc => query.OrderByDescending(p => p.Colors.Count),
            _ => query
        };
        
        return query.Count();
    }

    public async Task<Palette?> GetPaletteAsync(int requestPalletId)
    {
        return await _context.Palettes.Include<Palette, List<Color>>(p => p.Colors)
            .FirstOrDefaultAsync(p => p.PaletteId == requestPalletId);
    }

    public async Task<Palette?> GetPaletteWithUseTrackingAsync(int requestPalleteId)
    {
        return await _context.Palettes.Include<Palette, List<Color>>(p => p.Colors)
            .Include<Palette, List<PaletteUseTracking>>(p => p.PaletteUseTrackings)
            .FirstOrDefaultAsync(p => p.PaletteId == requestPalleteId);
    }

    public async Task<Palette> CreatePaletteAsync(Palette palette)
    {
        await _context.Palettes.AddAsync(palette);
        await _context.SaveChangesAsync();
        
        return palette;
    }
}