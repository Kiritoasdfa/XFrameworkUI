using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework
{
    public class UICharacterThingList : UI, IAwake, ILoopScrollRectProvide<UIThingItem>
    {
        private List<int> thingList = new List<int>(100);

        public void Initialize()
        {
            for (int i = 0; i < 100; i++)
            {
                this.thingList.Add(10000 + i);
            }

            var loopRect = this.GetLoopScrollRect();
            loopRect.SetProvideData(UIPathSet.UIThingItem, this);
            loopRect.SetTotalCount(this.thingList.Count);
            loopRect.RefillCells();
        }

        protected override void OnClose()
        {
            this.thingList.Clear();
            base.OnClose();
        }

        void ILoopScrollRectProvide<UIThingItem>.ProvideData(UIThingItem ui, int index)
        {
            var id = this.thingList[index];
            ObjectHelper.Awake(ui, id);
        }
    }
}
