﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;

namespace TimaProject.ViewModels
{
    public interface IRecordViewModel : ITimeBase
    {
        string Title { get; set; }
        Project Project { get; set; }
    }
}
