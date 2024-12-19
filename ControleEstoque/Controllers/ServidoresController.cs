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
    public class ServidoresController : Controller
    {
        private readonly Contexto _context;

        public ServidoresController(Contexto context)
        {
            _context = context;
        }

        // GET: Servidores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Servidores.ToListAsync());
        }

        [HttpGet]
        public async Task<FileResult> ExportarServidoresAExcel()
        {
            var servidores = await _context.Servidores.ToListAsync();
            var nomeArquivo = $"Servidores.xlsx";
            return GenerarExcel(nomeArquivo, servidores);
        }

        private FileResult GenerarExcel(string nomeArquivo, IEnumerable<Servidor> servidores)
        {
            DataTable dataTable = new DataTable("Servidores");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("id"),
                new DataColumn("modelo"),
                new DataColumn("serie"),
                new DataColumn("patrimonio"),
                new DataColumn("quantidade"),
                new DataColumn("status"),
            });

            // Corrigindo o nome da variável de 'rede' para 'servidor'
            foreach (var servidor in servidores)
            {
                dataTable.Rows.Add(servidor.id,
                    servidor.modelo,
                    servidor.serie,
                    servidor.patrimonio,
                    servidor.quantidade,
                    servidor.status);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dataTable);  // Corrigido: definindo 'ws' como a worksheet

                // Adiciona uma linha no final com o total de dispositivos
                var lastRow = dataTable.Rows.Count + 2;  // Fica 2 linhas abaixo do último item
                ws.Cell(lastRow, 1).Value = "Total de Dispositivos:";
                ws.Cell(lastRow, 2).Value = servidores.Count();  // Conta o número total de computadores

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        nomeArquivo);
                }
            }
        }

        // GET: Servidores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servidor = await _context.Servidores
                .FirstOrDefaultAsync(m => m.id == id);
            if (servidor == null)
            {
                return NotFound();
            }

            return View(servidor);
        }

        // GET: Servidores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Servidores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,modelo,serie,patrimonio,quantidade,status")] Servidor servidor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(servidor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(servidor);
        }

        // GET: Servidores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servidor = await _context.Servidores.FindAsync(id);
            if (servidor == null)
            {
                return NotFound();
            }
            return View(servidor);
        }

        // POST: Servidores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,modelo,serie,patrimonio,quantidade,status")] Servidor servidor)
        {
            if (id != servidor.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servidor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServidorExists(servidor.id))
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
            return View(servidor);
        }

        // GET: Servidores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servidor = await _context.Servidores
                .FirstOrDefaultAsync(m => m.id == id);
            if (servidor == null)
            {
                return NotFound();
            }

            return View(servidor);
        }

        // POST: Servidores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servidor = await _context.Servidores.FindAsync(id);
            if (servidor != null)
            {
                _context.Servidores.Remove(servidor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServidorExists(int id)
        {
            return _context.Servidores.Any(e => e.id == id);
        }
    }
}
