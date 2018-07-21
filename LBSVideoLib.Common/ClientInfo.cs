﻿using LBFVideoLib.Common.Entity;
using System;
using System.Collections.Generic;
using System.IO;

namespace LBFVideoLib.Common
{
    [Serializable()]
    public class ClientInfo
    {
        public string EmailId { get; set; }

        public string Password { get; set; }

        public DateTime ExpiryDate { get; set; }

        public DateTime LastAccessStartTime { get; set; }

        public DateTime LastAccessEndTime { get; set; }

        public string SchoolName { get; set; }

        public string SchoolId { get; set; }

        public string SchoolCity { get; set; }

        public string SessionString { get; set; }

        public DateTime RegistrationDate { get; set; }

        public List<ClassFB> SelectedVideoDetails { get; set; }

        public string GetClientDetail()
        {
            return string.Format("Welcome, {0}, {1} [{2}]", this.SchoolName, this.SchoolCity, this.SchoolId);
        }

    }
}
