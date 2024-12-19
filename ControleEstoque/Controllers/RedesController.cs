using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControleEstoque.Models;
using ClosedXML.Excel;
using System.Data;
using System.IO;
using NuGet.DependencyResolver;

namespace ControleEstoque.Controllers
{
    public class RedesController : Controller
    {
        private readonly Contexto _context;

        public RedesController(Contexto context)
        {
            _context = context;
        }

        // GET: Redes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Redes.ToListAsync());
        }

        [HttpGet]
        public async Task<FileResult> ExportarRedesAExcel()
        {
            var redes = await _context.Redes.ToListAsync();
            var nomeArquivo = $"Redes.xlsx";
            return GenerarExcel(nomeArquivo, redes);
        }

        private FileResult GenerarExcel(string nomeArquivo, IEnumerable<Rede> redes)
        {
            DataTable dataTable = new DataTable("Redes");
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

            // Corrigindo o nome da variável de 'dispositivo' para 'rede'
            foreach (var rede in redes)
            {
                dataTable.Rows.Add(rede.id,
                    rede.modelo,
                    rede.serie,
                    rede.patrimonio,
                    rede.quantidade,
                    rede.setor,
                    rede.status);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dataTable);  // Corrigido: definindo 'ws' como a worksheet

                // Adiciona uma linha no final com o total de dispositivos
                var lastRow = dataTable.Rows.Count + 2;  // Fica 2 linhas abaixo do último item
                ws.Cell(lastRow, 1).Value = "Total de Dispositivos:";
                ws.Cell(lastRow, 2).Value = redes.Count();  // Conta o número total de computadores

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        nomeArquivo);
                }
            }
        }

        // GET: Redes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rede = await _context.Redes
                .FirstOrDefaultAsync(m => m.id == id);
            if (rede == null)
            {
                return NotFound();
            }

            return View(rede);
        }

        // GET: Redes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Redes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,modelo,serie,patrimonio,quantidade,setor,status")] Rede rede)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rede);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rede);
        }

        // GET: Redes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rede = await _context.Redes.FindAsync(id);
            if (rede == null)
            {
                return NotFound();
            }
            return View(rede);
        }

        // POST: Redes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,modelo,serie,patrimonio,quantidade,setor,status")] Rede rede)
        {
            if (id != rede.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rede);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RedeExists(rede.id))
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
            return View(rede);
        }

        // GET: Redes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rede = await _context.Redes
                .FirstOrDefaultAsync(m => m.id == id);
            if (rede == null)
            {
                return NotFound();
            }

            return View(rede);
        }

        // POST: Redes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rede = await _context.Redes.FindAsync(id);
            if (rede != null)
            {
                _context.Redes.Remove(rede);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RedeExists(int id)
        {
            return _context.Redes.Any(e => e.id == id);
        }
    }
}
