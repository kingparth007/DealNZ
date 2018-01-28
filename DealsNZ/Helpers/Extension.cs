using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace DealsNZ.Helpers
{
    public static class Extension
    {
        public static bool IsValidImageList(this IEnumerable<HttpPostedFileBase> sequence, bool allownull)
        {
            bool valid;
            if (sequence == null ||sequence.Count() <= 0)
            {
                return allownull;
            }
            else
            {
                valid = true;
            }

            foreach (var file in sequence)
            {
                // The file is required.
                if (file == null)
                {
                    valid = false;
                    break;
                }

                // The meximum allowed file size is 10MB.
                if (file.ContentLength > 1024 * 1024)
                {

                    valid = false;
                    break;
                }

                string[] supporttype = new[] { "jpg", "jpeg", "pdf", "png" };
                string ext = Path.GetExtension(file.FileName).Substring(1);
                if ( !supporttype.Contains(ext))
                {
                    valid = false;
                    break;
                }
                else
                {

                    var fileName = Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName);
                    var path = Path.Combine(HttpContext.Current.Server.MapPath("/Images/DealsImages/"), fileName);
                    file.SaveAs(path);

                }
            }
            return valid;
        }
    
        public static String SaveImageFile(this HttpPostedFileBase sequence)

        {
            
                var fileName = Path.GetFileNameWithoutExtension(sequence.FileName) + Path.GetExtension(sequence.FileName);
                var filepath = "/Images/DealsImages/" + fileName;
                var path = Path.Combine(HttpContext.Current.Server.MapPath("/Images/DealsImages/"), fileName);
                sequence.SaveAs(path);
                return filepath;

            
        }
    }
}