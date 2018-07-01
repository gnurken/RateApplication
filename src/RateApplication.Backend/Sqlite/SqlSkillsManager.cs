using System;
using System.Collections.Generic;
using System.Linq;
using RateApplication.Backend.Skills;

namespace RateApplication.Backend.Sqlite
{
    public class SqlSkillsManager : ISkillsManager
    {
        private readonly SkillsDbContext _context;

        public int MaxRating { get; }

        public SqlSkillsManager(SkillsDbContext context, int maxRating)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            if (maxRating < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRating), "maxRating must be positive.");
            }

            MaxRating = maxRating;
        }

        public IReadOnlyList<Skill> GetSkills() => _context.Skills.ToList();

        public Skill GetSkill(int id)
        {
            return _context.Skills.FirstOrDefault(skill => skill.Id == id);
        }

        public Skill GetSkill(string name)
        {
            return _context.Skills.FirstOrDefault(skill => skill.Name == name);
        }

        public bool HasSkill(int id)
        {
            return _context.Skills.Any(skill => skill.Id == id);
        }

        public bool HasSkill(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            return _context.Skills.Any(skill => skill.Name == name);
        }

        public int AddSkill(Skill skill)
        {
            if (skill.Rating > MaxRating || skill.Rating < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(skill), $"Valid rating range (0-{MaxRating})");
            }

            if (HasSkill(skill.Name))
            {
                throw new ArgumentException($"A Skill with name '{skill.Name}' already exists",
                    nameof(skill));
            }

            _context.Skills.Add(skill);
            _context.SaveChanges();
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
            _context.SaveChanges();
        }

        public void RemoveSkill(int id)
        {
            var skill = GetSkill(id);
            if (skill == null)
            {
                throw new ArgumentException($"No Skill with id {id} exists.",
                    nameof(id));
            }

            _context.Skills.Remove(skill);
            _context.SaveChanges();
        }
    }
}