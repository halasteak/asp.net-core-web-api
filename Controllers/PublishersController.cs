﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using my_books.ActionResults;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using my_books.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private PublishersService _publisherService;
        private readonly ILogger<PublishersController> _logger;

        public PublishersController(PublishersService publisherService, ILogger<PublishersController> logger)
        {
            _publisherService = publisherService;
            _logger = logger;
        }


        [HttpGet("get-all-publishers")]
        public IActionResult GetAllPublishers(string sortBy, string searchString, int? pageNr) 
        {
            try
            {
                _logger.LogInformation("This is just a log in getAllPublishers");
                var _result = _publisherService.GetAllPublishers(sortBy, searchString, pageNr);
                return Ok(_result);
            }
            catch (Exception ex)
            {
                return BadRequest("sorry, we could not load the publishers");
            }
        }

        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody] PublisherVM publisher)
        {
            try
            {
                var newPublisher = _publisherService.AddPublisher(publisher);
                return Created(nameof(AddPublisher), newPublisher);
            }
            catch (PublisherNameException ex) 
            {
                return BadRequest($"{ex.Message}, Publisher name:{publisher.Name}");
            }
            catch (Exception ex)
            {
                return BadRequest("custom message");
            }
            
        }
        
        [HttpGet("get-publisher-by-id/{id}")]
        public IActionResult GetPublisherById(int id)
        {
            var _response = _publisherService.GetPublisherById(id);
            if (_response != null)
            {
                return Ok(_response);
               // return _response;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("get-publisher-books-with-authors/{id}")]
        public IActionResult GetPublisherData(int id) {
            var _response = _publisherService.GetPublisherData(id);
            return Ok(_response);
        }


        [HttpDelete("delete-publisher-by-id/{id}")]
        public IActionResult DeletePublisherById(int id) {

            try
            {
                _publisherService.DeletePublisherById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Custom Message, ex:" + ex.Message);
            }
        }
    }
}
