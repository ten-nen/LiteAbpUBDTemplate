using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteAbpUBD.Business.Dtos
{
    public class MenuDto
    {
        public int Id { get; set; }
        public int Pid { get; set; }
        public string TreeId { get; set; }         
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Route { get; set; }
        public int Order { get; set; }   
    }
}
