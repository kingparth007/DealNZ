using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace DealsNZ.Helpers
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AttachmentAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
          ValidationContext validationContext)
        {   
            
        IEnumerable< HttpPostedFileBase> files= value as IEnumerable<HttpPostedFileBase>;
            foreach (var file in files)
            {
                // The file is required.
                if (file == null)
                {
                    return new ValidationResult("Please upload a file");
                }

                // The meximum allowed file size is 10MB.
                if (file.ContentLength > 1024 * 1024)
                {
                    return new ValidationResult("This file is should be less than 1MB");
                }

                // Only image file can be uploaded.
                var supportedTypes = new[] { "jpg", "jpeg", "pdf", "png" };
                string ext = Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(ext))
                {
                    return new ValidationResult("only Images files with extension .jpg, .png,.jpeg,.pdf");
                }
            }
            // Everything OK.
            return ValidationResult.Success;
        }
    }
}