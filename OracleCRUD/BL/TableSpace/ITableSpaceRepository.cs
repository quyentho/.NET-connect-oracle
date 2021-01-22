using System.Collections.Generic;

namespace BL.TableSpace
{
    public interface ITableSpaceRepository
    {
        List<TableSpace> GetAll();
    }
}