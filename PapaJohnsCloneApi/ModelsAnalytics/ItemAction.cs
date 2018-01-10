using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.ModelsAnalytics
{
    public partial class ItemAction
    {
        public ItemAction()
        {
            ItemDetail = new HashSet<ItemDetail>();
        }

        public int ItemActionId { get; set; }
        public string Name { get; set; }

        public ICollection<ItemDetail> ItemDetail { get; set; }
    }
}
