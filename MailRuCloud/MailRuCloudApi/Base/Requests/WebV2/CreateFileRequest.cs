﻿using System;
using System.Text;
using YaR.MailRuCloud.Api.Base.Auth;
using YaR.MailRuCloud.Api.Base.Requests.Types;

namespace YaR.MailRuCloud.Api.Base.Requests.WebV2
{
   class CreateFileRequest : BaseRequestJson<CommonOperationResult<string>>
    {
        private readonly string _fullPath;
        private readonly string _hash;
        private readonly long _size;
        private readonly ConflictResolver _conflictResolver;

        public CreateFileRequest(HttpCommonSettings settings, IAuth auth, string fullPath, string hash, long size, ConflictResolver? conflictResolver) 
            : base(settings, auth)
        {
            _fullPath = fullPath;
            _hash = hash;
            _size = size;
            _conflictResolver = conflictResolver ?? ConflictResolver.Rename;
        }

        protected override string RelationalUri => "/api/v2/file/add";

        protected override byte[] CreateHttpContent()
        {
            string filePart = $"&hash={_hash}&size={_size}";
            string data = $"home={Uri.EscapeDataString(_fullPath)}&conflict={_conflictResolver}&api=2&token={Auth.AccessToken}" + filePart;

            return Encoding.UTF8.GetBytes(data);
        }
    }
}
