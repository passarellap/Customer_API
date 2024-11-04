using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AnagraficaAPI.Domain.Command;
using AnagraficaAPI.Domain.Services;
using AnagraficaAPI.Request;
using AnagraficaAPI.Response;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TreePlat.Domain.Constants;

namespace AnagraficaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentoController : ControllerBase
    {
        private readonly IDocumentoService _documentoService;
        private readonly ILogger<DocumentoController> _logger;
        private readonly IMapper _mapper;

        public DocumentoController(IDocumentoService documentoService, ILogger<DocumentoController> logger, IMapper mapper)
        {
            _documentoService = documentoService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost(Name = "InsertDocumento")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        public async Task<IActionResult> Insert(DocumentoSaveRequest request)
        {
            int? result = await _documentoService.Insert(_mapper.Map<DocumentoSaveCommand>(request));
            return result switch
            {
                ApiErrorCode.NotFound => NotFound(),
                null => StatusCode(StatusCodes.Status304NotModified),
                _ => Ok(result),
            };
        }

        [HttpDelete("{id}", Name = "DeleteDocumento")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _documentoService.Remove(id);
            return result switch
            {
                ApiErrorCode.NotFound => NotFound(),
                0 => StatusCode(StatusCodes.Status304NotModified),
                _ => Ok()
            };
        }

        [HttpPut("{id}", Name = "UpdateDocumento")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        public async Task<IActionResult> UpdateDocumento(int id, [FromBody][Required] DocumentoSaveRequest request)
        {
            int? result = await _documentoService.Update(id, _mapper.Map<DocumentoSaveCommand>(request));
            return result switch
            {
                ApiErrorCode.NotFound => NotFound(),
                _ => Ok(),
            };
        }

        [HttpGet("GetDocumento", Name = "GetDocumento")]
        [ProducesResponseType(typeof(DocumentoResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            DocumentoResponseDto result = _mapper.Map<DocumentoResponseDto>(await _documentoService.GetDocumento(id));
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }
    }
}
