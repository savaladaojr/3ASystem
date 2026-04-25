using Application.Abstractions.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace _3ASystem.Application
{
	public static class DependenciesResolver
	{
		public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
		{
			//adding mediatr
			services.AddMediatR(config => {
				config.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzk0MzU1MjAwIiwiaWF0IjoiMTc2MjkwMjAxOCIsImFjY291bnRfaWQiOiIwMTlhNzUyNTA2NWI3NWEyYWU1NDBkMjRmNTU5ODJkZSIsImN1c3RvbWVyX2lkIjoiY3RtXzAxazl0amMyM3B4ZHc2Y2p6cm04aHRkczkwIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.vvxyHlkCDcs2Cy3xwbMjVtDf086UsZ7PYET6oTlgtduMw3EhdZC-OwPvTYYcP4O55A_EwLk6llxm_fV0vQ3FXEyqL_LV-HIbPpb0xnVubroJxyKTPFnkXLrcFLYfc2iuhoW6zVRNUbvaHWXuby0FH1LLXu_9m6s5PvLaKymVxF9H-2QsGJnIhMVfAyvfJufSIEJX6NjoPlyI6--Whgf8H_lyKVRtvYf9cDwp5TomxAZL8KgiiIURtYLp57i9kMrs9G1xzMfmP5JuOde2_A3_ahHlbEb9dIXUIPKp_rDrOjCcGsq3_HMslivWfLMypyznCfLyUTLhWRGjZvJQZx1rTQ";

				config.RegisterServicesFromAssemblies(typeof(DependenciesResolver).Assembly);


				config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
				config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
			});

			//adding fluent validation
			services.AddValidatorsFromAssembly(typeof(DependenciesResolver).Assembly, includeInternalTypes: true);

			return services;
		}
	}
}
