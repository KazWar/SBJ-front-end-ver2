using RMS.EntityFrameworkCore;

namespace RMS.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly RMSDbContext _context;

        public InitialHostDbBuilder(RMSDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
