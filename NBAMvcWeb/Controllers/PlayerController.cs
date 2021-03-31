using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NBAMvcWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NBAMvcWeb.Controllers
{
    public class PlayerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        //for validating logined user
        private readonly UserManager<IdentityUser> _userManager;
        public PlayerController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                //ClaimsPrinciple doesn't exist in the database
                var cp = User;
                //IdentityUser is when you want to access the user data from the database
                IdentityUser iu = await _userManager.FindByNameAsync(User.Identity.Name);
                if (!User.IsInRole("Administrator"))
                {
                    await _userManager.AddToRoleAsync(iu, "Administrator");
                }
            }

            var m = await _context.Players.ToListAsync();
            return View(m);
        }

        public async Task<IActionResult> Details(int id)
        {
            var player = await _context.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (player == null)
            {
                return NotFound();
            }
            var m = _mapper.Map<Models.Player.Details>(player);
            return View(m);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var m = new Models.Player.Create
            {
                Teams = new SelectList(await _context.Teams.ToListAsync(), "Id", "Name")
            };
            return View(m);
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Age,Height,Weight,TeamId")] Models.Player.Create m)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var p = new Player
                    {
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        Age = m.Age,
                        Height = m.Height,
                        Weight = m.Weight,
                        TeamId = m.TeamId
                    };
                    _context.Players.Add(p);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("Exception", "Oops");
                }
            }
            m.Teams = new SelectList(await _context.Teams.ToListAsync(), "Id", "Name", m.TeamId);
            return View(m);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var p = await _context.Players.FindAsync(id);
            if (p == null)
            {
                return NotFound();
            }
            var m = _mapper.Map<Models.Player.Edit>(p);
            m.Teams = new SelectList(await _context.Teams.ToListAsync(), "Id", "Name", m.TeamId);
            return View(m);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id,FirstName,LastName,Age,Height,Weight,TeamId")] Models.Player.Edit m)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!_context.Players.Any(p=>p.Id == m.Id))
                    {
                        return NotFound();
                    }
                    _context.Update(_mapper.Map<Player>(m));

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("Exception", "Oops");
                }
            }
            m.Teams = new SelectList(await _context.Teams.ToListAsync(), "Id", "Name", m.TeamId);
            return View(m);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var player = await _context.Players
                                    .Include(p => p.Team)
                                    .FirstOrDefaultAsync(p => p.Id == id);
            if (player == null)
                return NotFound();
            var m = _mapper.Map<Models.Player.Details>(player);
            m.Team = player.Team;
            return View(m);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Models.Player.Details m)
        {
            try
            {
                var player = await _context.Players
                        .Include(p => p.Team)
                        .FirstOrDefaultAsync(p => p.Id == m.Id);
                if (player == null)
                    return NotFound();
                _context.Players.Remove(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("Exception", "Oops, you broke my code");
            }
            return View(m);
        }
    }
}
