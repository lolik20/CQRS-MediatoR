using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;



namespace CQRS_MediatoR.Api.Validation
{
    public static class FluentValidationInitializer
    {
        public static IMvcBuilder RegisterFluentValidation(this IMvcBuilder service)
        {
            return service.AddFluentValidation();
        }

    }
}
