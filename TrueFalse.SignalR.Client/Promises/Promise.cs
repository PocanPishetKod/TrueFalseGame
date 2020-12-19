using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.SignalR.Client.Dtos;

namespace TrueFalse.SignalR.Client.Promises
{
    public class Promise<TCallbackParameters> where TCallbackParameters : ResponseParams
    {
        private Action<TCallbackParameters> _successCallback;
        private Action<Exception> _errorCallback;
        private Action _finallyCallback;
        private Action _operation;
        private bool _invoked;
        private bool _finallyInvoked;
        private object _invokedLocker;
        private object _finallyLocker;
        private readonly int _requestId;

        public Promise(int requestId, Action operation)
        {
            _invoked = false;
            _finallyInvoked = false;
            _invokedLocker = new object();
            _finallyLocker = new object();
            _requestId = requestId;
            _operation = operation;
        }

        internal void OnCopleted(TCallbackParameters callbackParameters)
        {
            if (callbackParameters.RequestId == _requestId)
            {
                InvokeThenCallback(callbackParameters);
                InvokeFinallyCallback();
            }
        }

        internal void InvokeThenCallback(TCallbackParameters callbackParameters)
        {
            if (_successCallback == null)
            {
                throw new NullReferenceException(nameof(_successCallback));
            }

            if (!_invoked)
            {
                lock (_invokedLocker)
                {
                    if (!_invoked)
                    {
                        _successCallback.Invoke(callbackParameters);
                        _invoked = true;
                    }
                }
            }
        }

        internal void InvokeCatchCallback(Exception exception)
        {
            if (_errorCallback == null)
            {
                throw new NullReferenceException(nameof(_errorCallback));
            }

            if (!_invoked)
            {
                lock (_invokedLocker)
                {
                    if (!_invoked)
                    {
                        _errorCallback.Invoke(exception);
                        _invoked = true;
                    }
                }
            }
        }

        internal void InvokeFinallyCallback()
        {
            if (_finallyCallback != null)
            {
                if (!_finallyInvoked)
                {
                    lock (_finallyLocker)
                    {
                        if (!_finallyInvoked)
                        {
                            _finallyCallback.Invoke();
                            _finallyInvoked = true;
                        }
                    }
                }
            }
        }

        public Promise<TCallbackParameters> Then(Action<TCallbackParameters> successCallback)
        {
            if (successCallback == null)
            {
                throw new ArgumentNullException(nameof(successCallback));
            }

            _successCallback = successCallback;
            _operation.Invoke();
            return this;
        }

        public Promise<TCallbackParameters> Catch(Action<Exception> errorCallback)
        {
            if (errorCallback == null)
            {
                throw new ArgumentNullException(nameof(errorCallback));
            }

            _errorCallback = errorCallback;
            return this;
        }

        public Promise<TCallbackParameters> Finally(Action finallyCallback)
        {
            if (finallyCallback == null)
            {
                throw new ArgumentNullException(nameof(finallyCallback));
            }

            _finallyCallback = finallyCallback;
            return this;
        }
    }
}
