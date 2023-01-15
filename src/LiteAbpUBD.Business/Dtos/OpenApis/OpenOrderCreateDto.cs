

using System.ComponentModel.DataAnnotations;

namespace LiteAbpUBD.Business.Dtos.OpenApis
{
    public class OpenOrderCreateDto
    {
        public string OrderId { get; set; }

        [Required, MinLength(1), MaxLength(32)]
        public List<string> OrderDetails { get; set; }
    }
}
