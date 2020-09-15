using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TEntity, TRepo> : ControllerBase
        where TEntity : class
        where TRepo : IRepo<TEntity>
    {
        IRepo<TEntity> _repo;
        public BaseController(TRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IEnumerable<TEntity>> GetAll() => await _repo.GetAll();

        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> GetId(int Id) => await _repo.GetID(Id);

        [HttpPost]
        public async Task<ActionResult<TEntity>> Create(TEntity entity)
        {
            var data = await _repo.Create(entity);
            if (data > 0)
            {
                return Ok("Create Successfully");
            }
            return BadRequest("Something Wrong! Please check again");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int Id)
        {
            var data = await _repo.Delete(Id);
            if (data.Equals(null))
            {
                return BadRequest("Something Wrong! Please check again");
            }
            return data;
        }
    }
}
