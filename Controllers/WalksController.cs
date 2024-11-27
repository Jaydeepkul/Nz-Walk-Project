using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repositories;

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalksRepository walksRepository;

        public WalksController(IMapper mapper, IWalksRepository walksRepository)
        {
            this.mapper = mapper;
            this.walksRepository = walksRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalksRequestDTO addWalksRequestDTO)
        {
            if(ModelState.IsValid) 
            {
                var walksDTO = mapper.Map<Walk>(addWalksRequestDTO);

                await walksRepository.CreateAsync(walksDTO);

                return Ok(mapper.Map<WalkDto>(walksDTO));
            }
            else
            {
                return BadRequest(ModelState);
            }
            
        }

        [HttpGet]
        //Get/api/walks?filterOn=name&fiterQuery=track&sortBy=name&Isassending=true
        //name is colunm and track is searching parameter.
        public async Task<IActionResult> GetAll([FromQuery] string? fiterOn, [FromQuery] string? fiterQuery, [FromQuery] string? sortBy, [FromQuery] bool? Isassending,
            [FromQuery] int Pagesize = 1000, [FromQuery]int PageNumber=1)
        {
            var result = await walksRepository.GetAllWalks(fiterOn, fiterQuery,sortBy,Isassending ?? true, Pagesize,PageNumber);
            return Ok(mapper.Map<List<WalkDto>>(result));
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await walksRepository.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(result));
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult>Update([FromRoute]Guid id, UpdateWalkDtoRequestModel updateWalkDtoRequestModel)
        {
            if(ModelState.IsValid) 
            {
                var walkDomainModel = mapper.Map<Walk>(updateWalkDtoRequestModel);
                walkDomainModel = await walksRepository.Update(id, walkDomainModel);
                if (walkDomainModel == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            }
            else { return NotFound(ModelState); }
            
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult>Delete(Guid id)
        {
            var result = mapper.Map<Walk>(id);
            result = await walksRepository.Delete(id);
            if (result == null)
            {
                return NotFound(id);    
            }
            return Ok(mapper.Map<WalkDto>($"Record {result.Name} Deleted Successfully"));
        }
    }
}
