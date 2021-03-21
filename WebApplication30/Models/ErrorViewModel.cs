using System;
using System.Collections.Generic;

namespace WebApplication30.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
    public class HomePageViewModel
    {
        public List<Ads> Ads { get; set; }
        public bool IsAuthenticated { get; set; }
        public Users CurrentUser { get; set; }
    }
}
