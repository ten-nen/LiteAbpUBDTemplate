using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;

namespace LiteAbpUBD.Example.DataAccess.Entities
{
    public class Customer : FullAuditedAggregateRoot<Guid>
    {
        public Customer() : base()
        {

        }
        public Customer(Guid id) : base(id)
        {

        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

    }
}
