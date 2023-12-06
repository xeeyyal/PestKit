using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKitAB104.DAL;
using PestKitAB104.Models;
using System.Reflection.Metadata.Ecma335;

namespace PestKitAB104.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }
        
    }
}
