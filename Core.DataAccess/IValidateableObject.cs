namespace com.fabioscagliola.Core.DataAccess
{
    /// <summary>
    /// Exposes a Boolean value indicating if the implementing class has validation errors; 
    /// use the <see cref="ValidateableObjectHelper"/> class to implement it 
    /// </summary>
    public interface IValidateableObject
    {
        bool HasError { get; }

    }
}

