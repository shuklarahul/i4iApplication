using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.Models
{
    public class ExternalClassModel
    {

        public int TotalLikeCount { get; set; }
        public string imgPath { get; set; }

        //properties for images.
        public Nullable<int> ImageId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public Nullable<bool> ImgLikeFlag { get; set; }
        public string ImageTestimonials { get; set; }
        public Nullable<bool> CmtImgFlag { get; set; }
        public string ProfileAttach { get; set; }

        //properties for videos.
        public Nullable<int> VideosId { get; set; }
        public Nullable<bool> VideosLikeFlag { get; set; }
        public string VideosTestimonials { get; set; }
        public Nullable<bool> cmtVidFlag { get; set; }
       
        

    }
}