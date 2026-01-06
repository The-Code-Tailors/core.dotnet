using com.fabioscagliola.Core.DataAccess.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers
{
    /// <summary>
    /// Represents a property of an object edited using a property-grid editor or viewed using a property-grid viewer 
    /// </summary>
    public class PropertyGridHelperProperty
    {
        /// <summary>
        /// The category of the property 
        /// </summary>
        public string Category { get; protected set; }
        /// <summary>
        /// The description of the property 
        /// </summary>
        public string Description { get; protected set; }
        /// <summary>
        /// The <see cref="System.Reflection.PropertyInfo"/> object underlying the property whose value is to be displayed when <see cref="EditorType"/> is <see cref="PropertyGridHelperEditorType.CustomLookup"/> 
        /// </summary>
        public PropertyInfo DisplayPropertyInfo { get; protected set; }
        /// <summary>
        /// The value to be displayed when <see cref="EditorType"/> is <see cref="PropertyGridHelperEditorType.CustomLookup"/> 
        /// </summary>
        public object DisplayValue { get; protected set; }
        /// <summary>
        /// The type of editor used to render the property in the property-grid editor 
        /// </summary>
        public PropertyGridHelperEditorType EditorType { get; protected set; }
        /// <summary>
        /// The format to be applied to the value of the property in the property-grid editor 
        /// </summary>
        public string Format { get; protected set; }
        /// <summary>
        /// The identifier of the property on the client side 
        /// </summary>
        public string Identifier { get; protected set; }
        /// <summary>
        /// A Boolean value indicating if the property is read-only 
        /// </summary>
        public bool IsReadOnly { get; protected set; }
        /// <summary>
        /// The label of the property 
        /// </summary>
        public string Label { get; protected set; }
        /// <summary>
        /// The name of the property 
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// Allows to specify the parent entity of foreign-key properties 
        /// </summary>
        public Type ParentType { get; protected set; }
        /// <summary>
        /// The underlying <see cref="System.Reflection.PropertyInfo"/> object 
        /// </summary>
        public PropertyInfo PropertyInfo { get; protected set; }
        /// <summary>
        /// The position of the property in the property-grid editor and viewer 
        /// </summary>
        public int SequenceNumber { get; protected set; }
        /// <summary>
        /// The type of the property 
        /// </summary>
        public Type Type { get; protected set; }
        /// <summary>
        /// The value of the property 
        /// </summary>
        public object Value { get; protected set; }

        /// <summary>
        /// The list of the property's properties – if the type of the property is not primitive 
        /// </summary>
        public List<PropertyGridHelperProperty> PropertyList { get; set; }

        /// <summary>
        /// Returns the <see cref="System.ComponentModel.EnumConverter"/> of properties whose type is an enum; 
        /// throws an exception if the type of the property in not an enum, 
        /// or if the enum lacks the <see cref="System.ComponentModel.TypeConverterAttribute"/> attribute 
        /// </summary>
        public EnumConverter EnumConverter
        {
            get
            {
                if (!Type.IsEnum)
                {
                    throw new PresentationException($"The type of the property in not an enum!");
                }

                TypeConverterAttribute typeConverterAttribute = (TypeConverterAttribute)TypeDescriptor.GetAttributes(Value)[typeof(TypeConverterAttribute)];

                if (string.IsNullOrWhiteSpace(typeConverterAttribute.ConverterTypeName))
                {
                    // try to find the TypeConverter attribute on the property itself
                    try
                    {
                        typeConverterAttribute = (TypeConverterAttribute)PropertyInfo.GetCustomAttribute(typeof(TypeConverterAttribute));
                    }
                    catch
                    { }

                    if (string.IsNullOrWhiteSpace(typeConverterAttribute?.ConverterTypeName))
                    {
                        throw new PresentationException($"Most likely the \"{Type}\" enum or the \"{PropertyInfo.Name}\" property lacks the {nameof(TypeConverterAttribute)} attribute!");
                    }
                }

                Type type = Type.GetType(typeConverterAttribute.ConverterTypeName);
                EnumConverter enumConverter = (EnumConverter)Activator.CreateInstance(type, Type);

                return enumConverter;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FormattedValue
        {
            get
            {
                string result = null;

                if (Value != null)
                {
                    if (Format == null)
                    {
                        result = Value.ToString();
                    }
                    else
                    {
                        result = string.Format(Format, Value);
                    }
                }

                return result;
            }
        }

        protected PropertyGridHelperProperty()
        {
            PropertyList = new List<PropertyGridHelperProperty>();
        }

        /// <summary>
        /// Returns the list of the properties of the specified object 
        /// </summary>
        /// <param name="ob">The object edited using a property-grid editor or viewed using a property-grid viewer</param>
        public static List<PropertyGridHelperProperty> DoPropertyList(object ob)
        {
            return DoPropertyList(ob, null);
        }

        protected static List<PropertyGridHelperProperty> DoPropertyList(object ob, string parentIdentifier)
        {
            List<PropertyGridHelperProperty> propertyList = new List<PropertyGridHelperProperty>();

            if (ob != null)
            {
                PropertyInfo[] propertyInfoList = ob.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

                foreach (PropertyInfo propertyInfo in propertyInfoList)
                {
                    BrowsableAttribute browsableAttribute = (BrowsableAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(BrowsableAttribute));

                    if (browsableAttribute == null || (browsableAttribute != null && browsableAttribute.Browsable))
                    {
                        PropertyGridHelperProperty property = new PropertyGridHelperProperty();
                        propertyList.Add(property);

                        // Category 
                        CustomCategoryAttribute customCategoryAttribute = (CustomCategoryAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(CustomCategoryAttribute));
                        if (customCategoryAttribute != null)
                        {
                            property.Category = customCategoryAttribute.Category;
                        }

                        // Description 
                        CustomDescriptionAttribute customDescriptionAttribute = (CustomDescriptionAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(CustomDescriptionAttribute));
                        if (customDescriptionAttribute != null)
                        {
                            property.Description = customDescriptionAttribute.Description;
                        }

                        // Identifier 
                        property.Identifier += $"{parentIdentifier}__{propertyInfo.Name}";

                        // IsReadOnly 
                        ReadOnlyAttribute readOnlyAttribute = (ReadOnlyAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(ReadOnlyAttribute));
                        if (readOnlyAttribute != null)
                        {
                            property.IsReadOnly = true;
                        }
                        else
                        {
                            property.IsReadOnly = !propertyInfo.CanWrite;
                        }

                        // Label 
                        CustomDisplayNameAttribute customDisplayNameAttribute = (CustomDisplayNameAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(CustomDisplayNameAttribute));
                        if (customDisplayNameAttribute != null)
                        {
                            property.Label = customDisplayNameAttribute.DisplayName;
                        }
                        else
                        {
                            property.Label = propertyInfo.Name;
                        }

                        // Name 
                        property.Name = propertyInfo.Name;

                        // PropertyInfo 
                        property.PropertyInfo = propertyInfo;

                        // Type 
                        property.Type = propertyInfo.PropertyType;

                        // Value 
                        property.Value = propertyInfo.GetValue(ob);

                        // PropertyList 
                        if (property.Type.Module.Name != "mscorlib.dll")
                        {
                            property.PropertyList = DoPropertyList(property.Value, property.Identifier);
                        }


                        // DisplayPropertyInfo, DisplayValue, EditorType, Format, ParentType, SequenceNumber 
                        PropertyGridHelperAttribute propertyGridHelperAttribute = (PropertyGridHelperAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(PropertyGridHelperAttribute));
                        if (propertyGridHelperAttribute != null)
                        {
                            if (!string.IsNullOrWhiteSpace(propertyGridHelperAttribute.DisplayProperty))
                            {
                                PropertyInfo displayPropertyInfo = ob.GetType().GetProperty(propertyGridHelperAttribute.DisplayProperty);

                                if (displayPropertyInfo != null)
                                {
                                    property.DisplayPropertyInfo = displayPropertyInfo;
                                    property.DisplayValue = displayPropertyInfo.GetValue(ob);
                                }
                            }

                            property.EditorType = propertyGridHelperAttribute.EditorType;
                            property.Format = propertyGridHelperAttribute.Format;
                            property.ParentType = propertyGridHelperAttribute.ParentType;
                            property.SequenceNumber = propertyGridHelperAttribute.SequenceNumber;
                        }
                    }
                }
            }

            propertyList.Sort((a, b) =>
            {
                int result = a.SequenceNumber.CompareTo(b.SequenceNumber);

                if (result == 0)
                {
                    if (a.Label != null)
                    {
                        result = DoVersionComparison(a.Label, b.Label);

                        if (result == 0)
                        {
                            result = a.Label.CompareTo(b.Label);
                        }
                    }
                }

                return result;
            });

            return propertyList;
        }

        public override string ToString()
        {
            return Name;
        }

        public static int DoVersionComparison(string a, string b)
        {
            int result = 0;

            if (a != null && b != null)
            {
                if (Version.TryParse(a.Split(' ')[0], out Version aVersion))
                {
                    if (Version.TryParse(b.Split(' ')[0], out Version bVersion))
                    {
                        result = aVersion.CompareTo(bVersion);
                    }
                }
            }

            return result;
        }

    }
}

