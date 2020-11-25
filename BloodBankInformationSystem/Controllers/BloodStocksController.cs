﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloodLibrary.Entities;
using BloodBankInformationSystem.Services;
using AutoMapper;
using BloodBankInformationSystem.Models;
using BloodBankInformationSystem.Models.BloodStock;
using Microsoft.AspNetCore.Cors;

namespace BloodBankInformationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodStocksController : ControllerBase
    {
        private readonly IBloodStockInfoRepository _repository;
        private readonly IMapper _mapper;

        public BloodStocksController(IBloodStockInfoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/BloodStocks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BloodStock>>> GetStock()
        {
            var results = await _repository.GetList();

            return Ok(results);
        }

        // GET: api/BloodStocks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BloodStock>> GetBloodStock(int id)
        {
            var results = await _repository.GetById(id);
            var item = _mapper.Map<BloodStockDto>(results);
            return Ok(item);
        }

        // PUT: api/BloodStocks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBloodStock(int id, BloodStockUpdateDto bloodStock)
        {
            if (id != bloodStock.Id)
            {
                return BadRequest();
            }

            try
            {
                var item = _mapper.Map<BloodStock>(bloodStock);
                await _repository.Update(item);
            }
            catch (DbUpdateConcurrencyException)
            {
                var t = await BloodStockExists(id);
                if (!t)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BloodStocks
        [HttpPost]
        public async Task<ActionResult<BloodStockDto>> PostBloodStock(BloodStockCreateDto bloodStock)
        {
            //var item = this._mapper.Map<BloodStock>(bloodStock);
            //await this._repository.Add(item);
            //var dto = this._mapper.Map<BloodStockDto>(item);
            //return CreatedAtAction("GetBloodStock", new { id = dto.Id }, dto);

            if (bloodStock == null) return BadRequest();



            if (!ModelState.IsValid) return BadRequest(ModelState);


            var finalBloodStock = _mapper.Map<BloodStock>(bloodStock);

            await _repository.Add(finalBloodStock);


            if (!await _repository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdBloodStockToReturn = _mapper.Map<BloodStockDto>(finalBloodStock);

            return CreatedAtAction("GetBloodStock", new { id = createdBloodStockToReturn.Id }, createdBloodStockToReturn);

        }

        // DELETE: api/BloodStocks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BloodStock>> DeleteBloodStock(int id)
        {
            //var bloodStock = await _repository.GetById(id);
            //if (bloodStock == null)
            //{
            //    return NotFound();
            //}

            //_repository.Delete(bloodStock);

            //return bloodStock;

            if (!await _repository.Exists(id)) return NotFound();

            BloodStock bloodStockEntity2Delete = await _repository.GetById(id);
            if (bloodStockEntity2Delete == null) return NotFound();

            _repository.Delete(bloodStockEntity2Delete);

            if (!await _repository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();

        }

        private async Task<bool> BloodStockExists(int id)
        {
            return await _repository.Exists(id);
        }
    }
}
