using FYL.Entity.Enum;

namespace FYL.Framework.DataProvider
{
    public class PmhMobileHotDataProvider : BaseDataProvider
    {
        public static readonly PmhMobileHotDataProvider instance = new PmhMobileHotDataProvider();

        public PmhMobileHotDataProvider(EnumDataProviderType emProvider = EnumDataProviderType.PmhMobileHot) : base(emProvider)
        {

        }
    }
}
