using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WaveApi.Interfaces;
using WaveApi.Models;

namespace WaveApi.Controllers
{
    [Route("api/[controller]")]
    public class CredentialsController : Controller
    {
        private readonly ICredentialsRepository _credentialsRepository;

        public CredentialsController(ICredentialsRepository credentialsRepository)
        {
            _credentialsRepository = credentialsRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_credentialsRepository.Get);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Credentials credentials)
        {
            try
            {
                if (credentials == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.SomeFieldsRequired.ToString());
                }                
                bool itemExists = _credentialsRepository.DoesItemExist(credentials.username);
                if (itemExists)
                {
                    return StatusCode(StatusCodes.Status409Conflict, ErrorCode.ItemInUse.ToString());
                }
                
                _credentialsRepository.Post(credentials);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
            }
            return Ok(credentials);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Credentials credentials)
        {
            try
            {
                if (credentials == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.SomeFieldsRequired.ToString());
                }
                var existingItem = _credentialsRepository.Find(credentials.username);
                if (existingItem == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }
                _credentialsRepository.Put(credentials);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotUpdateItem.ToString());
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var item = _credentialsRepository.Find(id);
                if (item == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }
                _credentialsRepository.Delete(id);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotDeleteItem.ToString());
            }
            return NoContent();
        }
    }
}