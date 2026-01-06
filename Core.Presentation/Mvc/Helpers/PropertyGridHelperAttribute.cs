using System;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers
{
    /// <summary>
    /// The type of editor used to render the property in the property-grid editor 
    /// </summary>
    public enum PropertyGridHelperEditorType
    {
        /// <summary>
        /// The editor will be automatically chosen based on the type of the property 
        /// </summary>
        Auto,
        /// <summary>
        /// The editor will be rendered as a hidden field, a read-only text-box, and a button; 
        /// the button is expected to trigger a lookup mechanism to be implemented in JavaScript; 
        /// the goal of the lookup mechanism is to set the value of both the hidden field and the read-only text-box 
        /// </summary>
        CustomLookup,
        /// <summary>
        /// If the type of the property is <see cref="DateTime"/> or <see cref="Nullable&lt;DateTime&gt;"/>, 
        /// then the property will be rendered as a date-only picker instead of a date and time picker; 
        /// if the type of the property is other than <see cref="DateTime"/> or <see cref="Nullable&lt;DateTime&gt;"/>, 
        /// then the editor will be automatically chosen based on the type of the property anyway 
        /// </summary>
        DatePicker,
        /// <summary>
        /// If the type of the property is <see cref="Guid"/> or <see cref="Nullable&lt;Guid&gt;"/>, 
        /// then the property will be rendered as an illustration; 
        /// if the type of the property is other than <see cref="Guid"/> or <see cref="Nullable&lt;Guid&gt;"/>, 
        /// then the editor will be automatically chosen based on the type of the property anyway 
        /// </summary>
        Illustration,
        /// <summary>
        /// If the type of the property is <see cref="string"/>, 
        /// then the property will be rendered as a password-box instead of a text-box; 
        /// if the type of the property is other than <see cref="string"/>, 
        /// then the editor will be automatically chosen based on the type of the property anyway 
        /// </summary>
        Password,
        /// <summary>
        /// The editor will be rendered as a DevExtreme slider and a hidden field, whose value is automatically set by the slider 
        /// </summary>
        Percentage,
        /// <summary>
        /// If the type of the property is <see cref="string"/>, 
        /// then the property will be rendered as a text-area instead of a text-box; 
        /// if the type of the property is other than <see cref="string"/>, 
        /// then the editor will be automatically chosen based on the type of the property anyway 
        /// </summary>
        TextArea,
        /// <summary>
        /// If the type of the property is <see cref="DateTime"/> or <see cref="Nullable&lt;DateTime&gt;"/>, 
        /// then the property will be rendered as a time-only picker instead of a date and time picker; 
        /// if the type of the property is other than <see cref="DateTime"/> or <see cref="Nullable&lt;DateTime&gt;"/>, 
        /// then the editor will be automatically chosen based on the type of the property anyway 
        /// </summary>
        TimePicker,
    }

    /// <summary>
    /// Provides additional information about how to render the property in the property-grid editor and viewer 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PropertyGridHelperAttribute : Attribute
    {
        protected string displayProperty;
        protected PropertyGridHelperEditorType editorType;
        protected string format;
        Type parentType;
        protected int sequenceNumber;

        /// <summary>
        /// Initializes a new instance of the class 
        /// </summary>
        /// <param name="displayProperty">The name of the property whose value is to be displayed when <see cref="EditorType"/> is <see cref="PropertyGridHelperEditorType.CustomLookup"/></param>
        /// <param name="editorType">The type of editor used to render the property in the property-grid editor</param>
        /// <param name="format">The format to be applied to the value of the property in the property-grid editor</param>
        /// <param name="parentType">Allows to specify the parent entity of foreign-key properties</param>
        /// <param name="sequenceNumber">The position of the property in the property-grid editor and viewer</param>
        public PropertyGridHelperAttribute(string displayProperty = null, PropertyGridHelperEditorType editorType = PropertyGridHelperEditorType.Auto, string format = null, Type parentType = null, int sequenceNumber = 0)
        {
            this.displayProperty = displayProperty;
            this.editorType = editorType;
            this.format = format;
            this.parentType = parentType;
            this.sequenceNumber = sequenceNumber;
        }

        /// <summary>
        /// The name of the property whose value is to be displayed when <see cref="EditorType"/> is <see cref="PropertyGridHelperEditorType.CustomLookup"/> 
        /// </summary>
        public string DisplayProperty
        {
            get
            {
                return displayProperty;
            }
        }

        /// <summary>
        /// The type of editor used to render the property in the property-grid editor 
        /// </summary>
        public PropertyGridHelperEditorType EditorType
        {
            get
            {
                return editorType;
            }
        }

        /// <summary>
        /// The format to be applied to the value of the property in the property-grid editor 
        /// </summary>
        public string Format
        {
            get
            {
                return format;
            }
        }

        /// <summary>
        /// Allows to specify the parent entity of foreign-key properties 
        /// </summary>
        public Type ParentType
        {
            get
            {
                return parentType;
            }
        }

        /// <summary>
        /// The position of the property in the property-grid editor and viewer 
        /// </summary>
        public int SequenceNumber
        {
            get
            {
                return sequenceNumber;
            }
        }

    }
}

