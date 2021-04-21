using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Contoso.Facade
{
    public abstract class NamedView : BaseEntityView
    {
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z -']*$")]
        [DisplayName("Name")]
        public string Name { get; set; }
    }
}