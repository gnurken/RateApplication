using System;
using NUnit.Framework;
using RateApplication.Backend;

namespace RateApplication.Test
{
    [TestFixture]
    public class SkillsManagerTests
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(int.MaxValue)]
        public void ValidConstruction(int maxRating)
        {
            var skillsManager = new SkillsManager(maxRating);
            Assert.That(skillsManager.MaxRating, Is.EqualTo(maxRating));
        }

        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void InvalidConstructionThrows(int maxRating)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new SkillsManager(maxRating));
        }

        [TestCase(int.MaxValue)]
        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void AddOutOfRangeRatingThrows(int rating)
        {
            var skillsManager = new SkillsManager(0);
            var skill = new Skill
            {
                Name = "TestSkill",
                Rating = rating
            };
            Assert.Throws<ArgumentOutOfRangeException>(() => skillsManager.AddSkill(skill));
        }

        [Test]
        public void AddDuplicateNameSkillsThrows()
        {
            var skillsManager = new SkillsManager(0);
            var skill = new Skill
            {
                Name = "TestSkill",
                Rating = skillsManager.MaxRating
            };
            skillsManager.AddSkill(skill);
            Assert.Throws<ArgumentException>(() => skillsManager.AddSkill(skill));
        }

        [Test]
        public void AddMultipleSkillsGetSeparateIds()
        {
            var skillsManager = new SkillsManager(0);
            foreach (var postfix in new[] {1, 2})
            {
                skillsManager.AddSkill(new Skill
                {
                    Name = $"TestSkill{postfix}",
                    Rating = skillsManager.MaxRating
                });
            }

            var skills = skillsManager.GetSkills();
            Assert.That(skills.Count == 2);
            Assert.That(skills[0].Id, Is.Not.EqualTo(skills[1].Id));
        }

        [Test]
        public void GetSkillForNonExistantSkillReturnsNull()
        {
            var skillsManager = new SkillsManager(0);
            Assert.That(skillsManager.GetSkill(0), Is.Null);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(9)]
        [TestCase(10)]
        public void GetSkillGetsCorrectSkill(int rating)
        {
            var skillsManager = new SkillsManager(10);
            var id = skillsManager.AddSkill(new Skill
            {
                Name = "TestSkill",
                Rating = rating
            });
            var skill = skillsManager.GetSkill(id);
            Assert.That(skill, Is.Not.Null);
            Assert.That(skill.Value.Name, Is.EqualTo("TestSkill"));
            Assert.That(skill.Value.Rating, Is.EqualTo(rating));
        }

        [Test]
        public void UpdateSkillRatingForNonExistantSkillThrows()
        {
            var skillsManager = new SkillsManager(0);
            Assert.Throws<ArgumentException>(() => skillsManager.UpdateSkillRating(0, 0));
        }

        [Test]
        public void UpdateSkillRatingActuallyUpdatesSkillRating()
        {
            var skillsManager = new SkillsManager(0);
            Assert.Throws<ArgumentException>(() => skillsManager.UpdateSkillRating(0, 0));
        }

        [Test]
        public void GetSkillsEmptyAfterContruction()
        {
            var skillsManager = new SkillsManager(0);
            Assert.That(skillsManager.GetSkills(), Is.Empty);
        }

        [Test]
        public void RemoveSkillForNonExistantSkillThrows()
        {
            var skillsManager = new SkillsManager(0);
            Assert.Throws<ArgumentException>(() => skillsManager.RemoveSkill(0));
        }

        [Test]
        public void RemoveSkillActuallyRemovesSkill()
        {
            var skillsManager = new SkillsManager(0);
            var id = skillsManager.AddSkill(new Skill
            {
                Name = "TestSkill",
                Rating = skillsManager.MaxRating
            });
            skillsManager.RemoveSkill(id);
            Assert.That(skillsManager.GetSkills(), Is.Empty);
            Assert.That(skillsManager.GetSkill(id), Is.Null);
        }
    }
}
