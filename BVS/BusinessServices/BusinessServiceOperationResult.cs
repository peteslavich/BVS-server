using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BVS.BusinessServices
{
    public class BusinessServiceOperationResult
    {
        private List<string> _Errors;

        public List<string> Errors
        {
            get
            {
                if ( _Errors == null )
                {
                    _Errors = new List<string>();
                }
                return _Errors;
            }
        }

        public bool HasErrors
        {
            get
            {
                return (_Errors != null && _Errors.Count > 0);
            }
        }

        public void AddError(string error)
        {
            Errors.Add( error );
        }
    }
}