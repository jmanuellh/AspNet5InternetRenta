using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNet5InternetRenta.Data;
using AspNet5InternetRenta.Models;
using OfficeOpenXml;
using System.IO;
using System.Globalization;

namespace AspNet5InternetRenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternetRentasController : ControllerBase
    {
        private readonly InternetRentaContext _context;

        public InternetRentasController(InternetRentaContext context)
        {
            _context = context;
        }

        // GET: api/InternetRentas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InternetRenta>>> GetInternetRentas()
        {
            return await _context.InternetRentas.ToListAsync();
        }

        // GET: api/InternetRentas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InternetRenta>> GetInternetRenta(long id)
        {
            var internetRenta = await _context.InternetRentas.FindAsync(id);

            if (internetRenta == null)
            {
                return NotFound();
            }

            return internetRenta;
        }

        // PUT: api/InternetRentas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInternetRenta(long id, InternetRenta internetRenta)
        {
            if (id != internetRenta.Id)
            {
                return BadRequest();
            }

            _context.Entry(internetRenta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InternetRentaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/InternetRentas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InternetRenta>> PostInternetRenta(InternetRenta internetRenta)
        {
            if (internetRenta.Nombre is null || internetRenta.Cantidad is 0)
            {
                return Ok();                
            }

            _context.InternetRentas.Add(internetRenta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInternetRenta", new { id = internetRenta.Id }, internetRenta);
        }

        // DELETE: api/InternetRentas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInternetRenta(long id)
        {
            var internetRenta = await _context.InternetRentas.FindAsync(id);
            if (internetRenta == null)
            {
                return NotFound();
            }

            _context.InternetRentas.Remove(internetRenta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InternetRentaExists(long id)
        {
            return _context.InternetRentas.Any(e => e.Id == id);
        }

        // GET: api/InternetRentas/descargarExcel
        [HttpGet("descargarExcel")]
        public async Task<FileContentResult> getExcelInternetRentas()
        {
            var archivoEnBytes = await GenerarExcel();

            return File(
                archivoEnBytes,
                "application/octet-stream",
                "Internet Rentas.xlsx"
            );
        }

        private async Task<Byte[]> GenerarExcel()
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
            //Set some properties of the Excel document
            excelPackage.Workbook.Properties.Author = "jmanuellh";
            excelPackage.Workbook.Properties.Title = "Internet Rentas";
            excelPackage.Workbook.Properties.Subject = "Internet rentas registro";
            excelPackage.Workbook.Properties.Created = DateTime.Now;

                //Create the WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Internet rentas");

                worksheet.Cells[1,1].Value = "Cortado";
                worksheet.Cells[1,2].Value = "Nombre";
                worksheet.Cells[1,3].Value = "Fecha corte";
                worksheet.Cells[1,4].Value = "Cantidad";

                var rentas = await _context.InternetRentas.ToListAsync();

                int contador = 0;
                foreach (var renta in rentas)
                {
                    worksheet.Cells[contador+2, 1].Value = renta.Cortado;
                    worksheet.Cells[contador+2, 2].Value = renta.Nombre;
                    worksheet.Cells[contador+2, 3].Value = renta.FechaCorte.ToString("d", CultureInfo.CreateSpecificCulture("es-MX"));
                    worksheet.Cells[contador+2, 4].Value = renta.Cantidad;
                    contador++;
                }

                return excelPackage.GetAsByteArray();
            }
        }
    }
}
