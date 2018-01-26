using FirmaBLL.Models;
using FirmaBLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Interfaces.Validation;

namespace FirmaBLL
{
    public class PartnerBllProvider
    {
        // public delegate void Validate(IValidatableModel model);

        public Partner FetchEmpty()
        {
            return new Partner(FirmaDAL.PartnerDalProvider.GenerateEmpty(), new List<LookupModel>(), e => Validator.ValidatePartner(e as Partner));
        }
        public Partner Fetch (int Id)
        {
            FirmaDAL.PartnerDalProvider dalProvider = new FirmaDAL.PartnerDalProvider();
            MjestoBllProvider mjestoBllProvider = new MjestoBllProvider();
            
            return new Partner(dalProvider.Fetch(Id), mjestoBllProvider.FetchLookup(), e => Validator.ValidatePartner(e as Partner));
        }

        public List<Partner> FetchAll()
        {
            FirmaDAL.PartnerDalProvider partnerDalProvider = new FirmaDAL.PartnerDalProvider();
            MjestoBllProvider mjestoBllProvider = new MjestoBllProvider();
            var mjestoLookupList = mjestoBllProvider.FetchLookup();
            var result = partnerDalProvider.FetchAll();
            List<Partner> partnerList = new List<Partner>();
            foreach(var partner in result)
                partnerList.Add(new Partner(partner, mjestoLookupList, e => Validator.ValidatePartner(e as Partner)));

            return partnerList;
        }

        public Partner FetchAtPosition<TKey>(int position)
        {
            throw new NotImplementedException();
        }

        public Partner FetchByPKs(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public int ItemsCount()
        {
            FirmaDAL.PartnerDalProvider dalProvider = new FirmaDAL.PartnerDalProvider();
            return dalProvider.ItemsCount();
        }

        public void SaveChanges(IEnumerable<Partner> changedItems, IEnumerable<Partner> newItems, IEnumerable<Partner> deletedItems)
        {

        }

        public void AddItem(Partner item)
        {
            FirmaDAL.PartnerDalProvider dalProvider = new FirmaDAL.PartnerDalProvider();
            dalProvider.AddItem(item.ToDTO());
        }

        public void UpdateItem(Partner item)
        {
            FirmaDAL.PartnerDalProvider dalProvider = new FirmaDAL.PartnerDalProvider();
            dalProvider.UpdateItem(item.ToDTO());
        }

        public void DeleteItem(Partner item)
        {
            FirmaDAL.PartnerDalProvider dalProvider = new FirmaDAL.PartnerDalProvider();
            dalProvider.DeleteItem(item.ToDTO());
        }

        // TODO: zamijeniti dictionary sa lookup model listom
        public Dictionary<int, string> FetchLookup()
        {
            FirmaDAL.PartnerDalProvider dalProvider = new FirmaDAL.PartnerDalProvider();
            return dalProvider.FetchLookup();
        }

        void Validate(IValidatableModel partner)
        {
            Validator.ValidatePartner(partner as Partner);
        }
    }
}
