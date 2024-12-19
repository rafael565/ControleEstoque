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
using System.IO;

namespace ControleEstoque.Controllers
{
    public class ComputadoresController : Controller
    {
        private readonly Contexto _context;

        public ComputadoresController(Contexto context)
        {
            _context = context;
        }

        // GET: Computadores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Computadores.ToListAsync());
        }

        [HttpGet]
        public async Task<FileResult> ExportarComputadoresAExcel()
        {
            var computadores = await _context.Computadores.ToListAsync();
            var nomeArquivo = $"Computadores.xlsx";
            return GenerarExcel(nomeArquivo, computadores);
        }

        private FileResult GenerarExcel(string nomeArquivo, IEnumerable<Computador> computadores)
        {
            DataTable dataTable = new DataTable("Computadores");
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

            foreach (var computador in computadores)
            {
                dataTable.Rows.Add(computador.id,
                    computador.modelo,
                    computador.serie,
                    computador.patrimonio,
                    computador.quantidade,
                    computador.nome,
                    computador.setor,
                    computador.status);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dataTable);  // Corrigido: definindo 'ws' como a worksheet

                // Adiciona uma linha no final com o total de dispositivos
                var lastRow = dataTable.Rows.Count + 2;  // Fica 2 linhas abaixo do último item
                ws.Cell(lastRow, 1).Value = "Total de Dispositivos:";
                ws.Cell(lastRow, 2).Value = computadores.Count();  // Conta o número total de computadores

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        nomeArquivo);
                }
            }
        }

        // GET: Computadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computador = await _context.Computadores
                .FirstOrDefaultAsync(m => m.id == id);
            if (computador == null)
            {
                return NotFound();
            }

            return View(computador);
        }

        // GET: Computadores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Computadores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,modelo,serie,patrimonio,quantidade,nome,setor,status")] Computador computador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(computador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(computador);
        }

        // GET: Computadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computador = await _context.Computadores.FindAsync(id);
            if (computador == null)
            {
                return NotFound();
            }
            return View(computador);
        }

        // POST: Computadores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,modelo,serie,patrimonio,quantidade,nome,setor,status")] Computador computador)
        {
            if (id != computador.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(computador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComputadorExists(computador.id))
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
            return View(computador);
        }

        // GET: Computadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computador = await _context.Computadores
                .FirstOrDefaultAsync(m => m.id == id);
            if (computador == null)
            {
                return NotFound();
            }

            return View(computador);
        }

        // POST: Computadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var computador = await _context.Computadores.FindAsync(id);
            if (computador != null)
            {
                _context.Computadores.Remove(computador);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComputadorExists(int id)
        {
            return _context.Computadores.Any(e => e.id == id);
        }
    }
}
