﻿using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace RateApplication.Backend
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureRequestContainer(
            TinyIoCContainer container,
            NancyContext context)
        {
            container.Register<ISkillsManager>(SkillsManager);
        }

        public static SkillsManager SkillsManager { private get; set; }

        // Enable CORS
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.AfterRequest += (ctx) =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,DELETE");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers", "Accept, Origin, Content-type");
            };
        }
    }
}
