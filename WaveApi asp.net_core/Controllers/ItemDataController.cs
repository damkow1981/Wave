using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WaveApi.Interfaces;
using WaveApi.Models;

namespace WaveApi.Controllers
{
    [Route("api/[controller]")]
    public class ItemDataController : Controller
    {
        private readonly IItemDataRepository _itemDataRepository;

        public ItemDataController(IItemDataRepository itemDataRepository)
        {
            _itemDataRepository = itemDataRepository;
        }

        [HttpGet("{id}")]
        public IActionResult List(string id)
        {
            return Ok(_itemDataRepository.All(id));
        }

        [HttpPost]
        public IActionResult Create([FromBody]ItemData data)
        {
            try
            {
                if (data == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.SomeFieldsRequired.ToString());
                }

                _itemDataRepository.Insert(data);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
            }
            return Ok(data);
        }

        [HttpPut]
        public IActionResult Edit([FromBody] ItemData data)
        {
            try
            {
                if (data == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.SomeFieldsRequired.ToString());
                }

                _itemDataRepository.Update(data);
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
                _itemDataRepository.Delete(id);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotDeleteItem.ToString());
            }
            return NoContent();
        }
    }


}