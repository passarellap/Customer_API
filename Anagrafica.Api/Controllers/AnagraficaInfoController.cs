using System;
using System.Diagnostics;
using System.Reflection;
using AutoMapper;
using ControllerUtilities.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceUtilities.SystemInfo;

namespace AnagraficaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnagraficaInfoController : ControllerBase
    {
        private readonly ISystemInfoApiService _sysemInfoApiService;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;

        public AnagraficaInfoController(ISystemInfoApiService sysemInfoApiService, IWebHostEnvironment environment, IMapper mapper, IHttpContextAccessor accessor)
        {
            _sysemInfoApiService = sysemInfoApiService;
            _environment = environment;
            _mapper = mapper;
            _accessor = accessor;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(InfoResponseDto), StatusCodes.Status200OK)]
        public IActionResult Info()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            return Ok(_mapper.Map<InfoResponseDto>(
                        _sysemInfoApiService.GetSystemInfo(executingAssembly, _environment.EnvironmentName, _environment.WebRootPath, Process.GetCurrentProcess(), Environment.MachineName, _accessor)));
        }


        [AllowAnonymous]
        [HttpGet("IsAlive", Name = "IsAlive")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult IsAlive()
        {
            return Ok($"Yes, I am alive : {DateTime.UtcNow}");
        }
    }
}
