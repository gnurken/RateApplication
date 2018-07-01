using System;
using Nancy;

namespace RateApplication.Frontend
{
    public class Module : NancyModule
    {
        public Module()
        {
            var skillsHtml = Resources.skills.Replace("___SKILLS_API_URL___", SkillsApiUrl);

            Get["/skills.html"] = _ => skillsHtml;
        }

        public static string SkillsApiUrl { get; set; }
    }
}
