using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.ViewModels
{
    public class SetUserExternalIDJsonViewModel: JsonBaseViewModel
    {
        public SetUserExternalIDJsonViewModel()
        {
            ResponseData = new SetUserExternalIDResponseData();
        }

        public SetUserExternalIDResponseData ResponseData = new SetUserExternalIDResponseData();
    }
}
