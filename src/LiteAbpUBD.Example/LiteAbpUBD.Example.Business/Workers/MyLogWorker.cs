using Quartz;
using Volo.Abp.BackgroundWorkers.Quartz;

namespace LiteAbpUBD.Example.Business.Workers
{
    //public class MyLogWorker : QuartzBackgroundWorkerBase
    //{
    //    public MyLogWorker()
    //    {
    //        JobDetail = JobBuilder.Create<MyLogWorker>().WithIdentity(nameof(MyLogWorker)).Build();
    //        Trigger = TriggerBuilder.Create().WithIdentity(nameof(MyLogWorker)).WithSimpleSchedule(x => x
    //    .WithIntervalInSeconds(10)
    //    .RepeatForever()).Build();
    //    }

    //    public override Task Execute(IJobExecutionContext context)
    //    {
    //        System.Diagnostics.Debug.WriteLine(DateTime.Now + " --- Executed MyLogWorker..!");
    //        return Task.CompletedTask;
    //    }
    //}
}
