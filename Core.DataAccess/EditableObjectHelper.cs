using com.fabioscagliola.Core.DataAccess.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace com.fabioscagliola.Core.DataAccess
{
    /// <summary>
    /// This class is meant to help implementing the <see cref="System.ComponentModel.IEditableObject"/> interface 
    /// by allowing to cancel modifications to properties marked with the <see cref="EditablePropertyAttribute"/> attribute 
    /// </summary>
    public class EditableObjectHelper
    {
        protected object Parent;

        /// <summary>
        /// The dictionary containing the orignal values of the properties 
        /// </summary>
        protected Dictionary<string, object> properties = null;

        public EditableObjectHelper(object parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Saves the original values of the properties marked with the <see cref="EditablePropertyAttribute"/> to the dictionary 
        /// </summary>
        public void BeginEdit()
        {
            PropertyInfo[] propertyInfoList = Parent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList()
            .FindAll(x => x.GetCustomAttributes(typeof(EditablePropertyAttribute), true).Count() != 0).ToArray();

            properties = new Dictionary<string, object>();

            foreach (PropertyInfo propertyInfo in propertyInfoList)
            {
                //if (propertyInfo.GetSetMethod() != null)
                //{
                    object value = propertyInfo.GetValue(Parent, null).Copy();
                    properties.Add(propertyInfo.Name, value);
                //}
            }
        }

        /// <summary>
        /// Restores the original values of the properties marked with the <see cref="EditablePropertyAttribute"/> from the dictionary 
        /// </summary>
        public void CancelEdit()
        {
            if (this.properties != null)
            {
                PropertyInfo[] propertyInfoList = (Parent.GetType()).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList()
                    .FindAll(x => x.GetCustomAttributes(typeof(EditablePropertyAttribute), true).Count() != 0).ToArray();

                foreach (PropertyInfo propertyInfo in propertyInfoList)
                {
                    if (propertyInfo.GetSetMethod() != null)
                    {
                        object value = properties[propertyInfo.Name];
                        propertyInfo.SetValue(Parent, value, null);
                    }
                }

                properties = null;
            }
        }

        /// <summary>
        /// Discards the orioginal values of the properties 
        /// </summary>
        public void EndEdit()
        {
            properties = null;
        }

        public bool IsChanged()
        {
            if (this.properties != null)
            {
                PropertyInfo[] propertyInfoList = (Parent.GetType()).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList()
                    .FindAll(x => x.GetCustomAttributes(typeof(EditablePropertyAttribute), true).Count() != 0).ToArray();

                foreach (PropertyInfo propertyInfo in propertyInfoList)
                {
                    object obj1 = propertyInfo.GetValue(Parent, null);
                    object obj2 = properties[propertyInfo.Name];
                    if(Equals(obj1, obj2) == false)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

