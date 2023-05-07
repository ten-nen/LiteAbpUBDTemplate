

using Volo.Abp.Domain.Entities.Auditing;

namespace LiteAbpUBD.Example.DataAccess.Entities
{
    public class ActivityInfo : FullAuditedAggregateRoot<Guid>
    {
        public ActivityInfo() : base()
        {

        }
        public ActivityInfo(Guid id) : base(id)
        {

        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 集合地址
        /// </summary>
        public string CollectionAddress { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 报名截至时间
        /// </summary>
        public DateTime EntryEndTime { get; set; }
        /// <summary>
        /// 最大人数
        /// </summary>
        public short MaxNum { get; set; }
        /// <summary>
        /// 描述内容
        /// </summary>
        public string ContentHtml { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string Manager { get; set; }
        /// <summary>
        /// 负责人手机号
        /// </summary>
        public string ManagerPhone { get; set; }
        /// <summary>
        /// 是否已发布
        /// </summary>
        public bool IsPublished { get; set; }
    }
}
