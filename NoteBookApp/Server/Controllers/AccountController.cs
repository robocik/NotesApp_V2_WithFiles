using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using NoteBookApp.Logic.Handlers.Accounts;
using NoteBookApp.Server.Infrastructure;

namespace NoteBookApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController(IMapper mapper, INotifierMediatorService mediator)
        {
            Mapper = mapper;
            Mediator = mediator;
        }

        private IMapper Mapper { get; }
        private INotifierMediatorService Mediator { get; }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var query = new GetMyProfileQuery();
            var retValue = await Mediator.Send(query);
            return Ok(retValue);
        }
    }
}
