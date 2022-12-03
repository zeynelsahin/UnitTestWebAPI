using AutoMapper;
using EmployeeManagement.Business;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers;
[Route("api/demonInternalEmployees")]
public class DemoInternalEmployeesController: ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;

    public DemoInternalEmployeesController(IEmployeeService employeeService, IMapper mapper)
    {
        _employeeService = employeeService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<InternalEmployeeDto>> CreateInternalEmployee(InternalEmployeeForCreationDto internalEmployeeForCreationDto)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var internalEmployee = await _employeeService.CreateInternalEmployeeAsync(internalEmployeeForCreationDto.FirstName, internalEmployeeForCreationDto.LastName);
        await _employeeService.AddInternalEmployeeAsync(internalEmployee);
        
        return CreatedAtAction("GetInternalEmployee",
            _mapper.Map<InternalEmployeeDto>(internalEmployee),
            new { employeeId = internalEmployee.Id } );
    }
    public async Task<ActionResult<InternalEmployeeDto>> GetInternalEmployee(
        Guid? employeeId)
    {
        if (!employeeId.HasValue)
        { 
            return NotFound(); 
        }

        var internalEmployee = await _employeeService.FetchInternalEmployeeAsync(employeeId.Value);
        if (internalEmployee == null)
        { 
            return NotFound();
        }             

        return Ok(_mapper.Map<InternalEmployeeDto>(internalEmployee));
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetProtectedInternalEmployees()
    {
        if (User.IsInRole("Admin"))
        {
            return BadRequest("You dont called this method");
        }

        return RedirectToAction("GetInternalEmployees", "InternalEmployees");
    }
}