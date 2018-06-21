using System;
using System.Collections.Generic;
using System.Linq;

namespace RateApplication.Backend
{
    public class SkillsManager : ISkillsManager
    {
        private readonly List<Skill> _skills = new List<Skill>();
        private int _nextId = 1;

        public int MaxRating { get; }

        public SkillsManager(int maxRating)
        {
            if (maxRating < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRating), "maxRating must be positive.");
            }

            MaxRating = maxRating;
        }

        public IReadOnlyList<Skill> GetSkills() => _skills;

        public Skill GetSkill(int id)
        {
            return _skills.FirstOrDefault(skill => skill.Id == id);
        }

        public Skill GetSkill(string name)
        {
            return _skills.FirstOrDefault(skill => skill.Name == name);
        }

        public bool HasSkill(int id)
        {
            return _skills.Any(skill => skill.Id == id);
        }

        public bool HasSkill(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            return _skills.Any(skill => skill.Name == name);
        }

        public int AddSkill(Skill skill)
        {
            if (skill == null) throw new ArgumentNullException(nameof(skill));
            if (skill.Rating > MaxRating || skill.Rating < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(skill), $"Valid rating range (0-{MaxRating})");
            }

            if (HasSkill(skill.Name))
            {
                throw new ArgumentException($"A Skill with name '{skill.Name}' already exists",
                    nameof(skill));
            }

            skill.Id = _nextId++;
            _skills.Add(skill);
            return skill.Id;
        }

        public void UpdateSkillRating(int id, int rating)
        {
            if (rating > MaxRating || rating < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rating), $"Valid rating range (0-{MaxRating})");
            }

            var skill = GetSkill(id);
            if (skill == null)
            {
                throw new ArgumentException($"No Skill with id {id} exists.",
                    nameof(id));
            }

            skill.Rating = rating;
        }

        public void RemoveSkill(int id)
        {
            var index = _skills.FindIndex(skill => skill.Id == id);
            if (index == -1)
            {
                throw new ArgumentException($"No Skill with id {id} exists.",
                    nameof(id));
            }

            _skills.RemoveAt(index);
        }
    }
}