using DemoAPIDataAccess;

namespace MemLib
{
    public class MenuService : IMenuService
    {
        private IMenuDA _MenuDA;

        public MenuService(IMenuDA menuDA)
        {
            _MenuDA = menuDA;
        }

        public async Task<string> GetMenu()
        {
            return await _MenuDA.GetMenu();
        }
    }
}
