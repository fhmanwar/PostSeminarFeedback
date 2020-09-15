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

    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : BaseController<Question, QuestionRepo>
    {
        readonly QuestionRepo _questRepo;
        public QuestionsController(QuestionRepo questRepo) : base(questRepo)
        {
            _questRepo = questRepo;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update(int id, Question entity)
        {
            var getId = await _questRepo.GetID(id);
            getId.QuestionDesc = entity.QuestionDesc;
            getId.TrainingId = entity.TrainingId;
            var data = await _questRepo.Update(getId);
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
    public class FeedbacksController : BaseController<Feedback, FeedbackRepo>
    {
        readonly FeedbackRepo _feedbackRepo;
        public FeedbacksController(FeedbackRepo feedbackRepo) : base(feedbackRepo)
        {
            _feedbackRepo = feedbackRepo;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update(int id, Feedback entity)
        {
            var getId = await _feedbackRepo.GetID(id);
            getId.Review = entity.Review;
            getId.Rate = entity.Rate;
            getId.UserId = entity.UserId;
            getId.QuestionId = entity.QuestionId;
            var data = await _feedbackRepo.Update(getId);
            if (data.Equals(null))
            {
                return BadRequest("Something Wrong! Please check again");
            }
            return data;
        }
    }
}
