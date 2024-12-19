using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControleEstoque.Models;
using ClosedXML.Excel;
using System.Data;
using NuGet.DependencyResolver;

namespace ControleEstoque.Controllers
{
    public class HardwaresController : Controller
    {
        private readonly Contexto _context;

        public HardwaresController(Contexto context)
        {
            _context = context;
        }

        // GET: Hardwares
        public async Task<IActionResult> Index()
        {
            return View(await _context.Hardwares.ToListAsync());
        }

        [HttpGet]
        public async Task<FileResult> ExportarHardwaresAExcel()
        {
            var hardwares = await _context.Hardwares.ToListAsync();
            var nomeArquivo = $"Hardwares.xlsx";
            return GenerarExcel(nomeArquivo, hardwares);
        }

        private FileResult GenerarExcel(string nomeArquivo, IEnumerable<Hardware> hardwares)
        {
            DataTable dataTable = new DataTable("Hardwares");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("id"),
                new DataColumn("modelo"),
                new DataColumn("serie"),
                new DataColumn("patrimonio"),
                new DataColumn("quantidade"),
                new DataColumn("nome"),
                new DataColumn("setor"),
                new DataColumn("status"),
            });

            // Corrigindo o nome da variável de 'celulares' para 'dispositivos'
            foreach (var dispositivo in hardwares)
            {
                dataTable.Rows.Add(dispositivo.id,
                    dispositivo.modelo,
                    dispositivo.serie,
                    dispositivo.patrimonio,
                    dispositivo.quantidade,
                    dispositivo.nome,
                    dispositivo.setor,
                    dispositivo.status);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dataTable);  // Corrigido: definindo 'ws' como a worksheet

                // Adiciona uma linha no final com o total de dispositivos
                var lastRow = dataTable.Rows.Count + 2;  // Fica 2 linhas abaixo do último item
                ws.Cell(lastRow, 1).Value = "Total de Dispositivos:";
                ws.Cell(lastRow, 2).Value = hardwares.Count();  // Conta o número total de computadores

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        nomeArquivo);
                }
            }
        }

        // GET: Hardwares/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardware = await _context.Hardwares
                .FirstOrDefaultAsync(m => m.id == id);
            if (hardware == null)
            {
                return NotFound();
            }

            return View(hardware);
        }

        // GET: Hardwares/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hardwares/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,modelo,serie,patrimonio,quantidade,nome,setor,status")] Hardware hardware)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hardware);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hardware);
        }

        // GET: Hardwares/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardware = await _context.Hardwares.FindAsync(id);
            if (hardware == null)
            {
                return NotFound();
            }
            return View(hardware);
        }

        // POST: Hardwares/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,modelo,serie,patrimonio,quantidade,nome,setor,status")] Hardware hardware)
        {
            if (id != hardware.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hardware);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HardwareExists(hardware.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hardware);
        }

        // GET: Hardwares/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardware = await _context.Hardwares
                .FirstOrDefaultAsync(m => m.id == id);
            if (hardware == null)
            {
                return NotFound();
            }

            return View(hardware);
        }

        // POST: Hardwares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hardware = await _context.Hardwares.FindAsync(id);
            if (hardware != null)
            {
                _context.Hardwares.Remove(hardware);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HardwareExists(int id)
        {
            return _context.Hardwares.Any(e => e.id == id);
        }
    }
}
