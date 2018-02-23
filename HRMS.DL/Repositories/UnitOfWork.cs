using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.DL.AccountModels;
using HRMS.DL.Entities;

namespace TSMind.PB.CL.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        private GenericRepository<tblCertificateInputFieldDetails> tblCertificateInputFieldDetailsRepository;
        public GenericRepository<tblCertificateInputFieldDetails> TblCertificateInputFieldDetailsRepository
        {
            get
            {
                if (this.tblCertificateInputFieldDetailsRepository == null)
                    this.tblCertificateInputFieldDetailsRepository = new GenericRepository<tblCertificateInputFieldDetails>(context);
                return tblCertificateInputFieldDetailsRepository;
            }
        }


        private GenericRepository<tblMaCertificateTypes> tblMaCertificateTypesRepository;
        public GenericRepository<tblMaCertificateTypes> TblMaCertificateTypesRepository
        {
            get
            {
                if (this.tblMaCertificateTypesRepository == null)
                    this.tblMaCertificateTypesRepository = new GenericRepository<tblMaCertificateTypes>(context);
                return tblMaCertificateTypesRepository;
            }
        }

        private GenericRepository<tblMaCertificateSubTypes> tblMaCertificateSubTypesRepository;
        public GenericRepository<tblMaCertificateSubTypes> TblMaCertificateSubTypesRepository
        {
            get
            {
                if (this.tblMaCertificateSubTypesRepository == null)
                    this.tblMaCertificateSubTypesRepository = new GenericRepository<tblMaCertificateSubTypes>(context);
                return tblMaCertificateSubTypesRepository;
            }
        }

        private GenericRepository<tblMaInputFields> tblMaInputFieldsRepository;
        public GenericRepository<tblMaInputFields> TblMaInputFieldsRepository
        {
            get
            {
                if (this.tblMaInputFieldsRepository == null)
                    this.tblMaInputFieldsRepository = new GenericRepository<tblMaInputFields>(context);
                return tblMaInputFieldsRepository;
            }
        }



        public async Task<int> Save()
        {
            return await context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
