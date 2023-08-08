using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WebAPI.DTOs;
using WebAPI.DTOs.Request;
using WebAPI.DTOs.Response;
using WebAPI.Errors;
using WebAPI.Interfaces;
using WebAPI.Models;
using WebAPI.Utils;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/employees")]
    [ApiController]
    [ApiVersion("1.0")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository; 
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<IEnumerable<EmployeeRespDto>>))]
        public IActionResult GetAllEmployee()
        {
            var employees = _employeeRepository.GetAll();
            var employeesRespDtos = _mapper.Map<List<EmployeeRespDto>>(employees);
            var resp = new BaseResponse<List<EmployeeRespDto>>()
            {
                Code = StatusCodes.Status200OK,
                Status = StatusMessage.Success,
                Data = employeesRespDtos
            };
            return Ok(resp);
        }

        [HttpGet("{nik}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<EmployeeRespDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponse<string?>))]
        public IActionResult GetByNIK([FromRoute(Name = "nik")] string nik)
        {
            nik = Helper.ProcessNIK(nik);
            var employee = _employeeRepository.GetByNIK(nik);
            var employeeRespDtos = _mapper.Map<EmployeeRespDto>(employee);
            var resp = new BaseResponse<EmployeeRespDto>();

            if (employee == null)
            {
                resp.Code = StatusCodes.Status404NotFound;
                resp.Status = StatusMessage.NotFound;
                resp.Data = employeeRespDtos;
                return StatusCode(StatusCodes.Status404NotFound, resp);
            }

            resp.Code = StatusCodes.Status200OK;
            resp.Status = StatusMessage.Success;
            resp.Data = employeeRespDtos;
            return Ok(resp);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BaseResponse<string?>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateEmployee([FromBody] EmployeeReqDto employeeReq)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var resp = new BaseResponse<string?>();

            var employee = _mapper.Map<Employee>(employeeReq);

            try
            {
                _employeeRepository.Create(employee);
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

        [HttpPut("{nik}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<string?>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateEmployee([FromBody] EmployeeReqDto employeeReq, [FromRoute(Name = "nik")] string nik)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            nik = Helper.ProcessNIK(nik);
            var resp = new BaseResponse<string?>();
            int rowsAffected;

            var employee = _mapper.Map<Employee>(employeeReq);

            try
            {
                rowsAffected = _employeeRepository.Update(employee, nik);
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

        [HttpDelete("{nik}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<string?>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Delete([FromRoute(Name = "nik")] string nik)
        {
            nik = Helper.ProcessNIK(nik);
            var resp = new BaseResponse<string?>();
            int rowsAffected;

            try
            {
                rowsAffected = _employeeRepository.Delete(nik);
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
