﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppBanwao.KaryKart.Web.Models
{
    public class SmsModel
    {
        public string Message { get; set; }

        public string Number { get; set; }

        public string IdtoBeSent { get; set; }
    }
}