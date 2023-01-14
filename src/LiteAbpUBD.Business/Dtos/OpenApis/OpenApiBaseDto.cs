

using System.ComponentModel.DataAnnotations;

namespace LiteAbpUBD.Business.Dtos.OpenApis
{
    public class OpenApiBaseDto
    {
        /// <summary>
        /// 签名
        /// </summary>
        [Required]
        [StringLength(32)]
        public string Sign { get; set; }
        /// <summary>
        /// 时间戳（秒）
        /// </summary>
        [Required]
        public long Timestamp { get; set; }
    }

}
