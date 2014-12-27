using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFramework.Constants.Entities.Session
{
    public interface IClient
    {
        int SessionId
        {
            get;
            set;
        }
    }
}
