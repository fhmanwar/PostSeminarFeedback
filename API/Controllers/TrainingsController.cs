using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Base;
using API.Models;
using API.Repository.Data;
using API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class TypeTrainsController : BaseController<TypeTraining, TypeTrainingRepo>
    {
        readonly TypeTrainingRepo _typeRepo;
        public TypeTrainsController(TypeTrainingRepo typeRepo) : base(typeRepo)
        {
            _typeRepo = typeRepo;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update(int id, TypeTraining entity)
        {
            var getId = await _typeRepo.GetID(id);
            getId.Name = entity.Name;
            var data = await _typeRepo.Update(getId);
            if (data.Equals(null))
            {
                return BadRequest("Something Wrong! Please check again");
            }
            return data;
        }
    }

    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingsController : BaseController<Training, TrainingRepo>
    {
        readonly TrainingRepo _trainRepo;
        public TrainingsController(TrainingRepo trainRepo) : base(trainRepo)
        {
            _trainRepo = trainRepo;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update(int id, Training entity)
        {
            var getId = await _trainRepo.GetID(id);
            getId.Title = entity.Title;
            getId.Target = entity.Target;
            getId.Location = entity.Location;
            getId.Schedule = entity.Schedule;
            getId.UserId = entity.UserId;
            getId.TypeId = entity.TypeId;
            var data = await _trainRepo.Update(getId);
            if (data.Equals(null))
            {
                return BadRequest("Something Wrong! Please check again");
            }
            return data;
        }
    }
}
