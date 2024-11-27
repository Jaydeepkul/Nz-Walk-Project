using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NZWalks.ValidateModelAttribute_CustomValidation_
{
    public class ValidateModelAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid==false) 
            {
                context.Result = new BadRequestResult();
            }
            // Using ValidateModelAttribute this method we can validate the mothod
        }
    }
}
