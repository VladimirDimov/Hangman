﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangman.Web.ViewModels.Games
{
    public class MakeGuessRequestViewModel
    {
        public int Index { get; set; }

        public char Letter { get; set; }
    }
}