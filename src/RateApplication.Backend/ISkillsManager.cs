using System.Collections.Generic;

namespace RateApplication.Backend
{
    public interface ISkillsManager
    {
        IReadOnlyList<Skill> GetSkills();
        Skill GetSkill(int id);
        Skill GetSkill(string name);
        bool HasSkill(int id);
        bool HasSkill(string name);
        int AddSkill(Skill skill);
        void UpdateSkillRating(int id, int rating);
        void RemoveSkill(int id);
        int MaxRating { get; }
    }
}