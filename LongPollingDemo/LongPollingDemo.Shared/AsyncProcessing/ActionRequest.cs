using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongPolling.Shared
{
    public class ActionRequest : Request
    {

        Action action;

        public ActionRequest(Action action)
        {

            this.action = action;
        }
        protected override void OnExecute()
        {
            action();
        }
    }
}
