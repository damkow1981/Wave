using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WaveApi.Interfaces;
using WaveApi.Models;

namespace WaveApi.Controllers
{
    [Route("api/[controller]")]
    public class PhotoController : Controller
    {
        private readonly IPhotoRepository _photoRepository;

        public PhotoController(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_photoRepository.Get);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Photo photo)
        {
            try
            {
                if (photo == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.SomeFieldsRequired.ToString());
                }                

                _photoRepository.Post(photo);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
            }
            return Ok(photo);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Photo photo)
        {
            try
            {
                if (photo == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.SomeFieldsRequired.ToString());
                }

                _photoRepository.Put(photo);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotUpdateItem.ToString());
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete()
        {
            return NoContent();
        }
    }
}