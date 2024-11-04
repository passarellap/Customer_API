using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AnagraficaAPI.Domain.Command;
using AnagraficaAPI.Domain.Services;
using AnagraficaAPI.Request;
using AnagraficaAPI.Response;
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
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly ILogger<TagController> _logger;
        private readonly IMapper _mapper;

        public TagController(ITagService tagService, ILogger<TagController> logger, IMapper mapper)
        {
            _tagService = tagService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetTag")]
        [ProducesResponseType(typeof(TagResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            TagResponseDto result = _mapper.Map<TagResponseDto>(await _tagService.GetTag(id));
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [HttpGet("GetListBestMatchTagName", Name = "GetListBestMatchTagName")]
        [ProducesResponseType(typeof(IEnumerable<TagResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListBestMatchTagName([FromQuery] string tagname)
        {
            return Ok(_mapper.Map<IEnumerable<TagResponseDto>>(await _tagService.GetListBestMatchTagName(tagname)));
        }

        [HttpPost("InsertTag", Name = "InsertTag")]
        [ProducesResponseType(typeof(BaseResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        public async Task<IActionResult> Insert([FromBody][Required] TagSaveRequest request)
        {
            UpdatedModel updatedModel = await _tagService.Insert(_mapper.Map<TagSaveCommand>(request));
            return updatedModel.Result switch
            {
                0 => StatusCode(StatusCodes.Status304NotModified),
                _ => Ok(_mapper.Map<BaseResponseDto>(updatedModel)),
            };
        }

        [HttpPut("UpdateTag/{id}", Name = "UpdateTag")]
        [ProducesResponseType(typeof(BaseResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> UpdateTag(int id, [FromBody][Required] TagSaveRequest request)
        {
            UpdatedModel updatedModel = await _tagService.Update(id, _mapper.Map<TagSaveCommand>(request));
            return updatedModel.Result switch
            {
                ApiErrorCode.DbUpdateConcurrencyException => StatusCode(StatusCodes.Status412PreconditionFailed, "DbUpdateConcurrencyException"),
                ApiErrorCode.NotFound => NotFound(),
                _ => Ok(_mapper.Map<BaseResponseDto>(updatedModel))
            };
        }

        [HttpDelete("DeleteTag/{id}", Name = "DeleteTag")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTag(int id)
        {
            return await _tagService.DeleteTag(id) switch
            {
                ApiErrorCode.NotFound => NotFound(),
                _ => Ok(),
            };
        }

        [HttpGet("GetRecentTags", Name = "GetRecentTags")]
        [ProducesResponseType(typeof(IEnumerable<TagResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecentTags()
        {
            return Ok(await _tagService.GetRecentTags());
        }

        [HttpGet("GetPagedListBestMatchTagName", Name = "GetPagedListBestMatchTagName")]
        [ProducesResponseType(typeof(PagedViewDto<TagResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPagedListBestMatchTagName([FromQuery] string tagName)
        {
            return Ok(_mapper.Map<PagedViewDto<TagResponseDto>>(await _tagService.GetPagedListBestMatchTagName(tagName)));
        }
    }
}
