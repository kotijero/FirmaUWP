﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FirmaDAL
{
    public class Dokument
    {
        public Dokument()
        {
            this.Dokument1 = new HashSet<Dokument>();
            this.Stavke = new HashSet<Stavka>();
        }

        public int IdDokumenta { get; set; }
        public string VrDokumenta { get; set; }
        public int BrDokumenta { get; set; }
        public DateTime DatDokumenta { get; set; }
        public int IdPartnera { get; set; }
        public  Nullable<int> IdPrethDokumenta { get; set; }
        public decimal PostoPorez { get; set; }
        public decimal IznosDokumenta { get; set; }

        
        
        public virtual ICollection<Dokument> Dokument1 { get; set; }
        public virtual Dokument PrethodniDokument { get; set; }
        public virtual Partner Partner { get; set; }
        public virtual ICollection<Stavka> Stavke { get; set; }
        
        public string LookupData
        {
            get
            {
                return BrDokumenta + " - " + Partner.Naziv + " (" + DatDokumenta.ToShortDateString() + ")";
            }
        }
    }
}
