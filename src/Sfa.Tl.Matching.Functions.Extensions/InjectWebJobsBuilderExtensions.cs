using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;

namespace Sfa.Tl.Matching.Functions.Extensions
{
    public static class InjectWebJobsBuilderExtensions
    {
        /// <summary>
        /// Adds the Injection extension to the provided <see cref="IWebJobsBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IWebJobsBuilder"/> to configure.</param>
        public static IWebJobsBuilder AddInject(
            this IWebJobsBuilder builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            //Register the extension
            builder.AddExtension<InjectConfiguration>();

            //Register the cleanup a filter
            builder.Services.AddSingleton<IFunctionFilter, ScopeCleanupFilter>();

            return builder;
        }
    }
}
