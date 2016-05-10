using ServiceApp.Domain.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Common
{
    public class Validations : IDisposable
    {
        private ServiceAppDBContext context;

        public bool ValidatePhoneNumberExists(string PhoneNumber)
        {
            context = new ServiceAppDBContext();

            bool IsPhoneNumberExists = false;
            try
            {
                int PhoneNumberExistsCount = (from user in context.AspNetUsers
                                              where user.PhoneNumber == PhoneNumber
                                              select user.PhoneNumber).Count();

                if (PhoneNumberExistsCount > 0)
                    IsPhoneNumberExists = true;

                return IsPhoneNumberExists;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GeneratePassword(string Email, string MobileNumber)
        {
            if (string.IsNullOrEmpty(MobileNumber))
                MobileNumber = "1234567890";

            string strChars = Email + "" + MobileNumber + "!@#$.";

            var random = new Random();
            StringBuilder strFinalChars = new StringBuilder();

            for (int i = 0; i < 3; i++)
            {
                strFinalChars.Append(strChars[random.Next(strChars.Length)]);
            }

            return Email.Substring(0, 1).ToUpper() + strFinalChars.ToString() + "." + MobileNumber.Substring(0, 2) + "#" + Email.Substring(2, 1).ToLower();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose() // Implement IDisposable
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
