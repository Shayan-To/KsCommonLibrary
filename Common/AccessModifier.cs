namespace Ks.Common
{
    public enum AccessModifier
    {

        Public = 1,
        Private = 2,
        Protected = 4,
        Internal = 8,
        ProtectedInternal = Protected | Internal,
        PrivateProtected = Private | Protected

    }
}
