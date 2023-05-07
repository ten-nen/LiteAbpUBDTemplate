using System.ComponentModel.DataAnnotations;

namespace LiteAbpUBD.Example.Business.Dtos.OpenApi
{
    public class OpenApiBaseDto
    {
        /// <summary>
        /// 签名
        /// </summary>
        [Required, MinLength(1), MaxLength(32)]
        public string Sign { get; set; }
        /// <summary>
        /// 时间戳（秒）
        /// </summary>
        [Required]
        public long Timestamp { get; set; }
    }
}
