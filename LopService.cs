using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class LopService
    {
        QuanLySVDB context= new QuanLySVDB();
        public List<Lop> GetAll()
        {
            return context.Lop.ToList();
        }
    }
}
