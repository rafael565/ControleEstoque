using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using System.Data;
using ControleEstoque.Models;

namespace ControleEstoque.Controllers
{
    public class MonitorEquipamentosController : Controller
    {
        private readonly Contexto _context;

        public MonitorEquipamentosController(Contexto context)
        {
            _context = context;
        }

        // GET: MonitorEquipamentos
        public async Task<IActionResult> Index()
        {
            return View(await _context.MonitorEquipamentos.ToListAsync());
        }

        [HttpGet]
        public async Task<FileResult> ExportarMonitorEquipamentosAExcel()
        {
            var monitores = await _context.MonitorEquipamentos.ToListAsync();
            var nomeArquivo = $"Monitores.xlsx";
            return GenerarExcel(nomeArquivo, monitores);
        }

        private FileResult GenerarExcel(string nomeArquivo, IEnumerable<MonitorEquipamento> monitores)
        {
            DataTable dataTable = new DataTable("Monitores");
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

            foreach (var monitor in monitores)
            {
                dataTable.Rows.Add(
                    monitor.id,
                    monitor.modelo,
                    monitor.serie,
                    monitor.patrimonio,
                    monitor.quantidade,
                    monitor.nome,
                    monitor.setor,
                    monitor.status
                );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dataTable);

                // Adiciona uma linha no final com o total de dispositivos
                var lastRow = dataTable.Rows.Count + 2;  // Fica 2 linhas abaixo do último item
                ws.Cell(lastRow, 1).Value = "Total de Dispositivos:";
                ws.Cell(lastRow, 2).Value = monitores.Count();  // Conta o número total de monitores

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        nomeArquivo);
                }
            }
        }

        // GET: MonitorEquipamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monitorEquipamento = await _context.MonitorEquipamentos
                .FirstOrDefaultAsync(m => m.id == id);
            if (monitorEquipamento == null)
            {
                return NotFound();
            }

            return View(monitorEquipamento);
        }

        // GET: MonitorEquipamentos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MonitorEquipamentos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,modelo,serie,patrimonio,quantidade,nome,setor,status")] MonitorEquipamento monitorEquipamento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(monitorEquipamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(monitorEquipamento);
        }

        // GET: MonitorEquipamentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monitorEquipamento = await _context.MonitorEquipamentos.FindAsync(id);
            if (monitorEquipamento == null)
            {
                return NotFound();
            }
            return View(monitorEquipamento);
        }

        // POST: MonitorEquipamentos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,modelo,serie,patrimonio,quantidade,nome,setor,status")] MonitorEquipamento monitorEquipamento)
        {
            if (id != monitorEquipamento.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monitorEquipamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonitorEquipamentoExists(monitorEquipamento.id))
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
            return View(monitorEquipamento);
        }

        // GET: MonitorEquipamentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monitorEquipamento = await _context.MonitorEquipamentos
                .FirstOrDefaultAsync(m => m.id == id);
            if (monitorEquipamento == null)
            {
                return NotFound();
            }

            return View(monitorEquipamento);
        }

        // POST: MonitorEquipamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monitorEquipamento = await _context.MonitorEquipamentos.FindAsync(id);
            if (monitorEquipamento != null)
            {
                _context.MonitorEquipamentos.Remove(monitorEquipamento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MonitorEquipamentoExists(int id)
        {
            return _context.MonitorEquipamentos.Any(e => e.id == id);
        }
    }
}
