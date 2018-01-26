using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaBLL.Services
{
    public static class Validator
    {
        public static void ValidatePartner(Models.Partner partner)
        {
            // validate TipPartnera
            if (partner.TipPartnera == Models.TipPartnera.Nedefinirano)
                partner.Properties[nameof(partner.TipPartnera)].Errors.Add("Tip partnera je obavezno polje!");

            // validate IdMjestaPartnera
            if (partner.IdMjestaPartnera.HasValue)
            {
                // provjeri ako mjesto postoji, za to treba MjestoBllProvider
            }

            // validate IdMjestaIsporuke
            // isto kao i prije...

            // validate AdrPartnera
            if (!string.IsNullOrEmpty(partner.AdrPartnera) && partner.AdrPartnera.Length > 50)
                partner.Properties[nameof(partner.AdrPartnera)].Errors.Add(string.Format("Maksimalna duljina je {0}.", 50));

            // validate AdrIsporuke
            if (!string.IsNullOrEmpty(partner.AdrIsporuke) && partner.AdrIsporuke.Length > 50)
                partner.Properties[nameof(partner.AdrIsporuke)].Errors.Add(string.Format("Maksimalna duljina je {0}.", 50));

            // validate OIB
            if (string.IsNullOrEmpty(partner.OIB))
                partner.Properties[nameof(partner.AdrIsporuke)].Errors.Add("OIB je obavezno polje!");

            if (partner.OIB.Length != 11)
                partner.Properties[nameof(partner.OIB)].Errors.Add(string.Format("Duljina OIB-a treba biti {0}.", 11));

            if (!System.Text.RegularExpressions.Regex.IsMatch(partner.OIB, "^[0-9]*$"))
                partner.Properties[nameof(partner.OIB)].Errors.Add("Neispravan unos!");

            if (partner.TipPartnera == Models.TipPartnera.Osoba)
            {
                // validate ImeOsobe
                if (string.IsNullOrEmpty(partner.ImeOsobe))
                    partner.Properties[nameof(partner.ImeOsobe)].Errors.Add("Ime osobe je obavezno polje.");
                if (partner.ImeOsobe.Length > 20)
                    partner.Properties[nameof(partner.ImeOsobe)].Errors.Add(string.Format("Maksimalna duljina je {0}.", 20));

                // validate PrezmeOsobe
                if (string.IsNullOrEmpty(partner.PrezimeOsobe))
                    partner.Properties[nameof(partner.PrezimeOsobe)].Errors.Add("Prezime osobe je obavezno polje.");
                if (partner.PrezimeOsobe.Length > 20)
                    partner.Properties[nameof(partner.PrezimeOsobe)].Errors.Add(string.Format("Maksimalna duljina je {0}.", 20));
            }

            if (partner.TipPartnera == Models.TipPartnera.Tvrtka)
            {
                // validate NazivTvrtke
                if (string.IsNullOrEmpty(partner.NazivTvrtke))
                    partner.Properties[nameof(partner.NazivTvrtke)].Errors.Add("Naziv tvrtke je obavezno polje.");
                if (partner.NazivTvrtke.Length > 50)
                    partner.Properties[nameof(partner.NazivTvrtke)].Errors.Add(string.Format("Maksimalna duljina je {0}.", 50));

                // validate MatBrTvrtke
                if (string.IsNullOrEmpty(partner.MatBrTvrtke))
                    partner.Properties[nameof(partner.MatBrTvrtke)].Errors.Add("Matični broj je obavezno polje.");
                if (partner.MatBrTvrtke.Length > 50)
                    partner.Properties[nameof(partner.MatBrTvrtke)].Errors.Add(string.Format("Maksimalna duljina je {0}.", 30));
                if (!System.Text.RegularExpressions.Regex.IsMatch(partner.MatBrTvrtke, "^[0-9]*$"))
                    partner.Properties[nameof(partner.MatBrTvrtke)].Errors.Add("Neispravan unos.");
            }
        }
    }
}
