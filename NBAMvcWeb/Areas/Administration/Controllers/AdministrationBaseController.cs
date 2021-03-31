using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NBAMvcWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NBAMvcWeb.Areas.Administration.Controllers
{
    //Without Authorize notation, anyone who knows the link can access Administration page.
    [Area("Administration"), Authorize(Roles = "Administrator")]
    public abstract class AdministrationBaseController : Controller
    {

        protected readonly ApplicationDbContext _context;
        protected readonly IMapper _mapper;

        public AdministrationBaseController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
