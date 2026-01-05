using System;

namespace com.fabioscagliola.Core.DataAccess.Attributes
{
    /// <summary>
    /// Marks the property as editable thus allowing to cancel the modifications as per the <see cref="System.ComponentModel.IEditableObject"/> interface 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EditablePropertyAttribute : Attribute
    {
        public EditablePropertyAttribute() { }

    }
}

