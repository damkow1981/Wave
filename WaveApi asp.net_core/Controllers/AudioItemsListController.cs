using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WaveApi.Interfaces;
using WaveApi.Models;
using WaveApi.Services;

namespace WaveApi.Controllers
{
    [Route("api/[controller]")]
    public class AudioItemsListController : Controller
    {
        private readonly IAudioListRepository _audioListRepository;

        public AudioItemsListController(IAudioListRepository audioListRepository)
        {
            _audioListRepository = audioListRepository;
        }

        [HttpGet("{username}")]
        public IActionResult List(string username)
        {
            return Ok(_audioListRepository.All(username));
        }

        [HttpPost]
        public IActionResult Create([FromBody]AudioItem item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.SomeFieldsRequired.ToString());
                }
                bool itemExists = _audioListRepository.DoesItemExist(item);
                if (itemExists)
                {
                    return StatusCode(StatusCodes.Status409Conflict, ErrorCode.ItemInUse.ToString());
                }
                _audioListRepository.Insert(item);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
            }
            return Ok(item);
        }

        [HttpPut]
        public IActionResult Edit([FromBody] AudioItem item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.SomeFieldsRequired.ToString());
                }
                var existingItem = _audioListRepository.Find(item);
                if (existingItem == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }

                if (item.ToBeDeleted)
                {
                    _audioListRepository.Delete(item);
                }
                else
                {
                    _audioListRepository.Update(item);
                }
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotUpdateItem.ToString());
            }
            return NoContent();
        }
    }


}