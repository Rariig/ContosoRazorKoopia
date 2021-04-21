namespace Contoso.Core
{
    public interface IEntityData : IBaseEntity
    {
        public new int Id { get; set; }
        public new byte[] RowVersion { get; set; }
    }
}
