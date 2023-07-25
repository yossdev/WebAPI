using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WebAPI.DTOs;
using WebAPI.DTOs.Request;
using WebAPI.DTOs.Response;
using WebAPI.Errors;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/job-titles")]
    [ApiController]
    [ApiVersion("1.0")]
    public class JobTitleController : ControllerBase
    {
        private readonly IJobTitleRepository _jobTitleRepository;
        private readonly IMapper _mapper;

        public JobTitleController(IJobTitleRepository jobTitleRepository, IMapper mapper)
        {
            _jobTitleRepository = jobTitleRepository; 
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<IEnumerable<JobTitleRespDto>>))]
        public IActionResult GetAllJobTitle()
        {
            var jobTitles = _jobTitleRepository.GetAll();
            var jobTitlesRespDtos = _mapper.Map<List<JobTitleRespDto>>(jobTitles);
            BaseResponse<List<JobTitleRespDto>> resp = new BaseResponse<List<JobTitleRespDto>>()
            {
                Code = StatusCodes.Status200OK,
                Status = StatusMessage.Success,
                Data = jobTitlesRespDtos
            };
            return Ok(resp);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<JobTitleRespDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponse<string?>))]
        public IActionResult GetById([FromRoute(Name = "id")] int id)
        {
            var jobTitle = _jobTitleRepository.GetById(id);
            var jobTitleRespDtos = _mapper.Map<JobTitleRespDto>(jobTitle);
            var resp = new BaseResponse<JobTitleRespDto>();

            if (jobTitle == null)
            {
                resp.Code = StatusCodes.Status404NotFound;
                resp.Status = StatusMessage.NotFound;
                resp.Data = jobTitleRespDtos;
                return StatusCode(StatusCodes.Status404NotFound, resp);
            }

            resp.Code = StatusCodes.Status200OK;
            resp.Status = StatusMessage.Success;
            resp.Data = jobTitleRespDtos;
            return Ok(resp);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BaseResponse<string?>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateJobTitle([FromBody] JobTitleReqDto jobTitleReq)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var resp = new BaseResponse<string?>();

            var jobTitle = _mapper.Map<JobTitle>(jobTitleReq);

            try
            {
                _jobTitleRepository.Create(jobTitle);
            }
            catch (NpgsqlException ex)
            {
                resp.Code = StatusCodes.Status400BadRequest;
                resp.Status = ex.Message;
                return StatusCode(StatusCodes.Status400BadRequest, resp);
            }

            resp.Code = StatusCodes.Status201Created;
            resp.Status = StatusMessage.Created;
            return StatusCode(StatusCodes.Status201Created, resp);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<string?>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateJobTitle([FromBody] JobTitleReqDto jobTitleReq, [FromRoute(Name = "id")] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resp = new BaseResponse<string?>();
            int rowsAffected;

            var jobTitle = _mapper.Map<JobTitle>(jobTitleReq);

            try
            {
                rowsAffected = _jobTitleRepository.Update(jobTitle, id);
            }
            catch (NpgsqlException ex)
            {
                resp.Code = StatusCodes.Status400BadRequest;
                resp.Status = ex.Message;
                return StatusCode(StatusCodes.Status400BadRequest, resp);
            }

            resp.Code = StatusCodes.Status200OK;
            resp.Status = StatusMessage.Updated;
            if (rowsAffected == 0)
            {
                resp.Status = $"{rowsAffected} rows affected";
            }
            return StatusCode(StatusCodes.Status200OK, resp);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<string?>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Delete([FromRoute(Name = "id")] int id)
        {
            var resp = new BaseResponse<string?>();
            int rowsAffected;

            try
            {
                rowsAffected = _jobTitleRepository.Delete(id);
            }
            catch(NpgsqlException ex)
            {
                resp.Code = StatusCodes.Status400BadRequest;
                resp.Status = ex.Message;
                return StatusCode(StatusCodes.Status400BadRequest, resp);
            }

            resp.Code = StatusCodes.Status200OK;
            resp.Status = StatusMessage.Deleted;
            if (rowsAffected == 0)
            {
                resp.Status = $"{rowsAffected} rows affected";
            }
            return StatusCode(StatusCodes.Status200OK, resp);
        }
    }
}
