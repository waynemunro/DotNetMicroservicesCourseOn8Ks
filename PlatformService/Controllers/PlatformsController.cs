﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<PlatformsController> _logger;

    public PlatformsController(IPlatformRepo repository, IMapper mapper, ILogger<PlatformsController> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }


    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        _logger.LogInformation("In PlatformsController about to call GetPlatforms");
        
        var platformsToReturn = _repository.GetAllPlatforms();

        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>?>(platformsToReturn));
    }


}
