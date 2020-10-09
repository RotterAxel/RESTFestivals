using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Festivals.API.Infrastructure.Entities;
using Festivals.API.Models;
using Festivals.API.ResourceParameters;
using Festivals.API.Service;
using Microsoft.AspNetCore.Authorization;

namespace Festivals.API.Controllers
{
    [ApiController]
    [Route("api/festivals")]
    public class FestivalController : ControllerBase
    {
        private readonly IMedievalFestivalsRepository _medievalFestivalsRepository;
        private readonly IMapper _mapper;
        private IPropertyMappingService _propertyMappingService;

        public FestivalController(IMedievalFestivalsRepository medievalFestivalsRepository,
            IMapper mapper, 
            IPropertyMappingService propertyMappingService)
        {
            _medievalFestivalsRepository = medievalFestivalsRepository ??
                throw new ArgumentNullException(nameof(medievalFestivalsRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _propertyMappingService = propertyMappingService ??
              throw new ArgumentNullException(nameof(propertyMappingService));
        }

        [HttpGet(Name = "GetFestivals")]
        [HttpHead]
        public IActionResult GetFestivals(
            [FromQuery] FestivalsResourceParameters festivalsResourceParameters)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<FestivalDto, Festival>
                (festivalsResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            var festivalsFromRepo = _medievalFestivalsRepository.GetFestivals(festivalsResourceParameters);

            var paginationMetadata = new
            {
                totalCount = festivalsFromRepo.TotalCount,
                pageSize = festivalsFromRepo.PageSize,
                currentPage = festivalsFromRepo.CurrentPage,
                totalPages = festivalsFromRepo.TotalPages
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var shapedAuthors = _mapper.Map<IEnumerable<FestivalDto>>(festivalsFromRepo);

            return Ok(shapedAuthors);
        }

        [Produces("application/json",
            "application/vnd.mimo.festival.full+json")]
        [HttpGet("{festivalId}", Name = "GetFestival")]
        public ActionResult<FestivalFullDto> GetFestival(int festivalId,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType,
                out _))
            {
                return BadRequest();
            }


            var festivalsFromRepo = _medievalFestivalsRepository.GetFestivalFull(festivalId);

            if (festivalsFromRepo == null)
            {
                return NotFound();
            }

            var festivalFullDto = _mapper.Map<FestivalFullDto>(festivalsFromRepo);

            return Ok(festivalFullDto);
        }

        [HttpPost(Name = "CreateFestival")]
        [Produces("application/json",
            "application/vnd.mimo.festival.full+json")]
        [Consumes("application/json",
            "application/vnd.mimo.festivalforcreation+json")]
        public ActionResult<FestivalFullDto> CreateFestival(FestivalForCreationDto festival)
        {
            var festivalEntity = _mapper.Map<Festival>(festival);
            _medievalFestivalsRepository.AddFestival(festivalEntity);
            _medievalFestivalsRepository.Save();

            var festivalToReturn = _mapper.Map<FestivalFullDto>(festivalEntity);

            return CreatedAtRoute("GetFestival",
                new { festivalId = festivalToReturn.Id },
                festivalToReturn);
        }

        [Authorize(Roles = "Festival_Admin")]
        [HttpPatch("{festivalId}/adresses/{adressId}")]
        [Consumes("application/json-patch+json")]
        public ActionResult PartiallyUpdateFestival(int festivalId,
            int adressId,
            JsonPatchDocument<FestivalForUpdateDto> patchDocument)
        {
            if (!_medievalFestivalsRepository.FesitvalExists(festivalId) ||
            !_medievalFestivalsRepository.AdressExists(adressId))
            {
                return NotFound();
            }

            var festivalFromRepo = _medievalFestivalsRepository.GetFestivalFull(festivalId);

            var festivalToPatch = _mapper.Map<FestivalForUpdateDto>(festivalFromRepo);
            // add validation
            patchDocument.ApplyTo(festivalToPatch, ModelState);

            if (!TryValidateModel(festivalToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(festivalToPatch, festivalFromRepo);

            _medievalFestivalsRepository.UpdateFestival(festivalFromRepo);

            _medievalFestivalsRepository.Save();

            return NoContent();
        }

        //private IEnumerable<LinkDto> CreateLinksForFestivals(
        //    MedievalFestivalsResourceParameters medievalFestivalsResourceParameters,
        //    bool hasNext, bool hasPrevious)
        //{
        //    var links = new List<LinkDto>();

        //    // self 
        //    links.Add(
        //       new LinkDto(CreateFestivalsResourceUri(
        //           medievalFestivalsResourceParameters, ResourceUriType.Current)
        //       , "self", "GET"));

        //    if (hasNext)
        //    {
        //        links.Add(
        //          new LinkDto(CreateFestivalsResourceUri(
        //              medievalFestivalsResourceParameters, ResourceUriType.NextPage),
        //          "nextPage", "GET"));
        //    }

        //    if (hasPrevious)
        //    {
        //        links.Add(
        //            new LinkDto(CreateFestivalsResourceUri(
        //                medievalFestivalsResourceParameters, ResourceUriType.PreviousPage),
        //            "previousPage", "GET"));
        //    }

        //    return links;
        //}

        //private string CreateFestivalsResourceUri(
        //   MedievalFestivalsResourceParameters medievalFestivalsResourceParameters,
        //   ResourceUriType type)
        //{
        //    switch (type)
        //    {
        //        case ResourceUriType.PreviousPage:
        //            return Url.Link("GetFestivals",
        //              new
        //              {
        //                  orderBy = medievalFestivalsResourceParameters.OrderBy,
        //                  pageNumber = medievalFestivalsResourceParameters.PageNumber - 1,
        //                  pageSize = medievalFestivalsResourceParameters.PageSize,
        //                  todayOrLater = medievalFestivalsResourceParameters.todayOrLater,
        //                  searchQuery = medievalFestivalsResourceParameters.SearchQuery
        //              });
        //        case ResourceUriType.NextPage:
        //            return Url.Link("GetFestivals",
        //              new
        //              {
        //                  orderBy = medievalFestivalsResourceParameters.OrderBy,
        //                  pageNumber = medievalFestivalsResourceParameters.PageNumber + 1,
        //                  pageSize = medievalFestivalsResourceParameters.PageSize,
        //                  searchQuery = medievalFestivalsResourceParameters.SearchQuery
        //              });
        //        case ResourceUriType.Current:
        //        default:
        //            return Url.Link("GetFestivals",
        //            new
        //            {
        //                orderBy = medievalFestivalsResourceParameters.OrderBy,
        //                pageNumber = medievalFestivalsResourceParameters.PageNumber,
        //                pageSize = medievalFestivalsResourceParameters.PageSize,
        //                searchQuery = medievalFestivalsResourceParameters.SearchQuery
        //            });
        //    }

        //}
    }
}
