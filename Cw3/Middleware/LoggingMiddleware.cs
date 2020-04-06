using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cw3.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next) { _next = next; }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            string logg = httpContext.Request.Method + " ";
            logg += httpContext.Request.Path + " ";
            var bodyStream = string.Empty;
            using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyStream = await reader.ReadToEndAsync();
            }
            logg += bodyStream + " ";
            logg += httpContext.Request.QueryString;
            var path = Directory.GetCurrentDirectory();
            path += "\\Loggs\\loggs.txt";

            using(StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(logg);
                sw.Close();
            }
            

            await _next(httpContext); }
        }
    }
