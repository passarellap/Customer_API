using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AnagraficaAPI.Domain.Command;
using AnagraficaAPI.Domain.Services;
using AnagraficaAPI.Request;
using AutoMapper;
using ControllerUtilities.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceUtilities.Model;
using TreePlat.Domain.Constants;

namespace AnagraficaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndirizzoController : ControllerBase
    {
        private readonly IIndirizzoService _indirizzoService;
        private readonly IMapper _mapper;
        private readonly ILogger<IndirizzoController> _logger;

        public IndirizzoController(IIndirizzoService indirizzoService, IMapper mapper, ILogger<IndirizzoController> logger)
        {
            _indirizzoService = indirizzoService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("InsertIndirizzo", Name = "InsertIndirizzo")]
        [ProducesResponseType(typeof(BaseResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InsertIndirizzo([FromBody][Required] IndirizzoSaveRequest request)
        {
            UpdatedModel updatedModel = await _indirizzoService.Insert(_mapper.Map<IndirizzoSaveCommand>(request));
            return updatedModel.Result switch
            {
                0 => StatusCode(StatusCodes.Status304NotModified),
                _ => Ok(_mapper.Map<BaseResponseDto>(updatedModel)),
            };
        }

        [HttpPut("UpdateIndirizzo/{id}", Name = "UpdateIndirizzo")]
        [ProducesResponseType(typeof(BaseResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> UpdateIndirizzo(int id, [FromBody][Required] IndirizzoSaveRequest request)
        {
            UpdatedModel updatedModel = await _indirizzoService.Update(id, _mapper.Map<IndirizzoSaveCommand>(request));
            return updatedModel.Result switch
            {
                ApiErrorCode.DbUpdateConcurrencyException => StatusCode(StatusCodes.Status412PreconditionFailed, "DbUpdateConcurrencyException"),
                ApiErrorCode.NotFound => NotFound(),
                _ => Ok(_mapper.Map<BaseResponseDto>(updatedModel))
            };
        }
    }
}
