using System;
using System.Data;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NHibernate;
using NoteBookApp.Shared;
using NoteBookApp.Shared.Exceptions;
using ObjectNotFoundException = NoteBookApp.Shared.Exceptions.ObjectNotFoundException;

namespace NoteBookApp.Server.Infrastructure
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), subApp =>
            {
                subApp.UseExceptionHandler(appError =>
                {
                    appError.Run(async context =>
                    {
                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                        var exception = contextFeature.Error;

                        var errorDetails = new ErrorDetails()
                        {
                            Message = exception.Message
                        };
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        switch (exception)
                        {
                            case ObjectNotFoundException:
                                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                errorDetails.ServiceError = ServiceError.ObjectNotFoundException;
                                break;
                            case UniqueException:
                                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                errorDetails.ServiceError = ServiceError.UniqueException;
                                break;
                            case StaleObjectStateException:
                                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                errorDetails.ServiceError = ServiceError.StaleObjectStateException;
                                break;
                            case UnauthorizedAccessException:
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                errorDetails.ServiceError = ServiceError.UnauthorizedAccessException;
                                break;
                            case ArgumentNullException:
                                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                errorDetails.ServiceError = ServiceError.ArgumentNullException;
                                break;
                            case InvalidOperationException:
                                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                errorDetails.ServiceError = ServiceError.InvalidOperationException;
                                break;
                            case ArgumentOutOfRangeException:
                                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                errorDetails.ServiceError = ServiceError.ArgumentOutOfRangeException;
                                break;
                            case ConstraintException:
                                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                                errorDetails.ServiceError = ServiceError.ConstraintException;
                                break;
                        }



                        context.Response.ContentType = "application/json";
                        var json = JsonConvert.SerializeObject(errorDetails);
                        await context.Response.WriteAsync(json).ConfigureAwait(false);
                    });
                });
            });

        }
    }
}