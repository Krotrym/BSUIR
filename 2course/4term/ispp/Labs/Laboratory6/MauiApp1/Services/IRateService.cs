using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratornay6.Services
{
    public interface IRateService
    {
        Task<IEnumerable<Rate>> GetRates(DateTime date);
    }

}
