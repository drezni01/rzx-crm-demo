namespace Rzx.Crm.Core.Exceptions
{
    public class CrmException:Exception
    {
        public CrmException(string msg) : base(msg, null) { }
    }
}
