using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains.Model.Entities
{
   public class CarriageModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public Carriage Carriage { get; set; }
    }
}
