using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Persistence
{
    public interface IRBFPersistenceDataAccess
    {
        Task<Tuple< System.Drawing.Color[], Model.Point[]> >  LoadAsync(string path);
        Task SaveAsync(string path, Tuple<System.Drawing.Color[], Model.Point[]> input);

    }
}
