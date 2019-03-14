using System;
using System.Collections.Generic;
using System.Linq;
using LaDOSE.Business.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LaDOSE.Api.Controllers
{
    public class GenericControllerDTO<T, TU, D> : Controller where TU : Entity.Context.Entity where T : IBaseService<TU>
    {
        protected T _service;

        public GenericControllerDTO(T service)
        {
            _service = service;
        }

        [HttpPost]
        public D Post([FromBody]D dto)
        {
            TU entity = AutoMapper.Mapper.Map<TU>(dto);
            return AutoMapper.Mapper.Map<D>(_service.AddOrUpdate(entity));
        }
        [HttpGet]
        public List<D> Get()
        {

            return AutoMapper.Mapper.Map<List<D>>(_service.GetAll().ToList());

        }
        [HttpGet("{id}")]
        public D Get(int id)
        {
            return AutoMapper.Mapper.Map<D>(_service.GetById(id));

        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                return _service.Delete((int)id) ? (IActionResult)NoContent() : NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}