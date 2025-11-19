namespace Luck.Framework;

public interface IAncillaryPaySuccessWithAncillaryScopeProvider
{
    Task<(bool, string)> AncillaryPaySuccessProviderAsync(string request, string originMessage);
}