﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaDAL
{
    public class Tvrtka : Partner
    {
        public string MatBrTvrtke { get; set; }
        public string NazivTvrtke { get; set; }
        public override string Naziv
        {
            get
            {
                return NazivTvrtke + " (" + MatBrTvrtke + ")";
            }
        }
    }
}
