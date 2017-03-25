using FYL.Entity.Enum;

namespace FYL.Framework.DataProvider
{
    public class PmhMobileDataProvider : BaseDataProvider
    {
        public static readonly PmhMobileDataProvider instance = new PmhMobileDataProvider();

        public PmhMobileDataProvider(EnumDataProviderType emProvider = EnumDataProviderType.PmhMobile) : base(emProvider)
        {

        }
    }
}
