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
    [Route("api/v{version:apiVersion}/job-positions")]
    [ApiController]
    [ApiVersion("1.0")]
    public class JobPositionController : ControllerBase
    {
        private readonly IJobPositionRepository _jobPositionRepository;
        private readonly IMapper _mapper;

        public JobPositionController(IJobPositionRepository jobPositionRepository, IMapper mapper)
        {
            _jobPositionRepository = jobPositionRepository; 
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<IEnumerable<JobPositionRespDto>>))]
        public IActionResult GetAllJobPosition()
        {
            var jobPositions = _jobPositionRepository.GetAll();
            var jobPositionsRespDtos = _mapper.Map<List<JobPositionRespDto>>(jobPositions);
            var resp = new BaseResponse<List<JobPositionRespDto>>()
            {
                Code = StatusCodes.Status200OK,
                Status = StatusMessage.Success,
                Data = jobPositionsRespDtos
            };
            return Ok(resp);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<JobPositionRespDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponse<string?>))]
        public IActionResult GetById([FromRoute(Name = "id")] int id)
        {
            var jobPosition = _jobPositionRepository.GetById(id);
            var jobPositionRespDtos = _mapper.Map<JobPositionRespDto>(jobPosition);
            var resp = new BaseResponse<JobPositionRespDto>();

            if (jobPosition == null)
            {
                resp.Code = StatusCodes.Status404NotFound;
                resp.Status = StatusMessage.NotFound;
                resp.Data = jobPositionRespDtos;
                return StatusCode(StatusCodes.Status404NotFound, resp);
            }

            resp.Code = StatusCodes.Status200OK;
            resp.Status = StatusMessage.Success;
            resp.Data = jobPositionRespDtos;
            return Ok(resp);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BaseResponse<string?>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateJobPosition([FromBody] JobPositionReqDto jobPositionReq)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var resp = new BaseResponse<string?>();

            var jobPosition = _mapper.Map<JobPosition>(jobPositionReq);

            try
            {
                _jobPositionRepository.Create(jobPosition);
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
        public IActionResult UpdateJobPosition([FromBody] JobPositionReqDto jobPositionReq, [FromRoute(Name = "id")] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var resp = new BaseResponse<string?>();
            int rowsAffected;

            var jobPosition = _mapper.Map<JobPosition>(jobPositionReq);

            try
            {
                rowsAffected = _jobPositionRepository.Update(jobPosition, id);
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
                rowsAffected = _jobPositionRepository.Delete(id);
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
