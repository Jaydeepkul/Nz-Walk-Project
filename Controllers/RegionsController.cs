using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repositories;
using NZWalks.ValidateModelAttribute_CustomValidation_;
using System.Collections.Generic;
using System.Text.Json;

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    //[Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext,IRegionRepository regionRepository,IMapper mapper,ILogger<RegionsController> logger) 
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        //[Authorize(Roles ="Reader,Writer")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                throw new Exception("Custome Exception"); 

                //GET DATA FROM DATABASE - DOMAIN MODELS
                var RegionDomains = await regionRepository.GetAllAsync();
                logger.LogInformation("Get all region action method was invoked");

                // MAP DOMAIN MODELS TO DTO
                var regionDTO = mapper.Map<List<RegionDTO>>(RegionDomains);
                logger.LogInformation($"finished GetAll Region method with data:{JsonSerializer.Serialize(regionDTO)}");

                // Return that DTO
                return Ok(regionDTO);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }


        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "Reader, Writer")]
        public async Task<IActionResult> GetById(Guid id) 
        {
            //var result = dbContext.Regions.Find(id);

            // Get the Region Domain model from Databse
            var RegionDomain = await regionRepository.GetById(id);
            if (RegionDomain == null)
            {
                return NotFound();
            }

            //Map/Convert the Region Domain Model to Region DTO
            //var regionDTO = new RegionDTO
            //{
            //    Id = RegionDomain.Id,
            //    Name = RegionDomain.Name,
            //    Code = RegionDomain.Code,
            //    RegionImgUrl = RegionDomain.RegionImgUrl
            //};
            var regionDTO  = mapper.Map<RegionDTO>(RegionDomain);
            //Return DTO back to the client.
            return Ok(regionDTO);
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create(AddRegionRequestDTO addRegionRequestDTO)
        {
            //Map or Convert DTO to Domain Model
            //var regionDomainModel = new Region
            //{
            //    Code = addRegionRequestDTO.Code,
            //    Name = addRegionRequestDTO.Name,
            //    RegionImgUrl = addRegionRequestDTO.RegionImgUrl
            //};
            if(ModelState.IsValid) { 
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDTO);
            //use Domain Model to Create Region
             regionDomainModel =await regionRepository.Create(regionDomainModel);
           

            //map Domain Model back to DTO
            //var regionDTO = new RegionDTO
            //{
            //    Id = regionDomainModel.Id,
            //    Name = regionDomainModel.Name,
            //    Code = regionDomainModel.Code,
            //    RegionImgUrl = regionDomainModel.RegionImgUrl
            //};
            var regionDTO= mapper.Map<RegionDTO>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDTO.Id }, regionDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "Writer")]
        //[ValidateModel] Custume validation
        public async Task<IActionResult> Update ([FromRoute]Guid id,UpdateRegionResquestDTO updateRegionResquestDTO)
        {
            //var regionDomainModel = new Region
            //{
            //    Code = updateRegionResquestDTO.Code,
            //    Name = updateRegionResquestDTO.Name,
            //    RegionImgUrl = updateRegionResquestDTO.RegionImgUrl
            //};
            if (ModelState.IsValid) 
            {
                var regionDomainModel = mapper.Map<Region>(updateRegionResquestDTO);
                regionDomainModel = await regionRepository.Update(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }
                //mAP dto TO DOMAIN
                //// regionDomainModel.Name= updateRegionResquestDTO.Name;
                //// regionDomainModel.Code= updateRegionResquestDTO.Code;
                //// regionDomainModel.RegionImgUrl= updateRegionResquestDTO.RegionImgUrl;

                ////await dbContext.SaveChangesAsync();

                //Convert Domain model to DTO
                //var regionDTO = new RegionDTO
                //{
                //    Id = regionDomainModel.Id,
                //    Name = regionDomainModel.Name,
                //    Code = regionDomainModel.Code,
                //    RegionImgUrl = regionDomainModel.RegionImgUrl

                //};
                var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);
                return Ok(regionDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
            
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete ([FromRoute]Guid id) 
        {
            var regionDomainModel =await regionRepository.Delete(id);
            if(regionDomainModel == null)
            {
                return NotFound();

            }
            
            return Ok("Delete record succssfullly");
        }
    }
}
