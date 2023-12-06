﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKitAB104.Areas.Admin.ViewModels;
using PestKitAB104.DAL;
using PestKitAB104.Models;
using System.Data;

namespace PestKitAB104.Area.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Index()
        {
            List<Employee> employees = await _context.Employees.ToListAsync();
            return View(employees);
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Positions=await _context.Positions.ToListAsync();
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVM employeeVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = await _context.Positions.ToListAsync();
                ViewBag.Departments = await _context.Departments.ToListAsync();
                return View();
            }

            bool result = _context.Authors.Any(a => a.Name.Trim() == employeeVM.Name.Trim());
            if (result)
            {
                ViewBag.Positions = await _context.Positions.ToListAsync();
                ViewBag.Departments = await _context.Departments.ToListAsync();
                ModelState.AddModelError("Name", "Bu adda ishci movcuddur");
                return View();
            }

            Employee employee = new Employee
            {
                Name = employeeVM.Name,
                DepartmentId=(int)employeeVM.DepartmentId,
                PositionId=(int)employeeVM.PositionId,
            };

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Employee employee = await _context.Employees.FirstOrDefaultAsync(d => d.Id == id);

            if (employee is null) return NotFound();

            UpdateEmployeeVM employeeVM = new UpdateEmployeeVM
            {
                Name = employee.Name,
                DepartmentId=(int)employee.DepartmentId,
                PositionId=(int)employee.PositionId
            };

            ViewBag.Positions = await _context.Positions.ToListAsync();
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View(employeeVM);
        }
        [HttpPost]

        public async Task<IActionResult> Update(int id, UpdateEmployeeVM employeeVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = await _context.Positions.ToListAsync();
                ViewBag.Departments = await _context.Departments.ToListAsync();
                return View(employeeVM);
            }
            Employee existed = await _context.Employees.FirstOrDefaultAsync(d => d.Id == id);

            if (existed is null) return NotFound();

            bool result = _context.Employees.Any(c => c.Name == employeeVM.Name && c.Id != id);
            if (result)
            {
                ViewBag.Positions = await _context.Positions.ToListAsync();
                ViewBag.Departments = await _context.Departments.ToListAsync();
                ModelState.AddModelError("Name", "Bu adda ishci artiq movcuddur");
                return View();
            }
            existed.Name = employeeVM.Name;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Employee existed = await _context.Employees.FirstOrDefaultAsync(a => a.Id == id);

            if (existed is null) return NotFound();

            _context.Employees.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();

            Employee employee = await _context.Employees.FirstOrDefaultAsync(d => d.Id == id);
            if (employee == null) return NotFound();

            return View(employee);
        }
    }
}
