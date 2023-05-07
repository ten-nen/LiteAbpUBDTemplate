

using System.ComponentModel.DataAnnotations;

namespace LiteAbpUBD.Example.Business.Dtos.OpenApi
{
    public class OpenCustomerBatchCreateDto : OpenApiBaseDto
    {
        /// <summary>
        /// 信息集合
        /// </summary>
        [Required, MinLength(1), MaxLength(1000)]
        public List<OpenCustomerCreateDto> Customers { get; set; }
    }

    /// <summary>
    /// 客户
    /// </summary>
    public class OpenCustomerCreateDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(32)]
        public string Name { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [Required]
        [StringLength(16)]
        public string Tel { get; set; }
    }
}
