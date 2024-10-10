using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using desu.life.Data;
using desu.life.Data.Models;

namespace desu.life.Controllers
{
    public class InfoPanelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InfoPanelsController(ApplicationDbContext context)
        {
            _context = context;
        }

    }
}
