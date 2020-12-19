using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.SignalR.Client
{
    internal static class RequestIdGenerator
    {
        private static int _counter;
        private static object _locker;

        static RequestIdGenerator()
        {
            _locker = new object();
            _counter = 1;
        }

        private static void SetNextRequestId()
        {
            lock (_locker)
            {
                try
                {
                    _counter++;
                }
                catch (OverflowException)
                {
                    _counter = 1;
                }
            }
        }

        public static int NextRequestId
        {
            get
            {
                var result = _counter;
                SetNextRequestId();
                return result;
            }
        }
    }
}
