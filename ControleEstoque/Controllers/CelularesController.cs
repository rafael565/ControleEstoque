using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControleEstoque.Models;
using ClosedXML.Excel;
using System.Data;

namespace ControleEstoque.Controllers
{
    public class CelularesController : Controller
    {
        private readonly Contexto _context;

        public CelularesController(Contexto context)
        {
            _context = context;
        }

        // GET: Celulares
        public async Task<IActionResult> Index()
        {
            return View(await _context.Celulares.ToListAsync());
        }

        [HttpGet]
        public async Task<FileResult> ExportarCelularesAExcel()
        {
            var celulares = await _context.Celulares.ToListAsync();
            var nomeArchivo = $"Celulares.xlsx";
            return GenerarExcel(nomeArchivo, celulares);
        }

        private FileResult GenerarExcel(string nomeArchivo, IEnumerable<Celular> celulares)
        {
            DataTable dataTable = new DataTable("Celulares");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("id"),
                new DataColumn("modelo"),
                new DataColumn("serie"),
                new DataColumn("patrimonio"),
                new DataColumn("quantidade"),
                new DataColumn("nome"),
                new DataColumn("setor"),
                new DataColumn("status")
            });

            foreach (var celular in celulares)
            {
                dataTable.Rows.Add(celular.id,
                    celular.modelo,
                    celular.serie,
                    celular.patrimonio,
                    celular.quantidade,
                    celular.nome,
                    celular.setor,
                    celular.status);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dataTable);  // Corrigido: definindo 'ws' como a worksheet

                // Adiciona uma linha no final com o total de dispositivos
                var lastRow = dataTable.Rows.Count + 2;  // Fica 2 linhas abaixo do último item
                ws.Cell(lastRow, 1).Value = "Total de Dispositivos:";
                ws.Cell(lastRow, 2).Value = celulares.Count();  // Conta o número total de celulares

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        nomeArchivo);
                }
            }
        }

        // GET: Celulares/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var celular = await _context.Celulares
                .FirstOrDefaultAsync(m => m.id == id);
            if (celular == null)
            {
                return NotFound();
            }

            return View(celular);
        }

        // GET: Celulares/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Celulares/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,modelo,serie,patrimonio,quantidade,nome,setor,status")] Celular celular)
        {
            if (ModelState.IsValid)
            {
                _context.Add(celular);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(celular);
        }

        // GET: Celulares/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var celular = await _context.Celulares.FindAsync(id);
            if (celular == null)
            {
                return NotFound();
            }
            return View(celular);
        }

        // POST: Celulares/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,modelo,serie,patrimonio,quantidade,nome,setor,status")] Celular celular)
        {
            if (id != celular.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(celular);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CelularExists(celular.id))
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
            return View(celular);
        }

        // GET: Celulares/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var celular = await _context.Celulares
                .FirstOrDefaultAsync(m => m.id == id);
            if (celular == null)
            {
                return NotFound();
            }

            return View(celular);
        }

        // POST: Celulares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var celular = await _context.Celulares.FindAsync(id);
            if (celular != null)
            {
                _context.Celulares.Remove(celular);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CelularExists(int id)
        {
            return _context.Celulares.Any(e => e.id == id);
        }
    }
}
