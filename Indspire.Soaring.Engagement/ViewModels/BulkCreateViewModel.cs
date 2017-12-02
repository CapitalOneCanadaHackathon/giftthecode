using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.ViewModels
{
    public class BulkCreateViewModel
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int AmountCreated { get; set; }
        public int Amount { get; set; }
    }
}
