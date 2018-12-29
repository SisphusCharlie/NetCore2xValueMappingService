using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ValueMappingCoreAPI.Models.MapViewModels;

namespace ValueMappingCoreAPI.Custome.CustomeValidator
{
    public class CustomeValidator : ValidationAttribute, IClientModelValidator
    {
        private string valuationFN;
        public CustomeValidator(string vf)
        {
            valuationFN = vf;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValueMapsModel vmm = (ValueMapsModel)validationContext.ObjectInstance;

            if (!IsNumAndEnCh(vmm.ValuationFunction))
            {
                return new ValidationResult("VF must only have number and Char");
            }
            return ValidationResult.Success;
        }

        protected bool isNumberic(string message, out int result)
        {
            System.Text.RegularExpressions.Regex rex =
            new System.Text.RegularExpressions.Regex(@"^\d+$");
            result = -1;
            if (rex.IsMatch(message))
            {
                result = int.Parse(message);
                return true;
            }
            else
                return false;
        }
        public static bool IsNumAndEnCh(string input)
        {
            string pattern = @"^[A-Za-z0-9]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
