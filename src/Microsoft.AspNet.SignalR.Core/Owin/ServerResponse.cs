﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hosting;
using Microsoft.AspNet.SignalR.Owin.Infrastructure;
using Microsoft.Owin;

namespace Microsoft.AspNet.SignalR.Owin
{
    public class ServerResponse : IResponse
    {
        private readonly CancellationToken _callCancelled;
        private readonly OwinResponse _response;

        public ServerResponse(IDictionary<string, object> environment)
        {
            _response = new OwinResponse(environment);
            _callCancelled = _response.Environment.Get<CancellationToken>(OwinConstants.CallCancelled);
        }

        public CancellationToken CancellationToken
        {
            get { return _callCancelled; }
        }

        public string ContentType
        {
            get { return _response.GetHeader("Content-Type"); }
            set { _response.SetHeader("Content-Type", value); }
        }

        public void Write(ArraySegment<byte> data)
        {
            _response.Body.Write(data.Array, data.Offset, data.Count);
        }

        public Task Flush()
        {
            return _response.Body.FlushAsync();
        }

        public Stream ResponseBody
        {
            get
            {
                return _response.Body;
            }
        }
    }
}