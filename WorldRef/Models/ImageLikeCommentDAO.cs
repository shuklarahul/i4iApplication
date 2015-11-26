using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.Models
{
    public class ImageLikeCommentDAO
    {


        
            public int PImageLikeId { get; set; }
            public Nullable<int> ImageId { get; set; }
            public Nullable<int> UserId { get; set; }
            public Nullable<int> ProjectId { get; set; }
            public Nullable<bool> ImgLikeFlag { get; set; }
            public string ImageTestimonials { get; set; }
            public Nullable<bool> CmtImgFlag { get; set; }
            public string ProfileAttach { get; set; }
        
    }
}