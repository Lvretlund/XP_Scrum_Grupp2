using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;
using XP_Scrum_Grupp2.Models;

namespace XP_Scrum_Grupp2
{
    public class BlogScheduler
    {
        public static void Start()
        {
            IJobDetail emailJob = JobBuilder.Create<EmailBlogg>()
                  .WithIdentity("job1")
                  .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(12)
                    .OnEveryDay()
                  )
                 .ForJob(emailJob)
                 .WithIdentity("trigger1")
                 .StartNow()
                 .WithCronSchedule("0 0 17 * * ?") // Time : Every 1 Minutes job execute
                 .Build();

            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sc = sf.GetScheduler();
            sc.ScheduleJob(emailJob, trigger);
            sc.Start();
        }
    }
}
