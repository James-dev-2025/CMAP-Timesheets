﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Timesheets.Api.Features.TimesheetEntries;

[Route("api/timesheetEntries")]
[ApiController]
public class ApiController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApiController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<Create.Response> Create([FromBody] Create.Command request) => await _mediator.Send(request);

    [HttpPost("update")]
    public async Task<Update.Response> Update([FromBody] Update.Command request) => await _mediator.Send(request);

    [HttpPost("delete")]
    public async Task<Delete.Response> Delete([FromBody] Delete.Command request) => await _mediator.Send(request);

    [HttpGet("getCsv")]
    public async Task<GetCsv.Response> GetCsv([FromQuery] GetCsv.Query request) => await _mediator.Send(request);
}