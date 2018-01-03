using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProjectoEsw.Validadores.Perfil
{
     public class DataNascValidation : ValidationAttribute, IClientModelValidator
    {

        public DataNascValidation() : base() {
            
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var datanasc = (DateTime)value;
            var diff = (System.DateTime.Now.Subtract(datanasc).TotalDays) / 365.0;
            return diff >= 17 ? new ValidationResult(base.ErrorMessage): ValidationResult.Success;

        }

        public override bool IsValid(object value)
        {
            var dateNasc = (DateTime)value;
            var diff = (System.DateTime.Now.Subtract(dateNasc).TotalDays) / 365.0;

            return diff >= 17;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val-_17ormore", errorMessage);

        }

        private bool MergeAttribute(
        IDictionary<string, string> attributes,
        string key,
        string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }
            attributes.Add(key, value);
            return true;
        }
    }
}
