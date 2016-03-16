using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace ServiceApp.WebApi.Helpers
{
    public static class StringHelper
    {
        public static string ValidateModelState(this ModelStateDictionary ModelState)
        {
            string ResponseMessage = "";

            foreach (var state in ModelState)
            {
                if (!string.IsNullOrEmpty((state.Value.Errors[0]).ErrorMessage))
                    ResponseMessage += (state.Value.Errors[0]).ErrorMessage + "|";

                if (((state.Value.Errors[0]).Exception) != null)
                    ResponseMessage += ((state.Value.Errors[0]).Exception).Message + "|";
            }

            return ResponseMessage.Substring(0, ResponseMessage.Length - 1);
        }
    }
}