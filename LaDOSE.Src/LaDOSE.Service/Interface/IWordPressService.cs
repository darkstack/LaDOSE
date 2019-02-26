using System.Collections.Generic;
using LaDOSE.Entity.Wordpress;

namespace LaDOSE.Business.Interface
{
    public interface IWordPressService
    {
        List<WPEvent> GetWpEvent();
    }
}