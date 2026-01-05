using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace com.fabioscagliola.Core.DataAccess
{
    /// <summary>
    /// Holds information about each validation error returned by the <see cref="ValidateableObjectHelper.GetValidationErrors"/> method 
    /// </summary>
    public class ValidationError
    {
        protected PropertyInfo propertyInfo;

        protected string errorMessage;

        /// <summary>
        /// Initializes a new instance of the class 
        /// </summary>
        /// <param name="propertyInfo">The property whose value is invalid</param>
        /// <param name="errorMessage">The message of the validation error</param>
        public ValidationError(PropertyInfo propertyInfo, string errorMessage)
        {
            this.propertyInfo = propertyInfo;
            this.errorMessage = errorMessage;
        }

        /// <summary>
        /// The property whose value is invalid 
        /// </summary>
        public PropertyInfo PropertyInfo { get => propertyInfo; }

        /// <summary>
        /// The message of the validation error 
        /// </summary>
        public string ErrorMessage { get => errorMessage; }

    }

    /// <summary>
    /// This class is meant to help implementing the <see cref="IValidateableObject"/> interface; 
    /// it also exposes static methods allowing to validate objects implementing the <see cref="IValidateableObject"/> interface 
    /// </summary>
    public class ValidateableObjectHelper
    {
        /// <summary>
        /// The list of validation errors 
        /// </summary>
        /// <remarks>
        /// Derived classes implementing the <see cref="IDataErrorInfo"/> interface are supposed to add a unique string to this list 
        /// for each property whose value is invalid, and remove the string from this list when the value of the property is valid 
        /// (typically the unique string is the name of the property whose value is invalid); 
        /// since this list is private, strings are to be added using the <see cref="AddError"/> method, 
        /// and removed using the <see cref="RemError"/> method 
        /// </remarks>
        private List<string> errors;

        public ValidateableObjectHelper()
        {
            errors = new List<string>();
        }

        /// <summary>
        /// Adds a validation error 
        /// </summary>
        /// <param name="error">A unique string identifying the property whose value is invalid (typically the name of the property)</param>
        public void AddError(string error)
        {
            if (!errors.Contains(error))
            {
                errors.Add(error);
            }
        }

        /// <summary>
        /// Removes a validation error 
        /// </summary>
        /// <param name="error">A unique string identifying the property whose value is valid (typically the name of the property)</param>
        public void RemError(string error)
        {
            errors.Remove(error);
        }

        /// <summary>
        /// A Boolean value indicating if the value of at least one property is invalid 
        /// </summary>
        public bool HasError
        {
            get
            {
                return errors.Count != 0;
            }
        }

        /// <summary>
        /// This static method returns the list of the validation errors (if any) of an object implementing the <see cref="IDataErrorInfo"/> interface 
        /// </summary>
        /// <param name="ob">The object implementing the <see cref="IDataErrorInfo"/> interface</param>
        public static List<ValidationError> GetValidationErrors(IDataErrorInfo ob)
        {
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (ob != null)
            {
                PropertyInfo[] propertyInfoList = (ob.GetType()).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToArray();

                foreach (PropertyInfo propertyInfo in propertyInfoList)
                {
                    // Skip properties where the BrowsableAttribute is false 
                    BrowsableAttribute browsableAttribute = (BrowsableAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(BrowsableAttribute));
                    if (browsableAttribute != null && browsableAttribute.Browsable == false) continue;

                    // If the type of the property is IDataErrorInfo, then check if it is has an error 
                    if (propertyInfo.PropertyType.GetInterface(typeof(IDataErrorInfo).Name) != null)
                    {
                        IDataErrorInfo dataErrorInfo = (IDataErrorInfo)propertyInfo.GetValue(ob);

                        _ = HasValidationErrors(dataErrorInfo);
                    }

                    // If the type of the property is IEnumerable, and the property is not an indexer, then check if any of its items has an error 
                    if (propertyInfo.PropertyType.GetInterface(typeof(IEnumerable).Name) != null && propertyInfo.GetIndexParameters().Length == 0)
                    {
                        IEnumerable enumerable = (IEnumerable)propertyInfo.GetValue(ob);

                        if (enumerable != null)
                            foreach (object obj in enumerable)
                                if (obj is IDataErrorInfo dataErrorInfo && HasValidationErrors(dataErrorInfo))
                                    break;
                    }

                    string validationError = ob[propertyInfo.Name];

                    if (validationError != null)
                    {
                        validationErrorList.Add(new ValidationError(propertyInfo, validationError));
                    }
                }
            }

            return validationErrorList;
        }

        /// <summary>
        /// This static method returns the list of the messages of the validation errors (if any) of an object implementing the <see cref="IDataErrorInfo"/> interface 
        /// </summary>
        /// <param name="ob">The object implementing the <see cref="IDataErrorInfo"/> interface</param>
        public static List<string> GetErrorMessages(IDataErrorInfo ob)
        {
            return GetValidationErrors(ob).Select(x => x.ErrorMessage).ToList();
        }

        /// <summary>
        /// This static method returns the list of the names of the properties whose value is invalid (if any) of an object implementing the <see cref="IDataErrorInfo"/> interface 
        /// </summary>
        /// <param name="ob">The object implementing the <see cref="IDataErrorInfo"/> interface</param>
        public static List<string> GetInvalidProperties(IDataErrorInfo ob)
        {
            return GetValidationErrors(ob).Select(x => x.PropertyInfo.Name).ToList();
        }

        /// <summary>
        /// This static method returns a Boolean value indicating if an object implementing the <see cref="IDataErrorInfo"/> interface has validation errors 
        /// </summary>
        /// <param name="ob">The object implementing the <see cref="IDataErrorInfo"/> interface</param>
        public static bool HasValidationErrors(IDataErrorInfo ob)
        {
            return GetValidationErrors(ob).Count != 0;
        }

    }
}

