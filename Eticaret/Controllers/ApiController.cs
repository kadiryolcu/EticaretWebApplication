using System.Diagnostics;
using Eticaret.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace ETicaret.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiparislerimController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SiparislerimController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Siparislerim
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSiparislerim(int id)
        {



            // Kullanıcıya ait siparişleri getir
            var siparisler = await _context.Siparisler
                .Where(s => s.KullaniciId == id)
                .Select(s => new
                {
                    s.Id,
                    s.KullaniciId,
                    s.AdresId,
                    s.SiparisTarihi,
                    s.ToplamTutar,
                    s.Durum,
                    s.TeslimatSecenegiId
                })
                .ToListAsync();

            return Ok(siparisler);
        }
    }
}
