using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;

namespace RateApplication.Backend
{
    public class Module : NancyModule
    {
        private readonly ISkillsManager _skillsManager;

        public Module(ISkillsManager skillsManager)
        {
            _skillsManager = skillsManager ?? throw new ArgumentNullException(nameof(skillsManager));

            Get["test"] = _ => "test";

            Get["/skills"] = parameters => GetSkills();
            Get["/skills/{id:int}"] = parameters => GetSkill(GetId(parameters));
            Put["/skills/{id:int}/{rating:int}"] = parameters => UpdateSkillRating(GetId(parameters), GetRating(parameters));
            Post["/skills/"] = _ => AddSkill(this.Bind<Skill>());
            Delete["/skills/{id:int}"] = parameters => DeleteSkill(GetId(parameters));
        }

        private static int GetId(dynamic parameters)
        {
            return parameters.id;
        }

        private static int GetRating(dynamic parameters)
        {
            return parameters.rating;
        }

        private IReadOnlyList<Skill> GetSkills()
        {
            return _skillsManager.GetSkills();
        }

        private Response GetSkill(int id)
        {
            var skill = _skillsManager.GetSkill(id);
            return skill != null
                ? Response.AsJson(skill)
                : CreateResponse(HttpStatusCode.NotFound);
        }

        private Response UpdateSkillRating(int id, int rating)
        {
            if (rating > _skillsManager.MaxRating || rating < 0)
            {
                return CreateResponse($"Rating falls outside of valid range (0-{_skillsManager.MaxRating})",
                    HttpStatusCode.UnprocessableEntity);
            }

            if (!_skillsManager.HasSkill(id))
            {
                return CreateResponse(HttpStatusCode.NotFound);
            }
            
            _skillsManager.UpdateSkillRating(id, rating);
            return CreateResponse(HttpStatusCode.OK);
        }

        private Response DeleteSkill(int id)
        {
            if (!_skillsManager.HasSkill(id))
            {
                return CreateResponse(HttpStatusCode.NotFound);
            }

            _skillsManager.RemoveSkill(id);
            return CreateResponse(HttpStatusCode.OK);
        }

        private Response AddSkill(Skill skill)
        {
            if (skill.Rating > _skillsManager.MaxRating || skill.Rating < 0)
            {
                return CreateResponse($"Rating falls outside of valid range (0-{_skillsManager.MaxRating})",
                    HttpStatusCode.UnprocessableEntity);
            }
            if (_skillsManager.HasSkill(skill.Name))
            {
                return CreateResponse($"Skill with name '{skill.Name}' already exists.",
                    HttpStatusCode.Conflict);
            }

            var id = _skillsManager.AddSkill(skill);
            skill = _skillsManager.GetSkill(id);
            return Response.AsJson(skill).WithHeader(
                "Location",
                Context.Request.Url + $"/{id}");
        }

        private Response CreateResponse(HttpStatusCode statusCode)
        {
            return CreateResponse("", statusCode);
        }

        private Response CreateResponse(string message, HttpStatusCode statusCode)
        {
            return Response.AsJson(
                // Format to match Nancys default error messages
                new
                {
                    statusCode,
                    message,
                    details = ""
                },
                statusCode);
        }
    }
}
