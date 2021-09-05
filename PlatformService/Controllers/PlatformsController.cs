using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

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

    [HttpGet("{id}")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        _logger.LogInformation("In PlatformsController about to call GetPlatformById");

        var platformToReturn = _repository.GetPlatform(id);

        if (platformToReturn == null) return NotFound();

        return Ok(_mapper.Map<PlatformReadDto>(platformToReturn));
    }

    [HttpPost]
    public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto createDto)
    {
        _logger.LogInformation("In PlatformsController about to call CreatePlatform");

        if(createDto == null) return BadRequest();

        var mapped = _mapper.Map<PlatformCreateDto,Platform>(createDto);
        try
        {
            _repository.AddPlatform(mapped);
            _repository.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating platform: ", ex);
            return BadRequest(ex);
        }
        
        return Ok();
    }


}
