using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TzedakahFund.Data
{
    public class TzedakahFundRepository
    {
        private string _connectionString;
        public TzedakahFundRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IEnumerable<Application> GetApplications(Status status)
        {
            using (var context = new TzedakahFundDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Application>(a => a.User);
                loadOptions.LoadWith<Application>(a => a.Category);
                context.LoadOptions = loadOptions;

                if (status == Status.Approved)
                {
                    return context.Applications.Where(a => a.Status == true).ToList();
                }
                else if (status == Status.Rejected)
                {
                    return context.Applications.Where(a => a.Status == false).ToList();
                }
                else
                {
                    return context.Applications.Where(a => a.Status == null).ToList();
                }
            }
        }
        //public IEnumerable<Application> GetApplications()
        //{
        //    using (var context = new TzedakahFundDataContext(_connectionString))
        //    {
        //        var loadOptions = new DataLoadOptions();
        //        loadOptions.LoadWith<Application>(a => a.User);
        //        loadOptions.LoadWith<Application>(a => a.Category);
        //        context.LoadOptions = loadOptions;
        //        return context.Applications.ToList();
        //    }
        //}
        public void AddApplication(Application a)
        {
            using (var context = new TzedakahFundDataContext(_connectionString))
            {
                a.Date = DateTime.Now;
                context.Applications.InsertOnSubmit(a);
                context.SubmitChanges();
            }
        }
        public IEnumerable<Category> GetCategories()
        {
            using (var context = new TzedakahFundDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Category>(c => c.Applications);
                context.LoadOptions = loadOptions;
                return context.Categories.ToList();
            }
        }
        public void AddCategory(string name)
        {
            using (var context = new TzedakahFundDataContext(_connectionString))
            {
                Category c = new Category()
                {
                    Name = name
                };
                context.Categories.InsertOnSubmit(c);
                context.SubmitChanges();
            }
        }
        public void EditCategory(string name, int id)
        {
            using (var context = new TzedakahFundDataContext(_connectionString))
            {
                context.ExecuteCommand("UPDATE Categories SET Name = {0} WHERE Id = {1}", name, id);
            }
        }
        public void DeleteCategory(int id)
        {
            using (var context = new TzedakahFundDataContext(_connectionString))
            {
                context.ExecuteCommand("DELETE Categories WHERE Id = {0}", id);
            }
        }
        public void AcceptApplication(int id)
        {
            using (var context = new TzedakahFundDataContext(_connectionString))
            {
                context.ExecuteCommand("UPDATE Applications SET Status = 0 WHERE Id = {0}", id);
            }
        }
        public void RejectApplication(int id)
        {
            using (var context = new TzedakahFundDataContext(_connectionString))
            {
                context.ExecuteCommand("UPDATE Applications SET Status = 1 WHERE Id = {0}", id);
            }
        }
    }

}
