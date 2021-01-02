using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.SignalR.Client.Dtos
{
    public abstract class RequestParams
    {
        public int RequestId { get; private set; }

        public RequestParams()
        {
            RequestId = RequestIdGenerator.NextRequestId;
        }
    }
}
