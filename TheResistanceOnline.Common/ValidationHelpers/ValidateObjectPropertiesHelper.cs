using System.ComponentModel.DataAnnotations;

namespace TheResistanceOnline.Common.ValidationHelpers;

public static class ValidateObjectPropertiesHelper
{
    #region Public Methods

    public static void ValidateAllObjectProperties(params object[] objectsToValidate)
    {
        var validationResults = new List<ValidationResult>();
        foreach(var objectToValidate in objectsToValidate)
        {
            var validationContext = new ValidationContext(objectToValidate, null, null);
            if (!Validator.TryValidateObject(objectToValidate, validationContext, validationResults,
                                             true))
            {
                throw new Exception(string.Join(",", validationResults.Select(v => v.ErrorMessage)));
            }
        }
    }

    #endregion
}
