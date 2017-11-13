using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace FirmaDAL
{
    public class Artikl
    {
        public Artikl()
        {
            this.Stavka = new HashSet<Stavka>();
        }
        public ArtiklDalProvider DalProvider { get; set; }
        public int SifArtikla { get; set; }
        public string NazArtikla { get; set; }
        public string JedMjere { get; set; }
        public decimal CijArtkila { get; set; }
        public bool ZastUsluga { get; set; }
        public byte[] SlikaArtikla { get; set; }
        public ImageSource SlikaArtiklaImage { get { return ByteToImage(SlikaArtikla); } }
        public string TekstArtikla { get; set; }

        public string Lookup
        {
            get
            {
                return NazArtikla + " (" + CijArtkila + "kn/" + JedMjere + ")";
            }
        }

        private ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage img = new BitmapImage();
            using(InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
            {
                using(DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(imageData);
                    writer.StoreAsync().GetResults();
                }
                img.SetSource(ms);
                return img;
            }
            
        }

        public virtual ICollection<Stavka> Stavka { get; set; }
    }
}
