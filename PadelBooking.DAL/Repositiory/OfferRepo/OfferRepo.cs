using PadelBooking.DAL.Data;
using PadelBooking.DAL.Entities;
using PadelBooking.DAL.Repositiory.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.DAL.Repositiory.OfferRepo
{
    public class OfferRepo : GenericRepo<Offer>, IOfferRepo
    {
        public OfferRepo(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
