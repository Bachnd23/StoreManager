using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace COCOApp.Models.Validations
{
    public class ExportOrderItemMetaData
    {
        [Range(1, 100, ErrorMessage = "Số lượng phải lớn hơn không và nhỏ hơn hoặc bằng 100.")]
        public int Volume { get; set; }

        [Range(0, 100, ErrorMessage = "Số lượng thực phải là một số không âm và nhỏ hơn hoặc bằng 100.")]
        [RealVolumeValidation("Volume", ErrorMessage = "Số lượng thực phải nhỏ hơn hoặc bằng số lượng.")]
        public int RealVolume { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Giá sản phẩm phải là một giá trị dương.")]
        public decimal ProductPrice { get; set; }

    }
    // Custom validation attribute to ensure RealVolume <= Volume
    public class RealVolumeValidation : ValidationAttribute
    {
        private readonly string _volumePropertyName;

        public RealVolumeValidation(string volumePropertyName)
        {
            _volumePropertyName = volumePropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var realVolume = (int?)value;

            // Get the Volume property value
            var volumeProperty = validationContext.ObjectType.GetProperty(_volumePropertyName);
            if (volumeProperty == null)
                return new ValidationResult($"Unknown property {_volumePropertyName}");

            var volumeValue = (int?)volumeProperty.GetValue(validationContext.ObjectInstance);

            // Check if RealVolume is less than or equal to Volume
            if (realVolume.HasValue && volumeValue.HasValue && realVolume > volumeValue)
            {
                return new ValidationResult(ErrorMessage ?? $"RealVolume must be less than or equal to {volumeProperty.Name}");
            }

            return ValidationResult.Success;
        }
    }
}
