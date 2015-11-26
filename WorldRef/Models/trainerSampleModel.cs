using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorldRef.Models
{
    public class trainerSampleModel
    {
        public int SampleId { get; set; }
        public int Id { get; set; }
        public string SampleAttach { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string SampleAttachPath { get; set; }
    }
}