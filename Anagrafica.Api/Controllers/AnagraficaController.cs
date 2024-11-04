using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AnagraficaAPI.Domain.Command;
using AnagraficaAPI.Domain.Services;
using AnagraficaAPI.Request;
using AnagraficaAPI.Response;
using AutoMapper;
using ControllerUtilities.Request;
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
    public class AnagraficaController : ControllerBase
    {
        private readonly IAnagraficaService _anagraficaService;
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;

        public AnagraficaController(IAnagraficaService anagraficaService, IExportService exportService, IMapper mapper)
        {
            _anagraficaService = anagraficaService;
            _exportService = exportService;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetAnagrafica")]
        [ProducesResponseType(typeof(AnagraficaResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            AnagraficaResponseDto result = _mapper.Map<AnagraficaResponseDto>(await _anagraficaService.Get(id));
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [HttpGet("GetListByIds", Name = "GetListByIds")]
        [ProducesResponseType(typeof(IEnumerable<AnagraficaResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListByIds([FromQuery] IEnumerable<int> ids)
        {
            return Ok(_mapper.Map<IEnumerable<AnagraficaResponseDto>>(await _anagraficaService.Get(ids)));
        }

        [HttpGet("GetListByIdsForAgenda", Name = "GetListByIdsForAgenda")]
        [ProducesResponseType(typeof(IEnumerable<AnagraficaResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListByIdsForAgenda([FromQuery] IEnumerable<int> ids)
        {
            return Ok(_mapper.Map<IEnumerable<AnagraficaResponseDto>>(await _anagraficaService.GetForAgenda(ids)));
        }

        [HttpGet("GetList", Name = "GetList")]
        [ProducesResponseType(typeof(PagedViewDto<AnagraficaResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetList([FromQuery] AnagraficaFilterRequest request)
        {
            return Ok(_mapper.Map<PagedViewDto<AnagraficaResponseDto>>(await _anagraficaService.GetListByFilter(_mapper.Map<AnagraficaFilterCommand>(request))));
        }

        [HttpPost("Insert", Name = "Insert")]
        [ProducesResponseType(typeof(BaseResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody][Required] AnagraficaSaveRequest request)
        {
            return Ok(_mapper.Map<BaseResponseDto>(await _anagraficaService.Insert(_mapper.Map<AnagraficaSaveCommand>(request))));
        }

        [HttpPut("Update/{id}", Name = "Update")]
        [ProducesResponseType(typeof(BaseResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Update(int id, [FromBody][Required] AnagraficaSaveRequest request)
        {
            UpdatedModel updatedModel = await _anagraficaService.Update(id, _mapper.Map<AnagraficaSaveCommand>(request));
            return updatedModel.Result switch
            {
                ApiErrorCode.DbUpdateConcurrencyException => StatusCode(StatusCodes.Status412PreconditionFailed, "DbUpdateConcurrencyException"),
                ApiErrorCode.NotFound => NotFound(),
                _ => Ok(_mapper.Map<BaseResponseDto>(updatedModel))
            };
        }

        [HttpDelete("Delete/{id}", Name = "Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, [FromBody][Required] BaseUpdateRequest request)
        {
            UpdatedModel updatedModel = await _anagraficaService.Delete(id, request.RowVersion);
            return updatedModel.Result switch
            {
                ApiErrorCode.DbUpdateConcurrencyException => StatusCode(StatusCodes.Status412PreconditionFailed, "DbUpdateConcurrencyException"),
                ApiErrorCode.NotFound => NotFound(),
                _ => Ok(),
            };
        }

        [HttpGet("GetOrderColumnsAvailables", Name = "GetOrderColumnsAvailables")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderColumnsAvailables()
        {
            return Ok(await _anagraficaService.GetOrderColumnsAvailables());
        }

        [HttpPut("AddTagToCliente/{id}", Name = "AddTagToCliente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddTagToCliente(int id, [FromQuery][Required] int idTag)
        {
            return await _anagraficaService.AddTagToCliente(id, idTag) switch
            {
                ApiErrorCode.NotFound => NotFound(),
                _ => Ok()
            };
        }

        [HttpDelete("RemoveTagFromCliente/{id}", Name = "RemoveTagFromCliente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveTagFromCliente(int id, [FromQuery][Required] int idTag)
        {
            return await _anagraficaService.RemoveTagFromCliente(id, idTag) switch
            {
                ApiErrorCode.NotFound => NotFound(),
                _ => Ok()
            };
        }

        [HttpGet("GetExcelEstrazioneClienti", Name = "GetExcelEstrazioneClienti")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExcelEstrazioneClienti([FromQuery] AnagraficaFilterRequest request)
        {
            var data = await _exportService.GetExcelEstrazioneClienti(_mapper.Map<AnagraficaFilterCommand>(request));
            if (data == null)
                return NotFound();

            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileDownloadName: "EstrazioneClienti.xlsx");
        }

        [HttpGet("GetListSortByFreeText", Name = "GetListSortByFreeText")]
        [ProducesResponseType(typeof(IEnumerable<AnagraficaResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListSortByFreeText([FromQuery] string freeText)
        {
            return Ok(_mapper.Map<IEnumerable<AnagraficaResponseDto>>(await _anagraficaService.GetListSortByFreeText(freeText)));
        }
    }
}
