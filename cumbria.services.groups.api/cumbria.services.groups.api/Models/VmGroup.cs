using cumbria.services.storage;

namespace cumbria.services.groups.api.Models
{
    public class VmGroup:Group
    {
        public bool IsMember { get; set; }
    }
}