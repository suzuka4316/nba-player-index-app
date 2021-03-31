using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NBAMvcWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NBAMvcWeb.Areas.Administration.Controllers
{
    public class HomeController : AdministrationBaseController
    {
        public HomeController(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
