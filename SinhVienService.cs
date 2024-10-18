using DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class SinhVienService
    {
        QuanLySVDB context=new QuanLySVDB();
        public List<Sinhvien> GetAll()
        {
            return context.Sinhvien.ToList();
        }
        public void InsertUpdate(Sinhvien s)
        {
            context.Sinhvien.AddOrUpdate(s);
            context.SaveChanges();
        }
        public void Delete(Sinhvien s)
        {
            context.Sinhvien.Remove(s);
            context.SaveChanges();
        }
        public List<Sinhvien> Timkiem(string keyword)
        {
            return context.Sinhvien.Where(s => s.MaSV.Contains(keyword) || s.HotenSV.Contains(keyword)).ToList();
        }

    }
}
