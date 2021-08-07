using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollection.Entities.Base
{
    public class PaymentType
    {
        public PaymentType()
        {
            Instalment = 1;
        }
        public string Name { get; set; }
        public int Instalment { get; set; }

    }
}
