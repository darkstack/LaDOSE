using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LaDOSE.Business.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LaDOSE.Api.Controllers
{
    public abstract class GenericControllerDTO<T, TU, D> : Controller where TU : Entity.Context.Entity where T : IBaseService<TU>
    {
        protected IMapper _mapper;
        protected T _service;

        public GenericControllerDTO(IMapper mapper, T service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpPost]
        public virtual D Post([FromBody]D dto)
        {
            TU entity = _mapper.Map<TU>(dto);
            return _mapper.Map<D>(_service.AddOrUpdate(entity));
        }
        [HttpGet]
        public virtual List<D> Get()
        {

            return _mapper.Map<List<D>>(_service.GetAll().ToList());

        }
        [HttpGet("{id}")]
        public virtual D Get(int id)
        {
            return _mapper.Map<D>(_service.GetById(id));

        }
        [HttpDelete("{id}")]
        public virtual IActionResult Delete(int id)
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