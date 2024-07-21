using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WikkiDBLib.DBAccess
{
    public class DBAccessHelper<Model> where Model : class
    {
        private AppDbContext _dbContext;
        private DbSet<Model> _dbSet;

        public DBAccessHelper()
        {
            _dbContext = new();
            _dbSet = _dbContext.Set<Model>();
        }



        public bool Add(Model model)
        {
            try
            {
                if (model is null)
                    return false;

                _dbSet.Add(model);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add" + Environment.NewLine + ex.Message);
                return false;
            }
        }

        public bool DeleteByID(int ID)
        {
            try
            {
                if(ID <= 0) return false;

                var model = _dbSet?.Find(ID);
                if (model is null) return false;

                _dbSet.Remove(model);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex) 
            {
                MessageBox.Show("DeleteByID" + Environment.NewLine + ex.Message);
                return false;
            }
        }

        public bool Update(Model model)
        {
            try
            {
                if (model is null)
                    return false;

                _dbSet.Update(model);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update" + Environment.NewLine + ex.Message);
                return false;
            }
        }

        public Model? GetByID(int ID)
        {
            Model? model = null;

            try
            {
                if (ID > 0)
                {
                    model = _dbSet?.Find(ID);
                }

                return model;
            }
            catch (Exception ex)
            {
                MessageBox.Show("GetByID" + Environment.NewLine +  ex.Message);
                return model;
            }
        }
    }
}
