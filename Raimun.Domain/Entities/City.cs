using Raimun.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raimun.Domain.Entities
{
    public class City : IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Temp { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
