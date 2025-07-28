using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TrainComponentManagementSystem.Context;
using TrainComponentManagementSystem.Models;
using TrainComponentManagementSystem.Services.Interfaces;

namespace TrainComponentManagementSystem.Controllers
{
    [ApiController]
    [Route("api/components")]
    public class ComponentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public ComponentsController(ApplicationDbContext context, IMapper mapper, ICacheService cacheService)
        {
            _context = context;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? search = null, int page = 1, int pageSize = 20)
        {
            string cacheKey = $"components_{search}_{page}_{pageSize}";

            if (!_cacheService.TryGetValue(cacheKey, out object? cachedResult))
            {
                var query = _context.TrainComponents.AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(c => c.Name.Contains(search) || c.UniqueNumber.Contains(search));
                }

                var totalItems = await query.CountAsync();

                var items = await query
                    .OrderBy(c => c.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                cachedResult = new { totalItems, items };

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cacheService.Set(cacheKey, cachedResult, cacheEntryOptions);
            }

            return Ok(cachedResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            string cacheKey = $"component_{id}";

            if (!_cacheService.TryGetValue<TrainComponent>(cacheKey, out var component))
            {
                component = await _context.TrainComponents
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (component == null)
                    return NotFound();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _cacheService.Set(cacheKey, component, cacheOptions);
            }

            return Ok(component);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComponent([FromBody] TrainComponentDto model)
        {
            var newComponent = _mapper.Map<TrainComponent>(model);

            _context.TrainComponents.Add(newComponent);
            await _context.SaveChangesAsync();

            _cacheService.RemoveByPrefix("components_");

            return CreatedAtAction(nameof(GetById), new { id = newComponent.Id }, newComponent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComponent(int id, [FromBody] TrainComponentDto model)
        {
            var component = await _context.TrainComponents.FindAsync(id);
            if (component == null) return NotFound();

            _mapper.Map(model, component);

            if (!component.CanAssignQuantity)
            {
                component.Quantity = null;
            }

            await _context.SaveChangesAsync();

            _cacheService.Remove($"component_{id}");
            _cacheService.RemoveByPrefix("components_");

            return Ok(component);
        }

        [HttpDelete("soft/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var component = await _context.TrainComponents.FindAsync(id);
            if (component == null) return NotFound();

            if (component.IsDeleted)
                return BadRequest("Component is already deleted.");

            component.IsDeleted = true;
            await _context.SaveChangesAsync();

            _cacheService.Remove($"component_{id}");
            _cacheService.RemoveByPrefix("components_");

            return NoContent();
        }

        [HttpDelete("hard/{id}")]
        public async Task<IActionResult> HardDelete(int id)
        {
            var component = await _context.TrainComponents.FindAsync(id);
            if (component == null) return NotFound();

            _context.TrainComponents.Remove(component);
            await _context.SaveChangesAsync();

            _cacheService.Remove($"component_{id}");
            _cacheService.RemoveByPrefix("components_");

            return NoContent();
        }
    }
}
