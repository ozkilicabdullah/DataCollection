using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwEpApi.Model.Response
{
    public class ValidationResultModel : ResponseModel
    {

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            Errors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => string.Concat(key, " ", x.ErrorMessage)))
                    .ToList();
        }
    }

}
