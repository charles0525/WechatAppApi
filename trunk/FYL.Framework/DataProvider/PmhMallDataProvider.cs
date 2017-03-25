using FYL.Entity.Enum;

namespace FYL.Framework.DataProvider
{
    public class PmhMallDataProvider : BaseDataProvider
    {
        public static readonly PmhMallDataProvider instance = new PmhMallDataProvider();

        public PmhMallDataProvider(EnumDataProviderType emProvider = EnumDataProviderType.PmhMall) : base(emProvider)
        {

        }
    }
}
