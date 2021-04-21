namespace Contoso.Core
{
    public interface IBaseEntity
    {
        public int Id { get; }
        public byte[] RowVersion { get; }
    }
}