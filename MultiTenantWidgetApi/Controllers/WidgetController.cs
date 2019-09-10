using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MultiTenantWidgetApi.DataStore;
using MultiTenantWidgetApi.Models;
using MultiTenantWidgetApi.Services;

namespace MultiTenantWidgetApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WidgetController : ControllerBase
    {
        private readonly WidgetDbContext _context;

        public WidgetController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            var tenantId = httpContextAccessor.HttpContext.User.Identity.Name;
            var settings = new TenantSettings
            {
                TenantId = tenantId,
                // get default cn from appsettings.json
                DefaultConnectionString = configuration.GetConnectionString("DefaultConnection")
            };

            // build a DbContext specifically for this tenant
            var options = CreateDbContextOptionsBuilder(settings);
            _context = new WidgetDbContext(options);

            // seed data for the tenant
            SeedTestData(settings);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Widget>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get()
        {
            if (_context == null)
            {
                return StatusCode(500);
            }

            var list = await _context.Widgets
                .AsNoTracking()
                .ToListAsync();

            return Ok(list);
        }

        private DbContextOptions<WidgetDbContext> CreateDbContextOptionsBuilder(TenantSettings settings)
        {
            // build a DbContext for this tenant
            var service = new CatalogService(settings);
            var cn = service.GetConnectionString().Result;

            // -------------------------------------------------------------------
            // This would be production code with a real backend database
            // -------------------------------------------------------------------
            // var optionsBuilder = new DbContextOptionsBuilder<WidgetDbContext>()
            //     .UseSqlServer(cn);

            // returning an in-memory db instead just for illustration purposes
            var optionsBuilder = new DbContextOptionsBuilder<WidgetDbContext>()
                .UseInMemoryDatabase(databaseName: settings.TenantId);

            return optionsBuilder.Options;
        }

        /// <summary>
        /// Quick and dirty simulation of physically isolated databases for tenants
        /// </summary>
        /// <param name="settings"></param>
        private void SeedTestData(TenantSettings settings)
        {
            RemoveAllWidgets();

            switch (settings.TenantId)
            {
                case "tenant1":
                    _context.Widgets.AddRange(new List<Widget>() {
                        new Widget { Id = Guid.NewGuid(), Color = "Yellow", Shape = "Square" },
                        new Widget { Id = Guid.NewGuid(), Color = "Blue", Shape = "Round" },
                    });
                    break;

                case "tenant2":
                    _context.Widgets.AddRange(new List<Widget>() {
                        new Widget { Id = Guid.NewGuid(), Color = "Green", Shape = "Cube" },
                        new Widget { Id = Guid.NewGuid(), Color = "Purple", Shape = "Polyhedron" },
                    });
                    break;

                default:
                    _context.Widgets.AddRange(new List<Widget>() {
                        new Widget { Id = Guid.NewGuid(), Color = "Brown", Shape = "Tube" },
                        new Widget { Id = Guid.NewGuid(), Color = "Black", Shape = "Knob" },
                    });
                    break;
            }

            _context.SaveChanges();
        }
        
        private void RemoveAllWidgets()
        {
            foreach (var entity in _context.Widgets)
            {
                _context.Remove(entity);
            }

            _context.SaveChanges();
        }
    }
}