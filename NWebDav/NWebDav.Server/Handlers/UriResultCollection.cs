﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using NWebDav.Server.Helpers;

namespace NWebDav.Server.Handlers
{
    internal class UriResultCollection
    {
        private struct UriResult
        {
            private WebDavUri Uri { get; }
            private DavStatusCode Result { get; }

            public UriResult(WebDavUri uri, DavStatusCode result)
            {
                Uri = uri;
                Result = result;
            }

            public XElement GetXmlResponse()
            {
                var statusText = $"HTTP/1.1 {(int)Result} {DavStatusCodeHelper.GetStatusDescription(Result)}";
                return new XElement(WebDavNamespaces.DavNs + "response",
                    new XElement(WebDavNamespaces.DavNs + "href", Uri.AbsoluteUri),
                    new XElement(WebDavNamespaces.DavNs + "status", statusText));
            }
        }

        private readonly IList<UriResult> _results = new List<UriResult>();

        public bool HasItems => _results.Any();

        public void AddResult(WebDavUri uri, DavStatusCode result)
        {
            _results.Add(new UriResult(uri, result));
        }

        public XElement GetXmlMultiStatus()
        {
            var xMultiStatus = new XElement(WebDavNamespaces.DavNs + "multistatus");
            foreach (var result in _results)
                xMultiStatus.Add(result.GetXmlResponse());
            return xMultiStatus;
        }
    }
}
