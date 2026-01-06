using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace com.fabioscagliola.Core.Presentation.Mvc
{
    /// <summary>
    /// Ensures that doubles including thousands separators are parsed correctly when the culture is not invariant 
    /// </summary>
    public class DoubleModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            double result = 0;

            if (bindingContext.ModelType == typeof(double))
            {
                ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                ModelState modelState = new ModelState();
                modelState.Value = new ValueProviderResult(valueProviderResult.RawValue, valueProviderResult.AttemptedValue, Thread.CurrentThread.CurrentUICulture);
                bindingContext.ModelState.Add(bindingContext.ModelName, modelState);

                string s = valueProviderResult.AttemptedValue;
                double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, Thread.CurrentThread.CurrentUICulture, out result);
            }

            return result;
        }

    }
}

