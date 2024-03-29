﻿using System;
using System.Collections.Generic;
using System.Linq;
using LaDOSE.Business.Interface;
using LaDOSE.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaDOSE.Api.Controllers
{
    public abstract class GenericController<T, TU> : Controller where TU : Entity.Context.Entity where T : IBaseService<TU>
    {
        protected T _service;

        public GenericController(T service)
        {
            _service = service;
        }

        [HttpPost]
        public virtual TU Post([FromBody] TU dto)
        {
            return _service.AddOrUpdate(dto);
        }

        [HttpGet]
        public virtual List<TU> Get()
        {

            return _service.GetAll().ToList();

        }

        [HttpGet("{id}")]
        public virtual TU Get(int id)
        {
            return _service.GetById(id);

        }
     
        [HttpDelete("{id}")]
        public virtual IActionResult Delete(int id)
        {
            try
            {
                return _service.Delete((int) id) ? (IActionResult) NoContent() : NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            

        }
    }
}