﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XCManager.Models
{
    public class IndividualResult
    {
        public int Id { get; set; }
        public Runner runner { get; set; }
        public Team Team { get; set; }
        public Race Race { get; set; }
        public TimeSpan finishingTime { get; set; }
        public int Place { get; set; }
    }
}