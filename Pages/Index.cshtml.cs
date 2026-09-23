using BusCima.Data;
using BusCima.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusCima.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        public IndexModel(AppDbContext db) { _db = db; }

        public void OnGet() { }

        // ---------- RESEÑAS ----------
        public JsonResult OnGetResenias(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta))
                return new JsonResult(new { promedio = 0.0, total = 0, resenias = new List<object>() });

            var lista = _db.Resenias
                .Where(r => r.RutaNombre == ruta)
                .OrderByDescending(r => r.ID)
                .ToList();

            double promedio = lista.Count > 0 ? lista.Average(r => r.Estrellas) : 0;

            return new JsonResult(new
            {
                promedio = Math.Round(promedio, 1),
                total = lista.Count,
                resenias = lista.Select(r => new { r.ID, r.Usuario, r.Mensaje, r.Estrellas })
            });
        }

        public JsonResult OnPostGuardarResenia([FromBody] ReseniaDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Mensaje))
                return new JsonResult(new { ok = false, error = "El mensaje no puede estar vacío." });

            if (dto.Estrellas < 1 || dto.Estrellas > 5)
                return new JsonResult(new { ok = false, error = "Las estrellas deben ser entre 1 y 5." });

            var usuario = string.IsNullOrWhiteSpace(dto.Usuario) ? "Anonimo" : dto.Usuario.Trim();
            if (usuario.Length > 50) usuario = usuario.Substring(0, 50);

            var mensaje = dto.Mensaje.Trim();
            if (mensaje.Length > 200) mensaje = mensaje.Substring(0, 200);

            var nueva = new Resenia
            {
                Usuario = usuario,
                Mensaje = mensaje,
                Estrellas = dto.Estrellas,
                RutaNombre = dto.RutaNombre
            };

            _db.Resenias.Add(nueva);
            _db.SaveChanges();

            return new JsonResult(new { ok = true, id = nueva.ID });
        }

        // ---------- PARADAS ----------
        // GET: /Index?handler=Paradas&ruta=Vigía 1
        public JsonResult OnGetParadas(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta))
                return new JsonResult(new List<object>());

            var paradas = _db.Paradas
                .Where(p => p.RutaNombre == ruta && p.Latitud != null && p.Longitud != null)
                .OrderBy(p => p.Orden)
                .Select(p => new
                {
                    p.IdParada,
                    p.Nombre,
                    Latitud = (double)p.Latitud,
                    Longitud = (double)p.Longitud,
                    p.Orden,
                    p.EsWaypoint
                })
                .ToList();

            return new JsonResult(paradas);
        }

        // GET: /Index?handler=TodasLasParadas
        public JsonResult OnGetTodasLasParadas()
        {
            var paradas = _db.Paradas
                .Where(p => p.Latitud != null && p.Longitud != null && p.EsWaypoint == false)
                .Select(p => new
                {
                    p.IdParada,
                    p.Nombre,
                    Latitud = (double)p.Latitud,
                    Longitud = (double)p.Longitud,
                    p.RutaNombre,
                    p.Orden
                })
                .ToList();

            return new JsonResult(paradas);
        }

        public class ReseniaDto
        {
            public string Usuario { get; set; }
            public string Mensaje { get; set; }
            public int Estrellas { get; set; }
            public string RutaNombre { get; set; }
        }
    }
}