﻿using System.Collections.Generic;
using System.Net;
using AppGet.Http;
using NLog;

namespace AppGet.PackageRepository
{
    public class AppGetServerClient : IPackageRepository
    {
        private readonly IHttpClient _httpClient;
        private readonly Logger _logger;
        private HttpRequestBuilder requestBuilder;

        private const string API_ROOT = "https://appget.net/api/v1/";

        public AppGetServerClient(IHttpClient httpClient, Logger logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            requestBuilder = new HttpRequestBuilder(API_ROOT);
        }


        public PackageInfo GetLatest(string name)
        {
            _logger.Info("Getting package " + name);

            var request = requestBuilder.Build("packages/{package}/latest");
            request.AddSegment("package", name);

            try
            {
                var package = _httpClient.Get<PackageInfo>(request);
                return package.Resource;
            }
            catch (HttpException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw;
            }
        }

        public List<PackageInfo> Search(string term)
        {
            _logger.Info("Searching for " + term);

            var request = requestBuilder.Build("packages");

            request.UriBuilder.SetQueryParam("q", term.Trim());

            var package = _httpClient.Get<List<PackageInfo>>(request);
            return package.Resource;
        }
    }
}