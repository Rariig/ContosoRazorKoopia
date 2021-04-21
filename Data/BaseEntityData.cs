using Contoso.Core;
using System.ComponentModel.DataAnnotations;

namespace Contoso.Data
{
    public abstract class BaseEntityData : IEntityData
    {
        public int Id { get; set; }
        [Timestamp] public byte[] RowVersion { get; set; }
    }
}
